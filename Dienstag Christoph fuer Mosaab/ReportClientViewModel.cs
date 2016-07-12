using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace WebApplication1.Models.ViewModels
{
    [XmlRoot("MandatenListe")]
    [Serializable]
    [XmlInclude(typeof(ReportDepartmentViewModel))]
    public class ReportClientViewModel
    {
        [XmlIgnore]
        private List<ReportDepartmentViewModel> _ReportFields = new List<ReportDepartmentViewModel>();
        [XmlIgnore]
        public int Id { get; set; }
        [XmlArray("AbteilungsArray")]
        [XmlArrayItem("Personobjekt")]
        public List<ReportDepartmentViewModel> ReportFields
        {
            get { return _ReportFields; }
        }

        public void AddSummery()
        {
            _ReportFields.RemoveAll(d => d.Id == -1);//Removes all previously constructed summeries

            if (_ReportFields.Any())
            {
                ReportDepartmentViewModel summ = new ReportDepartmentViewModel()
                {
                    Id = -1,
                    Name = "GESAMT:",
                    NrOContracts = (from d in _ReportFields select d.NrOContracts).Sum(),
                    NrOSupervisingContracts = (from d in _ReportFields select d.NrOSupervisingContracts).Sum(),
                    NrAdaptable = (from d in _ReportFields select d.NrAdaptable).Sum(),
                    NrSoonEnding = (from d in _ReportFields select d.NrSoonEnding).Sum(),
                    ContractsSum = (from d in _ReportFields select d.ContractsSum).Sum(),
                    NrVarpayable = (from d in _ReportFields select d.NrVarpayable).Sum(),
                };
                _ReportFields.Add(summ);
            }
        }
    }
}