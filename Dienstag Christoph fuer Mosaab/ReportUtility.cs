using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using FileHelpers;
using System.Data.Entity;
using WebApplication1.Utilities.EventSystem;

namespace WebApplication1.Utilities
{
    public static class ReportUtility
    {
        public static int AdvanceDays = 100; //Number of days
        public static MyDbContext db = new MyDbContext();

        /// <summary>
        /// Generates a Report for a Department
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        public static ReportDepartmentViewModel ReportDataDept(int DepartmentId)
        {
            var departmentContracts = from c in db.Contracts
                                      where DepartmentId == c.DepartmentId || DepartmentId == c.SupervisorDepartmentId
                                      select c;
            ReportDepartmentViewModel result = new ReportDepartmentViewModel();
            result.Id = DepartmentId;
            result.Name = db.Departments.Find(DepartmentId).DepartmentName;
            result.NrOContracts = (from c in db.Contracts
                                   where DepartmentId == c.DepartmentId
                                   select c).Count();
            result.NrOSupervisingContracts = (from c in db.Contracts
                                              where DepartmentId == c.SupervisorDepartmentId
                                              select c).Count();
            result.NrAdaptable = (from c in departmentContracts
                                  where c.Adaptable.HasValue && c.Adaptable.Value
                                  select c).Count();
            result.ContractsSum = departmentSum(DepartmentId);
            result.NrSoonEnding = departmentContracts.Where(c => DbFunctions.DiffDays(c.ContractEnd ,DateTime.Now) <= AdvanceDays).Count();
                result.NrVarpayable = (from c in departmentContracts
                                       where c.VarPayable.HasValue && c.VarPayable.Value
                                       select c).Count();
            return result;
        }

        /// <summary>
        /// Returns the ReportData for all Departments of one Client
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns>All Reports ready for the Department</returns>
        public static ReportClientViewModel ReportDataClient(int ClientId)
        {
            ReportClientViewModel results = new ReportClientViewModel()
            {
                Id = ClientId
            };
            List<Department> departments = (from d in db.Departments
                                            where ClientId == d.Client.AccountingArea
                                            select d).ToList();
            foreach(Department d in departments)
            {
                results.ReportFields.Add(ReportDataDept(d.Id));
            }
            results.AddSummery();
            return results;
        }

        /// <summary>
        /// Creates the sum of t
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        private static double departmentSum(int DepartmentId)
        {
            var sum = (from c in db.Contracts
                       where c.DepartmentId == DepartmentId
                       //&& c.ContractStatus == "active"
                       select c.ContractValue).Sum();
            return sum.HasValue ? sum.Value : 0.0;
        }
        /// <summary>
        /// Exports a Client Report as xml
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public static string ReportAsXml(ReportClientViewModel report)
        {
            Type[] reportTypes = {typeof(ReportDepartmentViewModel) };
            using (StringWriter sw = new StringWriter(new StringBuilder()))
            {
                var serializer = new XmlSerializer(typeof(ReportClientViewModel), reportTypes);
                serializer.Serialize(sw, report);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Writes a report as a CSV file to a certrain path
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filepath"></param>
        public static void WriteReportAsCSV(ReportClientViewModel data, string filepath)
        {
            if (data.ReportFields.Any())
            {
                var engine = new FileHelperEngine<ReportDepartmentViewModel>()
                {
                    HeaderText = typeof(ReportDepartmentViewModel).GetCsvHeader()
                };
                engine.WriteFile(filepath, data.ReportFields);
            }
            else //No data to report
            {
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    sw.Write("0 Departments in this Client, report not possible");
                };
            }
        }

        /// <summary>
        /// Sends Reports of All Departments to any user that gets one
        /// </summary>
        public static void SendReports()
        {
            List<ReportDepartmentViewModel> dep_rep = new List<ReportDepartmentViewModel>();

            foreach (Department d in db.Departments)
            {
                dep_rep.Add(ReportDataDept(d.Id));
            }


            var filepaths = new List<ExportClientFilePathHelper>();
            foreach (Client c in db.Clients)
            {
                ReportClientViewModel rep = new ReportClientViewModel()
                {
                    Id = c.AccountingArea,
                };

                var departments = QueryUtility.GetDepartmentsFromClient(c.AccountingArea, db);
                rep.ReportFields.AddRange((from dr in dep_rep
                                           where departments.Any(d => dr.Id == d.Id)
                                           select dr).AsEnumerable()); //Mixed LINQ funtions and 
                rep.AddSummery();

                string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report" + c.ClientName + ".csv");
                //StringBuilder filepathwName = new StringBuilder(filepath);
                //filepathwName.Append("Report" + c.ClientName + ".csv");
                WriteReportAsCSV(rep, filepath);

                //Stores Filepaths for messages to multiple users
                ExportClientFilePathHelper help;
                help.ClientId = rep.Id;
                help.Filepath = filepath;
                filepaths.Add(help);
            }

            var reportusers = (from u in db.Users
                              where db.GetsReportsFromClient.Any(gr => gr.UserId == u.Id)
                              select u).ToList();
            
            //SendReports to every user
            foreach (ContractUser user in reportusers)
            {
                var reportclients = (from c in db.GetsReportsFromClient
                                    where c.UserId == user.Id
                                    select c.ClientId).ToList();
                List<string> pathsToExport = new List<string>();
                //Attach a report for any department with acces
                foreach (int clientId in reportclients)
                {
                    pathsToExport.Add(filepaths.Find(ecfh => ecfh.ClientId == clientId).Filepath);
                }
                MailUtility.SendReportMail(pathsToExport, user);
            }
            
            //Delete every tempory file
            foreach (ExportClientFilePathHelper ecfh in filepaths)
            {
                if (File.Exists(ecfh.Filepath))
                    File.Delete(ecfh.Filepath);
            }        
        }
    }

    public struct ExportClientFilePathHelper
    {
        public int ClientId;
        public string Filepath;
    }
}