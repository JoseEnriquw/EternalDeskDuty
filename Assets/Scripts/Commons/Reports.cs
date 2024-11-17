using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Reports 
{
    //Si los imprimio
    public static bool  _PrintSanchezReport {  get;  set; }
    public static bool _PrintMartinezReports { get; set; }
    public static bool _PrintBossReports { get; set; }

    //si los agarro de la impresora 

    public static bool HasPrintSanchezReport { get; set; }
    public static bool HasPrintMartinezReports { get; set; }
    public static bool HasPrintBossReports { get; set; }
}
