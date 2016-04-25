using FileHelpers;
using SmartTrans_Importer.Core.Models;
using System;
using System.Text;

namespace SmartTrans_Importer.Core
{
    public class Exporter
    {
        public static void ExporttoCsv(Calculator calc)
        {
            var engine = new FileHelperAsyncEngine<ExportRecord>(Encoding.UTF8);

            engine.HeaderText = CreateHeader();

            string path = Settings.Default.CsvExportLocation;

            
            // Create the file name from the agent ID and date
            string filename = GetFileName(calc.CollectRecords[0].Driver, calc.CollectRecords[0].Date, path);

            using (engine.BeginWriteFile(filename))
            {
                foreach (var record in calc.CollectRecords)
                {
                    ExportRecord exportRecord = Exporter.CreateFromCollectImportRecord(record);
                    engine.WriteNext(exportRecord);
                }
            }
        }

        public static string GetFileName(string _driver, string _date, string path)
        {
            string driver = _driver;
            string date = _date;

            DateTime date2;

            DateTime.TryParse(date, out date2);

            int dow = (int)date2.DayOfWeek;

            string filename = path + driver + "-" + date2.ToString("yyyyMMdd") + "-" + dow.ToString() + ".csv";

            return filename;
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
