using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Utilities.EventSystem;
using System.ComponentModel;
using FileHelpers;

namespace WebApplication1.Models
{
    [DelimitedRecord(",")]
    public class Contract
    {
        public int Id { get; set; }
        //David: Type of IntContractNum changed to String
        [Display(Name = "IntContractNum", ResourceType = typeof(Resources.Language))] //4.1
        public string IntContractNum { get; set; }

        [Display(Name = "ExtContractNum", ResourceType = typeof(Resources.Language))] //4.2
        public string ExtContractNum { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "ContractDescription", ResourceType = typeof(Resources.Language))] //4.3 Pflicht
        public string Description { get; set; }

        [Required]
        [Display(Name = "Signer", ResourceType = typeof(Resources.Language))] //4.7
        public string SignerId { get; set; }
        [Required]
        [Display(Name = "Owner", ResourceType = typeof(Resources.Language))] //4.8
        public string OwnerId { get; set; }
        [Display(Name = "AssignedDepartment", ResourceType = typeof(Resources.Language))] //4.9
        public Nullable<int> DepartmentId { get; set; }
        [Display(Name = "SupervisorDepartment", ResourceType = typeof(Resources.Language))] //4.10
        public Nullable<int> SupervisorDepartmentId { get; set; }
        [Display(Name = "ContractValue", ResourceType = typeof(Resources.Language))] //4.11
        public Nullable<double> ContractValue { get; set; }
        [Display(Name = "AnnualValue", ResourceType = typeof(Resources.Language))] //4.12 ??
        public Nullable<double> AnnualValue { get; set; }
        //Further Characteristics //4.13
        [Display(Name = "PrePayable", ResourceType = typeof(Resources.Language))]
        public Nullable<bool> PrePayable { get; set; }
        [Display(Name = "VarPayable", ResourceType = typeof(Resources.Language))]
        public Nullable<bool> VarPayable { get; set; }
        [Display(Name = "Adaptable", ResourceType = typeof(Resources.Language))]
        public Nullable<bool> Adaptable { get; set; }
        //characteristics:end
        [Display(Name = "PaymentBegin", ResourceType = typeof(Resources.Language))] //4.14
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] //Ober:Edit german Date format
        public Nullable<DateTime> PaymentBegin { get; set; }
        [Display(Name = "PaymentInterval", ResourceType = typeof(Resources.Language))] //4.15
        public Nullable<int> PaymentInterval { get; set; }

        [Display(Name = "ContractBegin", ResourceType = typeof(Resources.Language))] //4.19 Pflicht
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] //Ober:Edit german Date format
        public Nullable<DateTime> ContractBegin { get; set; }
        [Display(Name = "CancellationPeriod", ResourceType = typeof(Resources.Language))] //4.20
        public Period CancellationPeriod { get; set; }
        [Display(Name = "CancellationDate", ResourceType = typeof(Resources.Language))] //4.21
        public Nullable<DateTime> CancellationDate { get; set; }
        //Contract Duration in months
        [Display(Name = "MinContractDuration", ResourceType = typeof(Resources.Language))] //4.22
        [Range(12, int.MaxValue)]
        public Nullable<int> MinContractDuration { get; set; }
        [Display(Name = "ContractEnd", ResourceType = typeof(Resources.Language))] //4.23
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)] //Ober:Edit german Date format
        public Nullable<DateTime> ContractEnd { get; set; }
        [Display(Name = "AutoExtension", ResourceType = typeof(Resources.Language))] //4.24
        public Nullable<int> AutoExtension { get; set; }
        //Erinnerung?? //4.25
        //Escalation?? //4.26
        [Display(Name = "Tax", ResourceType = typeof(Resources.Language))] //4.27
        public Nullable<double> Tax { get; set; }
        [Display(Name = "Remarks", ResourceType = typeof(Resources.Language))] //4.28
        public string Remarks { get; set; }


        //Extra
        [Display(Name = "IsFrameContract", ResourceType = typeof(Resources.Language))]
        public Nullable<bool> IsFrameContract { get; set; }
        //Not relevant
        [Display(Name = "Dispatcher", ResourceType = typeof(Resources.Language))]
        public string DispatcherId { get; set; }



        //virtua: multiple Objects
        [Display(Name = "Signer", ResourceType = typeof(Resources.Language))] //4.7
        public virtual ContractUser Signer { get; set; }
        [Display(Name = "Owner", ResourceType = typeof(Resources.Language))] //4.8
        public virtual ContractUser Owner { get; set; }
        [Display(Name = "AssignedDepartment", ResourceType = typeof(Resources.Language))] //4.9
        public virtual Department Department { get; set; }
        [Display(Name = "SupervisorDepartment", ResourceType = typeof(Resources.Language))] //4.10
        public virtual Department SupervisorDepartment { get; set; }
        //virtual: multple Objects not relevant
        [Display(Name = "Dispatcher", ResourceType = typeof(Resources.Language))]
        public virtual ContractUser Dispatcher { get; set; }
        

        //virtual
        [Display(Name = "ContractType", ResourceType = typeof(Resources.Language))] //4.4 Pflicht
        public virtual ContractType ContractType { get; set; }
        [Display(Name = "SubType", ResourceType = typeof(Resources.Language))] //4.5
        public virtual ContractSubType ContractSubType { get; set; }
        [Display(Name = "ContractKind", ResourceType = typeof(Resources.Language))] //4.6 Pflicht
        public virtual ContractKind ContractKind { get; set; }
        //Multple of same type

        [Display(Name = "CostCenters", ResourceType = typeof(Resources.Language))] //4.16
        public virtual ICollection<ContractCostCenter_Relation> ContractCostCenter_Relations { get; set; }
        [Display(Name = "CostKind", ResourceType = typeof(Resources.Language))] //4.17
        public virtual CostKind CostKind { get; set; }
        [Display(Name = "ContractPartner", ResourceType = typeof(Resources.Language))] //4.18
        public virtual ContractPartner ContractPartner { get; set; }
        [Display(Name = "ContractStatus", ResourceType = typeof(Resources.Language))] //4.29
        public virtual ContractStatus ContractStatus { get; set; }

        //virtual Extra
        public virtual ICollection<ContractFile> ContractFiles { get; set; }
        //Moses : ContractTasks
        public virtual ICollection<ContractTask> ContractTasks { get; set; }
        public virtual ICollection<VarPayment> VarPayments { get; set; }

        //David: Framecontract
        public int? FrameContractId { get; set; }

        public virtual Contract FrameContract { get; set; }
        public virtual ICollection<Contract> SubContracts { get; set; }

        //David:
        [Display(Name = "RuntimeType", ResourceType = typeof(Resources.Language))] //Ober
        public Nullable<ContractRuntimeTypes> RuntimeType { get; set; } //Ober set nullable


        //David: Events for Tasks ***************************************************************************

        //Events for the DispatcherTask ------------------------------------------------ 1
        public delegate void DispatcherTaskEventHandler(object source, EventArgs args);
        public event DispatcherTaskEventHandler DispatcherTask;

        protected virtual void OnDispatcherTask() {
            if (DispatcherTask != null) {
                DispatcherTask(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerDispatcherTaskEvent() {
            this.DispatcherTask += EventUtility.OnDispatcherNeeded;
            OnDispatcherTask();
            this.DispatcherTask -= EventUtility.OnDispatcherNeeded;
        }


        //Events when the dispatcher is set ---------------------------------------------- 2
        public delegate void DispatcherSetEventHandler(object source, EventArgs args);
        public event DispatcherSetEventHandler DispatcherSet;

        protected virtual void OnDispatcherSet() {
            if (DispatcherSet != null) {
                DispatcherSet(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerDispatcherSetEvent() {
            this.DispatcherSet += EventUtility.OnDispatcherSet;
            this.DispatcherSet += MailUtility.OnDispatcherSet;
            this.DispatcherSet += EventUtility.MessengerOnContractCreated;
            OnDispatcherSet();
            this.DispatcherSet -= EventUtility.OnDispatcherSet;
            this.DispatcherSet -= MailUtility.OnDispatcherSet;
            this.DispatcherSet -= EventUtility.MessengerOnContractCreated;
        }

        //Event for Contract-Completion Task ---------------------------------------------- 3
        public delegate void ContractCompletionEventHandler(object source, EventArgs args);
        public event ContractCompletionEventHandler ContractToBeCompleted;

        protected virtual void OnContractToBeCompleted() {
            if (ContractToBeCompleted != null) {
                ContractToBeCompleted(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerContractToBeCompletedEvent() {
            this.ContractToBeCompleted += EventUtility.OnContractToBeCompleted;
            OnContractToBeCompleted();
            this.ContractToBeCompleted -= EventUtility.OnContractToBeCompleted;
        }

        //Event for "Files To Be Added" Task ---------------------------------------------- 4
        public delegate void FilesToBeAddedEventHandler(object source, EventArgs args);
        public event FilesToBeAddedEventHandler FilesToBeAdded;

        protected virtual void OnFilesToBeAdded() {
            if (FilesToBeAdded != null) {
                FilesToBeAdded(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerFilesToBeAddedEvent() {
            this.FilesToBeAdded += EventUtility.OnFilesToBeAdded;
            OnFilesToBeAdded();
            this.FilesToBeAdded -= EventUtility.OnFilesToBeAdded;
        }

        //Event when Contract Information is complete ------------------------------------ 5
        public delegate void ContractCompletedEventHandler(object source, EventArgs args);
        public event ContractCompletedEventHandler ContractCompleted;

        protected virtual void OnContractCompleted() {
            if (ContractCompleted != null) {
                ContractCompleted(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerContractCompletedEvent() {
            this.ContractCompleted += EventUtility.OnContractCompleted;
            this.ContractCompleted += EventUtility.MessengerOnContractCreated;
            OnContractCompleted();
            this.ContractCompleted -= EventUtility.OnContractCompleted;
            this.ContractCompleted -= EventUtility.MessengerOnContractCreated;
        }

        //Event when Contract-File is added --------------------------------------------- 6
        public delegate void FilesAddedEventHandler(object source, EventArgs args);
        public event FilesAddedEventHandler FilesAdded;

        protected virtual void OnFilesAdded() {
            if (FilesAdded != null) {
                FilesAdded(this, EventArgs.Empty);
            }
        }

        //Add Listeners here
        public void TriggerFilesAddedEvent() {
            this.FilesAdded += EventUtility.OnFilesAdded;
            OnFilesAdded();
            this.FilesAdded -= EventUtility.OnFilesAdded;
        }

        //David: Events for Tasks ***********************************************************************ENDE

        public override bool Equals(Object o)
        {
            var item = o as Contract;
            if (item != null)
            {
                return item.Id == Id;
            }
            return false;
        }

        public static bool operator ==(Contract a, Contract b)
        {
            if (((object)a == null) && ((object)b == null)) {
                return true;
            } else if ((object)a == null || (object)b == null) {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator!=(Contract a, Contract b)
        {
            return !(a == b);
        } 

    }

    //David: Nur als Info zur Art der Laufzeit des Vertrags
    public enum ContractRuntimeTypes
    {
        //wird in db als int gespeichert
        //Ober changed order unlimited with fixedTermWithP...
        [Display(Name = "fixedTerm", ResourceType = typeof(Resources.Language))]
        [Description("feste Laufzeit")]
        fixedTerm = 1,  //= 1 feste Laufzeit  
        [Display(Name = "fixedTermWithPrematureTermination", ResourceType = typeof(Resources.Language))]
        [Description("feste Laufzeit mit vorz. Kündigung")]
        fixedTermWithPrematureTermination = 2,  //= 2 feste Laufzeit mit vorz. Kuendigung
        [Display(Name = "unlimitedWithAutoExtension", ResourceType = typeof(Resources.Language))]
        [Description("unbefristet mit stillschweigender Verlängerung")]
        unlimited = 3  //= 3 unbefristet mit stillschweigender Verlaengerung
    }
}