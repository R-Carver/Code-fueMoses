using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Quartz;
using WebApplication1.Quartz.Jobs;
using WebApplication1.Utilities.SignalR;

namespace WebApplication1.Utilities.EventSystem
{
    public class EventUtility
    {
        
        //DAVID TaskTest *************************************************************************************
        public static void OnDispatcherNeeded(object source, EventArgs e)
        {
            MyDbContext db = new MyDbContext();

            var contract = (Contract)source;

            //Get contract from db to check wheter the saving worked and the status is set
            contract = db.Contracts.Find(contract.Id);

            if(contract != null && contract.ContractStatus.Id == 1)
            {
                var tempTask = new ContractTask();

                tempTask.Description = "Dispatcher hinzufuegen";
                tempTask.TaskType = TaskTypes.DispatcherZuweisen;
                tempTask.Contract = contract;
                tempTask.User = contract.Owner;
                tempTask.IsDone = false;

                tempTask.DateCreated = DateTime.Now;

                tempTask.NotificationDate = DateTime.Now.AddSeconds(Constants.LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST);
                tempTask.EscalationDate = DateTime.Now.AddSeconds(Constants.LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST * 2);

                //for Testing only - for Real App uncomment the Line after
                tempTask.Expiring = DateTime.Now.AddMinutes(Constants.LAUFZEIT_DISPATCHERTASK_MINUTEN_TEST);
                //tempTask.Expiring = DateTime.Now.AddDays(Constants.LAUFZEIT_DISPATCHERTASK_TAGE);

                db.ContractTasks.Add(tempTask);
                db.SaveChanges();

                tempTask.TriggerTaskCreatedEvent();

                System.Diagnostics.Debug.WriteLine("<Dispatcher zuweisen> Aufgabe erstellt");  
            }

        }

        public static void OnDispatcherSet(object source, EventArgs e)
        {
            //Not implemented yet - Mark Task as Done 
            MyDbContext db = new MyDbContext();

            var contract = (Contract)source;

            var task = db.ContractTasks.Where(t => t.Contract.Id == contract.Id && t.TaskType == TaskTypes.DispatcherZuweisen).SingleOrDefault();
            task.IsDone = true;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            //When the Dispatcher is set, create the tasks for him for completing the contract and uploading the files
            contract.TriggerContractToBeCompletedEvent();
            contract.TriggerFilesToBeAddedEvent();

            //take care that the task gets removed
            task.TriggerTaskDoneEvent();

            System.Diagnostics.Debug.WriteLine("Dispatcher gewaehlt");
        }

        public static void OnContractToBeCompleted(object source, EventArgs e)
        {
            MyDbContext db = new MyDbContext();

            var contract = (Contract)source;

            //Get contract from db to check wheter the saving worked and the status is set
             var contractForDbCheck = db.Contracts.Find(contract.Id);

            if (contractForDbCheck != null && contractForDbCheck.ContractStatus.Id == 1)
            {
                var tempTask = new ContractTask();

                //The dispatcher has to be loaded here because the nav property is not set at this point yet
                ContractUser dispatcher = db.Users.Find(contract.DispatcherId);

                tempTask.Description = "Vertrag vervollstaendigen";
                tempTask.TaskType = TaskTypes.VertragVervollstaendigen;
                tempTask.Contract = contractForDbCheck;
                tempTask.User = dispatcher;
                tempTask.IsDone = false;

                tempTask.DateCreated = DateTime.Now;

                tempTask.NotificationDate = DateTime.Now.AddSeconds(Constants.LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST * 2);
                tempTask.EscalationDate = DateTime.Now.AddSeconds(Constants.LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST * 3);

                //for Testing only - for Real App uncomment the Line after
                tempTask.Expiring = DateTime.Now.AddMinutes(Constants.LAUFZEIT_DISPATCHERTASK_MINUTEN_TEST);
                //tempTask.Expiring = DateTime.Now.AddDays(Constants.LAUFZEIT_DISPATCHERTASK_TAGE);

                db.ContractTasks.Add(tempTask);
                db.SaveChanges();

                //take care that the task gets removed
                tempTask.TriggerTaskCreatedEvent();

                System.Diagnostics.Debug.WriteLine("<Vertrag vervollstaendigen> Aufgabe erstellt");
            }

        }

        public static void OnContractCompleted(object source, EventArgs e)
        {
            //Not implemented yet - Mark Task as Done 
            MyDbContext db = new MyDbContext();

            var contract = (Contract)source;

            var task = db.ContractTasks.Where(t => t.Contract.Id == contract.Id && t.TaskType == TaskTypes.VertragVervollstaendigen).SingleOrDefault();
            task.IsDone = true;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            //take care that the task gets removed
            task.TriggerTaskDoneEvent();

            System.Diagnostics.Debug.WriteLine("Vertrag vervollstaendigt");
        }

        public static void OnFilesToBeAdded(object source, EventArgs e)
        {
            MyDbContext db = new MyDbContext();

            var contract = (Contract)source;
            
            //Get contract from db to check wheter the saving worked and the status is set
            var contractForDbCheck = db.Contracts.Find(contract.Id);

            if (contractForDbCheck != null && contractForDbCheck.ContractStatus.Id == 1)
            {
                var tempTask = new ContractTask();

                //The dispatcher has to be loaded here because the nav property is not set at this point yet
                ContractUser dispatcher = db.Users.Find(contract.DispatcherId);

                tempTask.Description = "Dokument hochladen";
                tempTask.TaskType = TaskTypes.DokumenteHochladen;
                tempTask.Contract = contractForDbCheck;
                tempTask.User = dispatcher;
                tempTask.IsDone = false;

                tempTask.DateCreated = DateTime.Now;

                tempTask.NotificationDate = DateTime.Now.AddSeconds(Constants.LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST);
                tempTask.EscalationDate = DateTime.Now.AddSeconds(Constants.LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST * 2);

                //for Testing only - for Real App uncomment the Line after
                tempTask.Expiring = DateTime.Now.AddMinutes(Constants.LAUFZEIT_DISPATCHERTASK_MINUTEN_TEST);
                //tempTask.Expiring = DateTime.Now.AddDays(Constants.LAUFZEIT_DISPATCHERTASK_TAGE);

                db.ContractTasks.Add(tempTask);
                db.SaveChanges();

                //take care that the task gets removed
                tempTask.TriggerTaskCreatedEvent();

                System.Diagnostics.Debug.WriteLine("<Datein hochladen> Aufgabe erstellt");
            }

        }

        public static void OnFilesAdded(object source, EventArgs e)
        {
            //Not implemented yet - Mark Task as Done 
            MyDbContext db = new MyDbContext();

            var contract = (Contract)source;

            var task = db.ContractTasks.Where(t => t.Contract.Id == contract.Id && t.TaskType == TaskTypes.DokumenteHochladen).SingleOrDefault();
            task.IsDone = true;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            //take care that the task gets removed
            task.TriggerTaskDoneEvent();

            System.Diagnostics.Debug.WriteLine("Vertragsdokument hochgeladen");
        }


        //------------------------------- General Events ---------------------------------------  

        public static void OnTaskDone(object source, EventArgs e)
        {
            //when task is marked as done schedule the remove process some time after
            ContractTask tempTask = (ContractTask)source;

            System.Diagnostics.Debug.WriteLine("Aufgabe "+ tempTask.TaskType.ToString() + " wird bald geloescht");

            //Call the Scheduler Singleton and Instantiate the job
            IScheduler scheduler = Scheduler.Instance();
            IJobDetail job = JobBuilder.Create<RemoveTask>().Build();

            //Pass the Id of the task to the job for deleting it
            job.JobDataMap["TaskId"] = tempTask.Id;

            //for Testing only - for Real App uncomment the Line after
            scheduler.ScheduleJob(job, Triggers.GetTriggerWithSecondOffset(Constants.LAUFZEIT_ERLEDIGTER_VERTRAG_SICHTBAR_SEKUNDEN_TEST));
            //scheduler.ScheduleJob(job, Triggers.GetTriggerWithSecondOffset(Constants.LAUFZEIT_ERLEDIGTER_VERTRAG_SICHTBAR_TAGE));
        }

        public static void OnTaskCreated(object source, EventArgs e)
        {
            //make sure that task is removed on expiration date
            ContractTask tempTask = (ContractTask)source;

            //Call the Scheduler Singleton and Instantiate the job
            IScheduler scheduler = Scheduler.Instance();


            //Job for Removing the Task when it expires  =========================================  1
            IJobDetail job = JobBuilder.Create<RemoveTask>().Build();

            //Pass the Id of the task to the job for deleting it
            job.JobDataMap["TaskId"] = tempTask.Id;
            scheduler.ScheduleJob(job, Triggers.GetTriggerAtDate((DateTime)tempTask.Expiring));

            //Job for Email Notification for Owner  ==============================================  2
            IJobDetail notificationJob = JobBuilder.Create<NotifyTaskOwner>().Build();

            //Pass the Id of the task to the job for deleting it
            notificationJob.JobDataMap["TaskId"] = tempTask.Id;
            scheduler.ScheduleJob(notificationJob, Triggers.GetTriggerAtDate((DateTime)tempTask.NotificationDate));

            //Job for Messenger Note on Notificationdate  ========================================  3
            IJobDetail messengerNoteJob = JobBuilder.Create<NotificationMessage>().Build();

            messengerNoteJob.JobDataMap["TaskId"] = tempTask.Id;
            scheduler.ScheduleJob(messengerNoteJob, Triggers.GetTriggerAtDate((DateTime)tempTask.NotificationDate));

            //Job for Email on Escalation Date    ================================================  4
            IJobDetail escalationJob = JobBuilder.Create<Escalation>().Build();

            escalationJob.JobDataMap["TaskId"] = tempTask.Id;
            scheduler.ScheduleJob(escalationJob, Triggers.GetTriggerAtDate((DateTime)tempTask.EscalationDate));

        }

        //------------------------------- Messenger Events ---------------------------------------

        public static void MessengerOnContractCreated(object source, EventArgs e)
        {
            Messenger messenger = Messenger.Instance;
            var contract = (Contract)source;

            string owner = contract.Owner.UserName;

            messenger.AddToMessenger(DateTime.Now, "<b>" + owner + "</b>" + " hat neuen Vertrag erstellt");

            System.Diagnostics.Debug.WriteLine("Benachrichtigung sollte erscheinen");
        }


        //DAVID TaskTest *********************************************************************************ENDE
    }
}