using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileHelpers;
using System.ComponentModel;
using System.Xml.Serialization;

namespace WebApplication1.Models.ViewModels
{
    [DelimitedRecord(",")]
    public class ReportDepartmentViewModel
    {
        [FieldHidden]
        public int Id;

        [FieldOrder(1), FieldTitle("Abteilung")]
        public string Name;
        
        [XmlAttribute("Anzahl zugeordn. Verträge")]
        [FieldOrder(2), FieldTitle("Anzahl zugeordn. Verträge")]
        public int NrOContracts;//Number of contracts linked to this Department
        
        [XmlAttribute("Anzahl überw. Verträge")]
        [FieldOrder(3), FieldTitle("Anzahl überw.Verträge")]
        public int NrOSupervisingContracts;//Number of Contracts this Department supervises
        
        [XmlAttribute("Anzahl anpassbarer Verträge")]
        [FieldOrder(4), FieldTitle("Anzahl anpassb. Verträge")]
        public int NrAdaptable;
        
        [XmlAttribute("Anzahl Verträge mit variablen Kosten")]
        [FieldOrder(5), FieldTitle("Anzahl Verträge mit variablen Kosten")]
        public int NrVarpayable;
        
        [XmlAttribute("Gesamtbetrag ü.a. Verträge")]
        [FieldOrder(6), FieldTitle("Gesamtbetrag ü.a. Verträge")]
        public double ContractsSum;//Sum of the amount of money all contracts in this Department together
        
        [XmlAttribute("Enden in 100 Tagen")]
        [FieldOrder(7), FieldTitle("Enden in 100Tagen")]
        public int NrSoonEnding; //Number of contracts, ending in a specified amount of time (start <90 days)


    }
}