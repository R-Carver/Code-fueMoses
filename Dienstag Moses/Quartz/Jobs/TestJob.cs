using Postal;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Utilities.EventSystem;
using WebApplication1.Utilities.SignalR;

namespace WebApplication1.Quartz.Jobs
{
    public class TestJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Diagnostics.Debug.WriteLine("Aufgabe wird geloescht ");

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int taskKey = dataMap.GetInt("TaskId");

            MyDbContext db = new MyDbContext();

            ContractTask task = db.ContractTasks.Find(taskKey);
            db.ContractTasks.Remove(task);
            db.SaveChanges();

            System.Diagnostics.Debug.WriteLine("Aufgabe wurde geloescht ");
        }

    }

    public class RemoveTask : IJob
    {   
        /// <summary>
        /// 
        /// Checks if the Task stil exists and removes it if it does
        /// 
        /// </summary>
        /// <param name="context"></param>
        
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int taskKey = dataMap.GetInt("TaskId");

            MyDbContext db = new MyDbContext();

            var task = db.ContractTasks.Find(taskKey);

            if(task != null)
            {
                db.ContractTasks.Remove(task);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("Aufgabe " + task.TaskType.ToString() + " wurde geloescht ");
            }
        }
    }

    public class NotifyTaskOwner : IJob
    {
        /// <summary>
        /// 
        /// Checks if the Task stil exists and if its not done yet.
        /// Then it sends a notification to the responsible person
        /// 
        /// </summary>
        /// <param name="context"></param>

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int taskKey = dataMap.GetInt("TaskId");

            MyDbContext db = new MyDbContext();

            var task = db.ContractTasks.Find(taskKey);

            if (task != null && task.IsDone == false)
            {
                MailUtility.NotifyTaskOwner(task);

                System.Diagnostics.Debug.WriteLine("Benachrichtigung ueber ablaufende Aufgabe abgeschickt");
            }
        }
    }

    public class NotificationMessage : IJob
    {
        /// <summary>
        /// 
        /// Messenger Note on Task Notification
        /// 
        /// </summary>
        /// <param name="context"></param>

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int taskKey = dataMap.GetInt("TaskId");

            MyDbContext db = new MyDbContext();

            var task = db.ContractTasks.Find(taskKey);

            if (task != null && task.IsDone == false)
            {
                Messenger messenger = Messenger.Instance;
                messenger.AddToMessenger(DateTime.Now, "Aufgabe " + "<b>" + task.Description + "</b>" + " muss bald erledigt werden");
            }
        }
    }

    public class Escalation : IJob
    {
        /// <summary>
        /// 
        /// Checks if the Task stil exists and if its not done yet.
        /// Then it sends a notification to the responsible person
        /// 
        /// </summary>
        /// <param name="context"></param>

        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            int taskKey = dataMap.GetInt("TaskId");

            MyDbContext db = new MyDbContext();

            var task = db.ContractTasks.Find(taskKey);

            if (task != null && task.IsDone == false)
            {
                var contract = db.Contracts.Find(task.Contract.Id);
                var contractOwner = contract.Owner;
                if (contractOwner == null)
                {
                    return;
                }

                MailUtility.NotifyOnEscalation(task, contractOwner.Email);
                System.Diagnostics.Debug.WriteLine("Benachrichtigung ueber Eskalation abgeschickt");
            }
        }
    }
}