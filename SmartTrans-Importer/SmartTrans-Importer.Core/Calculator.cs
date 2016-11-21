using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        public Calculator()
        {
            ImportRecords = new List<SmartTransRecord>();
            CollectRecords = new List<CollectImportRecord>();
        }

        public Calculator Calculate(DateTime Date, String Agent)
        {
            try
            {
                string result = DataQuery.GetRunsheet(Date, Agent);
                var format = "dd/MM/yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                ImportRecords = JsonConvert.DeserializeObject<List<SmartTransRecord>>(result, dateTimeConverter);

                ImportRecords.RemoveAt(0);
                ImportRecords.RemoveAt(ImportRecords.Count - 1);

                status = Status.OK;

            }
            catch (Exception ex)
            {
                status = Status.ERROR;
                Console.WriteLine(ex.Message);
            }

            return this;
        }

        public Calculator(DateTime Date, String Agent)
        {
            try
            {
                string result = DataQuery.GetRunsheet(Date, Agent);
                var format = "dd/MM/yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                ImportRecords = JsonConvert.DeserializeObject<List<SmartTransRecord>>(result, dateTimeConverter);

                ImportRecords.RemoveAt(0);
                ImportRecords.RemoveAt(ImportRecords.Count - 1);

                status = Status.OK;

            }
            catch (Exception)
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

                if (record.Description != null)
                {
                    if (record.Description.Length > 0)
                    {
                        r.CompletedCount = ComputeCompletedCount(record.Description.Substring(0, 1));
                        r.AttemptedCount = ComputeAttemptedCount(record.Description.Substring(1, 1));
                    }
                }

                r.Identifier = ComputeIdentifier(record.Status, r.CompletedCount, r.AttemptedCount);

                //r.Identifier = "OTHER";

                // collect initials for driver, look up from driver db
                if (r.Identifier.Contains("Completed"))
                    r.Driver = db.FindDriverCode(record.Driver);

                r.File = record.OrderNumber;

                r.Date = record.DeliveryDate.Value.ToShortDateString();
                //r.Time = record.ETA;
                r.Time = record.Departure;

                //r.Comment1 = RemoveXMLCode(record.Reasons);
                var tempReasons = RemoveXMLCode(record.Reasons);

                r.Comment1 = tempReasons;
                var tempComments = record.DriverComment;
                if (tempComments != null)
                    r = CalculateComments(record, r, tempComments);

                if (r.Identifier == "Completed2")
                {
                    r.Comment8 = "Property Placed Under Seizure";
                }
                else
                {
                    r.Comment8 = "";
                }

                //if (record.Reasons != null)
                //    ComputeReasonandOtherCode(record.Reasons, out reason, out other_Code);

                if (tempReasons != null)
                    ComputeReasonandOtherCode(tempReasons, out reason, out other_Code);

                r.Portal_Reason = reason;
                r.Other_Code = other_Code;

                r.Date2 = record.DeliveryDate.Value.ToShortDateString();
                r.Date3 = record.DeliveryDate.Value.ToShortDateString();

                CollectRecords.Add(r);
            }
        }


        private CollectImportRecord CalculateComments(SmartTransRecord record, CollectImportRecord r, string temp)
        {
            // int stringLength = record.Reasons.Length;
            temp = temp.Replace("\r", string.Empty).Replace("\n", string.Empty);

            int stringLength = temp.Length;
            if (stringLength < 60)
            {
                //r.Comment2 = record.Reasons;
                r.Comment2 = temp;

            }
            else if (stringLength < 120)
            {
                //r.Comment2 = record.Reasons.Substring(0, 60);
                //                                                      //- 60
                //r.Comment3 = record.Reasons.Substring(61, (stringLength - 61)); // this number is the remainder
                r.Comment2 = temp.Substring(0, 60);
                //- 60
                r.Comment3 = temp.Substring(60, (stringLength - 60)); // this number is the remainder
            }
            else if (stringLength < 180)
            {
                //r.Comment2 = record.Reasons.Substring(0, 60);
                //r.Comment3 = record.Reasons.Substring(61, 60);
                //r.Comment4 = record.Reasons.Substring(121, (stringLength - 121)); // this number is the remainder
                r.Comment2 = temp.Substring(0, 60);
                r.Comment3 = temp.Substring(60, 60);
                r.Comment4 = temp.Substring(120, (stringLength - 120)); // this number is the remainder
            }
            else if (stringLength < 240)
            {
                //r.Comment2 = record.Reasons.Substring(0, 60);
                //r.Comment3 = record.Reasons.Substring(61, 60);
                //r.Comment4 = record.Reasons.Substring(121, 60);
                //r.Comment5 = record.Reasons.Substring(181, (stringLength - 181)); // this number is the remainder
                r.Comment2 = temp.Substring(0, 60);
                r.Comment3 = temp.Substring(61, 60);
                r.Comment4 = temp.Substring(121, 60);
                r.Comment5 = temp.Substring(181, (stringLength - 181)); // this number is the remainder

            }
            else if (stringLength < 300)
            {
                //r.Comment2 = record.Reasons.Substring(0, 60);
                //r.Comment3 = record.Reasons.Substring(61, 60);
                //r.Comment4 = record.Reasons.Substring(121, 60);
                //r.Comment5 = record.Reasons.Substring(181, 60);
                //r.Comment6 = record.Reasons.Substring(241, (stringLength - 241)); // this number is the remainder
                r.Comment2 = temp.Substring(0, 60);
                r.Comment3 = temp.Substring(61, 60);
                r.Comment4 = temp.Substring(121, 60);
                r.Comment5 = temp.Substring(181, 60);
                r.Comment6 = temp.Substring(241, (stringLength - 241)); // this number is the remainder
            }
            else
            {
                //r.Comment2 = record.Reasons.Substring(0, 60);
                //r.Comment3 = record.Reasons.Substring(61, 60);
                //r.Comment4 = record.Reasons.Substring(121, 60);
                //r.Comment5 = record.Reasons.Substring(181, 60);
                //r.Comment6 = record.Reasons.Substring(241, 60);
                //r.Comment7 = record.Reasons.Substring(301, (stringLength - 301)); // this number is the remainder
                r.Comment2 = temp.Substring(0, 60);
                r.Comment3 = temp.Substring(61, 60);
                r.Comment4 = temp.Substring(121, 60);
                r.Comment5 = temp.Substring(181, 60);
                r.Comment6 = temp.Substring(241, 60);
                r.Comment7 = temp.Substring(301, (stringLength - 301)); // this number is the remainder
            }

            return r;
        }

        private string RemoveXMLCode(string reasons)
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
                        contents += (child.InnerText + " ");
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


            if (reasons.Trim().Equals("Partnership Person in Charge"))
            {
                reason = "Business Principal";
                other_Code = "1";
            }
            else if (reasons.Trim().Equals("Partners Handing"))
            {
                reason = "Handing Partners";
                other_Code = "2";
            }
            else if (reasons.Trim().Equals("Individual"))
            {
                reason = "Individual";
                other_Code = "3";
            }
            else if (reasons.Trim().Equals("Pty Ltd Lawyer"))
            {
                reason = "Lawyer Corporation";
                other_Code = "4";
            }
            else if (reasons.Trim().Equals("Public Lawyer"))
            {
                reason = "Lawyer Public";
                other_Code = "5";
            }
            else if (reasons.Trim().Equals("Individual Lawyer"))
            {
                reason = "Lawyers Individual";
                other_Code = "6";
            }
            else if (reasons.Trim().Equals("Individual Person 18 - Residence"))
            {
                reason = "Person 18";
                other_Code = "19";
            }
            else if (reasons.Trim().Equals("Individual Person 18 - Business"))
            {
                reason = "Person 18";
                other_Code = "20";
            }
            else if (reasons.Trim().Equals("Pty Ltd- Person in Charge"))
            {
                reason = "Person Corporation";
                other_Code = "9";
            }
            else if (reasons.Trim().Equals("Pre Paid Post"))
            {
                reason = "Pre Paid Post";
                other_Code = "10";
            }
            else if (reasons.Trim().Equals("Pty Ltd - Registered Office"))
            {
                reason = "Registered Office";
                other_Code = "11";
            }
            else if (reasons.Trim().Equals("value 12"))
            {
                reason = "value 12";
            }
            else if (reasons.Trim().Equals("Individual Placed"))
            {
                reason = "OTHER";
                other_Code = "12";
            }
            else if (reasons.Trim().Equals("Individual Prison"))
            {
                reason = "OTHER";
                other_Code = "13";
            }
            else if (reasons.Trim().Equals("Individual Guardian"))
            {
                reason = "OTHER";
                other_Code = "14";
            }
            else if (reasons.Trim().Equals("Individual Guardian Placed"))
            {
                reason = "OTHER";
                other_Code = "15";
            }
            else if (reasons.Trim().Equals("Partners Placed"))
            {
                reason = "OTHER";
                other_Code = "16";
            }
            else if (reasons.Trim().Equals("Public CEO"))
            {
                reason = "OTHER";
                other_Code = "17";
            }
            else if (reasons.Trim().Equals("Pty Ltd - Registered Office - Unattended"))
            {
                reason = "OTHER";
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
            if (status != null)
            {
                if (status.Equals("Completed"))
                {
                    if (completedcount == 0)
                    {
                        return "ExCompletedFA";
                    }
                    else if (completedcount < 8)
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
                    if (completedcount == 0)
                    {
                        return "EXAttemptFA";
                    }
                    else if (completedcount < 8)
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

            return "OTHER";
        }
    }
}
