using FileHelpers;
using System;

namespace SmartTrans_Importer.Core.Models
{
    [DelimitedRecord(",")]
    public class ExportRecord
    {
        public String Identifier { get; set; }
        public String File { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public String Comment1 { get; set; }
        public String Comment2 { get; set; }
        public String Comment3 { get; set; }
        public String Comment4 { get; set; }
        public String Comment5 { get; set; }
        public String Comment6 { get; set; }
        public String Comment7 { get; set; }
        public String Comment8 { get; set; }
        public String Portal_Reason { get; set; }
        public String Other_Code { get; set; }
        public String Date2 { get; set; }
        public String Date3 { get; set; }
        public String Driver { get; set; }
    }
}