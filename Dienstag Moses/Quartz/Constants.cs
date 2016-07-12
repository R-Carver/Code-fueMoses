using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Utilities
{
    public static class Constants
    {   
        /*
        Die ersten Konstanten sind dafuer da falls das Programm tatsaechlich mal eingesetzt wuerde.
        Zum Testen und Vorfuehren macht es natuerlich keinen Sinn die Aufgaben mehrere Tage lang 
        laufen zu lassen, da man keinen Effekt sehen wird.
        Zum Vorfuehren nutze die Konstanten aus dem Testbereich unten.
            */

        //Laufzeit in Tagen die eine Aufgabe bestehen soll
        public const int LAUFZEIT_DISPATCHERTASK_TAGE = 7;

        //Anzahl der Tage die eine Aufgabe noch besteht(sichtbar ist) nachdem Sie erledigt wurde
        public const int LAUFZEIT_ERLEDIGTER_VERTRAG_SICHTBAR_TAGE = 1;



        /*
        Konstanten zum TESTEN
            */

        //Laufzeit in Sekunden die eine Aufgabe bestehen soll
        public const int LAUFZEIT_DISPATCHERTASK_SEKUNDEN_TEST = 50;

        //Laufzeit in Sekunden die eine Aufgabe bestehen soll
        public const int LAUFZEIT_DISPATCHERTASK_MINUTEN_TEST = 360;

        //Anzahl der Sekunden die eine Aufgabe noch besteht(sichtbar ist) nachdem Sie erledigt wurde
        public const int LAUFZEIT_ERLEDIGTER_VERTRAG_SICHTBAR_SEKUNDEN_TEST = 30;
    }
}