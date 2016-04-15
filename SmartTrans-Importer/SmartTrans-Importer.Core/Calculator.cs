using Newtonsoft.Json;
using SmartTrans_Importer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrans_Importer.Core
{
    
    public enum Status { OK, ERROR, PENDING };

    public class Calculator
    {
        public List<SmartTransRecord> ImportRecords { get; set; }
        public List<CollectImportRecord> CollectRecords { get; set; }

        public DriverDB Drivers;
        public Status status;

        public Calculator(DateTime Date, String Agent)
        {
            try
            {
                string result = DataQuery.GetRunsheet(Date, Agent);
                ImportRecords = JsonConvert.DeserializeObject<List<SmartTransRecord>>(result);

                status = Status.OK;

            }
            catch (Exception ex)
            {
                status = Status.ERROR;
            }
        }

        public void ComputeFields()
        {
            foreach (var record in ImportRecords)
            {
                CollectImportRecord r = new CollectImportRecord();

                r.CompletedCount = ComputeCompletedCount();
                r.AttemptedCount = ComputeAttemptedCount();

                r.Identifier = ComputeIdentifier();

                r.File = record.OrderNumber;
                r.Date = record.DeliveryDate.ToShortDateString();
                r.Time = record.ETA;

                r.Comment1 = record.Reasons;

                r.Comment2 = record.Comments.Substring(0, 60);
                r.Comment3 = record.Comments.Substring(61, 60);
                r.Comment4 = record.Comments.Substring(121, 60);
                r.Comment5 = record.Comments.Substring(181, 60);
                r.Comment6 = record.Comments.Substring(241, 60);
                r.Comment7 = record.Comments.Substring(301, 60);

                if (r.Identifier == "Completed2")
                {
                    r.Comment8 = "Property Placed Under Seizure";
                }
                else
                {
                    r.Comment8 = "";
                }

                r.Portal_Reason = ComputePortalReason();

                r.Other_Code = ComputerOtherCode();

                r.Date2 = record.DeliveryDate.ToShortDateString();
                r.Date3 = record.DeliveryDate.ToShortDateString();

                CollectRecords.Add(r);
            }
        }

        private string ComputePortalReason()
        {
            throw new NotImplementedException();
        }

        private string ComputerOtherCode()
        {
            throw new NotImplementedException();
        }

        private int ComputeAttemptedCount()
        {
            throw new NotImplementedException();
        }

        private int ComputeCompletedCount()
        {
            throw new NotImplementedException();
        }

        private string ComputeIdentifier()
        {
            throw new NotImplementedException();
        }

        public void ExportToFile()
        {

        }
    }
}
