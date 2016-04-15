using FileHelpers;
using SmartTrans_Importer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartTrans_Importer.Core.Models.ExportRecord;

namespace SmartTrans_Importer.Core
{
    public class Exporter
    {
        public static void ExporttoCsv(Calculator calc)
        {
            var engine = new FileHelperAsyncEngine<ExportRecord>(Encoding.UTF8);

            engine.HeaderText = CreateHeader();

            string path = SmartTrans_Importer.Properties.Settings.Default.CsvExportLocation;

            using (engine.BeginWriteFile(path))
            {
                foreach (var record in calc.CollectRecords)
                {
                    ExportRecord exportRecord = Exporter.CreateFromCollectImportRecord(record);
                    engine.WriteNext(exportRecord);
                }
            }
        }

        private static ExportRecord CreateFromCollectImportRecord(CollectImportRecord c_record)
        {
            ExportRecord record = new ExportRecord();

            record.Comment1 = c_record.Comment1;
            record.Comment2 = c_record.Comment2;
            record.Comment3 = c_record.Comment3;
            record.Comment4 = c_record.Comment4;
            record.Comment5 = c_record.Comment5;
            record.Comment6 = c_record.Comment6;
            record.Comment7 = c_record.Comment7;
            record.Comment8 = c_record.Comment8;
            record.Date = c_record.Date;
            record.Date2 = c_record.Date2;
            record.Date3 = c_record.Date3;
            record.Driver = c_record.Driver;
            record.File = c_record.File;
            record.Identifier = c_record.Identifier;
            record.Other_Code = c_record.Other_Code;
            record.Portal_Reason = c_record.Portal_Reason;
            record.Time = c_record.Time;

            return record;

        }

        public static string CreateHeader()
        {
            return @"Identifier,File,Date,Time,Comment1,Comment2,Comment3,Comment4,Comment5,Comment6,Comment7,Comment8,Portal Reason,Other Code,Date2,Date3,Driver";
        }
    }
}
