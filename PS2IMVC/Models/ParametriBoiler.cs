using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PS2IMVC.Models
{
    public class ParametriBoiler
    {
        public static Boolean SystemEnable { get; set; }
        public static Boolean PompaG1 { get; set; }
        public static Boolean ValvaK1 { get; set; }
        [Range(0, 100)]
        public static int P1 { get; set; }
        [Range(0, 100)]
        public static int P2 { get; set; }
        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2})?$")]
        public static decimal Capacitate { get; set; }
        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2})?$")]
        public static decimal DebitMaxP1 { get; set; }
        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2})?$")]
        public static decimal DebitMaxP2 { get; set; }
        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2})?$")]
        public static decimal NivelCurent { get; set; }
        public static decimal PragB1 { get; set; }
        public static decimal PragB2 { get; set; }
        public static decimal PragB3 { get; set; }
        public static decimal PragB4 { get; set; }
    }
}