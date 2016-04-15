using Newtonsoft.Json;
using SmartTrans_Importer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartTrans_Importer.Core
{
    
    public enum Status { OK, ERROR, PENDING };

    public class Calculator
    {
        public List<SmartTransRecord> ImportRecords { get; set; }
        public List<CollectImportRecord> CollectRecords { get; set; }

        public Status status;

        public Calculator(DateTime Date, String Agent)
        {
            try
            {
                string result = DataQuery.GetRunsheet(Date, Agent);
                ImportRecords = JsonConvert.DeserializeObject<List<SmartTransRecord>>(result);

                ImportRecords.RemoveAt(0);
                ImportRecords.RemoveAt(ImportRecords.Count - 1);

                status = Status.OK;

            }
            catch (Exception ex)
            {
                status = Status.ERROR;
            }
        }

        public void ComputeFields(DriverDB db)
        {

            CollectRecords = new List<CollectImportRecord>();

            foreach (var record in ImportRecords)
            {
                CollectImportRecord r = new CollectImportRecord();
                string reason = "OTHER", other_Code = "OTHER";

                if (record.Description.Length > 0)
                {
                    r.CompletedCount = ComputeCompletedCount(record.Description.Substring(0, 1));
                    r.AttemptedCount = ComputeAttemptedCount(record.Description.Substring(1, 1));
                }

                r.Identifier = ComputeIdentifier(record.Status, r.CompletedCount, r.AttemptedCount);

                // collect initials for driver, look up from driver db
                if (r.Identifier.Contains("Completed"))
                    r.Driver = db.FindDriverCode(record.Driver);

                r.File = record.OrderNumber;
                r.Date = record.DeliveryDate.ToShortDateString();
                r.Time = record.ETA;

                r.Comment1 = ComputeReasonText(record.Reasons);

                if (record.Reasons != null)
                    r = CalculateComments(record, r);

                if (r.Identifier == "Completed2")
                {
                    r.Comment8 = "Property Placed Under Seizure";
                }
                else
                {
                    r.Comment8 = "";
                }

                if (record.Reasons != null)
                    ComputeReasonandOtherCode(record.Reasons, out reason, out other_Code);

                r.Portal_Reason = reason;
                r.Other_Code = other_Code;

                r.Date2 = record.DeliveryDate.ToShortDateString();
                r.Date3 = record.DeliveryDate.ToShortDateString();

                CollectRecords.Add(r);
            }
        }

        private CollectImportRecord CalculateComments(SmartTransRecord record, CollectImportRecord r)
        {
            int stringLength = record.Reasons.Length;

            if (stringLength < 60)
            {
                r.Comment2 = record.Reasons;
            }
            else if (stringLength < 120)
            {
                r.Comment2 = record.Reasons.Substring(0, 60);
                r.Comment3 = record.Reasons.Substring(61, (stringLength - 60)); // this number has to be the remainder not 60
            }
            else if (stringLength < 180)
            {
                r.Comment2 = record.Reasons.Substring(0, 60);
                r.Comment3 = record.Reasons.Substring(61, 60);
                r.Comment4 = record.Reasons.Substring(121, (stringLength - 121)); // this number has to be the remainder not 60
            }
            else if (stringLength < 240)
            {
                r.Comment2 = record.Reasons.Substring(0, 60);
                r.Comment3 = record.Reasons.Substring(61, 60);
                r.Comment4 = record.Reasons.Substring(121, 60);
                r.Comment5 = record.Reasons.Substring(181, (stringLength - 181)); // this number has to be the remainder not 60

            }
            else if (stringLength < 300)
            {
                r.Comment2 = record.Reasons.Substring(0, 60);
                r.Comment3 = record.Reasons.Substring(61, 60);
                r.Comment4 = record.Reasons.Substring(121, 60);
                r.Comment5 = record.Reasons.Substring(181, 60);
                r.Comment6 = record.Reasons.Substring(241, (stringLength - 241)); // this number has to be the remainder not 60
            }
            else
            {
                r.Comment2 = record.Reasons.Substring(0, 60);
                r.Comment3 = record.Reasons.Substring(61, 60);
                r.Comment4 = record.Reasons.Substring(121, 60);
                r.Comment5 = record.Reasons.Substring(181, 60);
                r.Comment6 = record.Reasons.Substring(241, 60);
                r.Comment7 = record.Reasons.Substring(301, (stringLength - 301)); // this number has to be the remainder not 60
            }

            return r;
        }

        private string ComputeReasonText(string reasons)
        {
            string contents = string.Empty;

            XmlDocument document = new XmlDocument();

            if (!(reasons == null))
            {
                document.LoadXml(reasons);

                foreach (XmlNode child in document.DocumentElement.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        contents += child.InnerText;
                    }
                }

            }
            else
            {
                contents = "";
            }

            return contents;
        }

        private string ComputePortalReason()
        {
            throw new NotImplementedException();
        }

        private void ComputeReasonandOtherCode(string reasons, out string reason, out string other_Code)
        {
            reason = "";
            other_Code = "";

            if (reasons.Equals("Partnership Person in Charge"))
            {
                reason = "Business Principal";
                other_Code = "1";
            }
            else if (reasons.Equals("Partners Handing"))
            {
                reason = "Handing Partners";
                other_Code = "2";
            }
            else if (reasons.Equals("Individual"))
            {
                reason = "Individual";
                other_Code = "3";
            }
            else if (reasons.Equals("Pty Ltd Lawyer"))
            {
                reason = "Lawyer Corporation";
                other_Code = "4";
            }
            else if (reasons.Equals("Public Lawyer"))
            {
                reason = "Lawyer Public";
                other_Code = "5";
            }
            else if (reasons.Equals("Individual Lawyer"))
            {
                reason = "Lawyers Individual";
                other_Code = "6";
            }
            else if (reasons.Equals("Individual Person 18 - Residence"))
            {
                reason = "Person 18";
                other_Code = "19";
            }
            else if (reasons.Equals("Individual Person 18 - Business"))
            {
                reason = "Person 18";
                other_Code = "20";
            }
            else if (reasons.Equals("Pty Ltd- Person in Charge"))
            {
                reason = "Person Corporation";
                other_Code = "9";
            }
            else if (reasons.Equals("Pre Paid Post"))
            {
                reason = "Pre Paid Post";
                other_Code = "10";
            }
            else if (reasons.Equals("Pty Ltd - Registered Office"))
            {
                reason = "Registered Office";
                other_Code = "11";
            }
            else if (reasons.Equals("value 12"))
            {
                reason = "value 12";
            }
            else if (reasons.Equals("Individual Placed"))
            {
                other_Code = "12";
            }
            else if (reasons.Equals("Individual Prison"))
            {
                other_Code = "13";
            }
            else if (reasons.Equals("Individual Guardian"))
            {
                other_Code = "14";
            }
            else if (reasons.Equals("Individual Guardian Placed"))
            {
                other_Code = "15";
            }
            else if (reasons.Equals("Partners Placed"))
            {
                other_Code = "16";
            }
            else if (reasons.Equals("Public CEO"))
            {
                other_Code = "17";
            }
            else if (reasons.Equals("Pty Ltd - Registered Office - Unattended"))
            {
                other_Code = "18";
            }
            else
            {
                reason = "OTHER";
                other_Code = "OTHER";
            }
        }

        private int ComputeCompletedCount(string description)
        {
            int ans = 9;

            Int32.TryParse(description, out ans);

            return ans;

        }

        private int ComputeAttemptedCount(string description)
        {
            int ans = 9;

            Int32.TryParse(description, out ans);

            return ans;
        }

        private string ComputeIdentifier(string status, int completedcount, int attemptedcount)
        {
            if (status.Equals("Completed"))
            {
                if (completedcount < 8)
                {
                    return "Completed" + completedcount.ToString();
                }
                else
                {
                    return "EXCompleted" + completedcount.ToString();
                }
            }
            else if (status.Equals("Del Exception"))
            {
                if (completedcount < 8)
                {
                    return "Attempt" + attemptedcount.ToString();
                }
                else
                {
                    return "EXAttempt" + attemptedcount.ToString();
                }
            }

            return "OTHER";
        }

        public void ExportToFile()
        {

        }
    }
}
