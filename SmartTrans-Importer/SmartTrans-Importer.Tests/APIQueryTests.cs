using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartTrans_Importer.Core;
using Newtonsoft.Json;
using SmartTrans_Importer.Core.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace SmartTrans_Importer.Tests
{
    [TestClass]
    public class APIQueryTests
    {
        //[TestMethod]
        //public void TestGetDataFromStaging()
        //{

        //    var Date = new DateTime(2016, 04, 06);
        //    var Agent = "JVW";

        //    string result = DataQuery.GetRunsheet(Date, Agent);


        //    Console.WriteLine(result);

        //}

        [TestMethod]
        public void TestConvertJsonToObject()
        {
            var Date = new DateTime(2016, 09, 27);
            var Agent = "JVW";
            
            string result = DataQuery.GetRunsheet(Date, Agent);

            var format = "dd/MM/yyyy";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            var answer = JsonConvert.DeserializeObject<List<SmartTransRecord>>(result, dateTimeConverter);


            Console.WriteLine();
        }
    }
}
