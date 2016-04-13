using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartTrans_Importer.Core;

namespace SmartTrans_Importer.Tests
{
    [TestClass]
    public class APIQueryTests
    {
        [TestMethod]
        public void TestGetDataFromStaging()
        {

            var Date = new DateTime(2016, 04, 06);
            var Agent = "JVW";

            string result = DataQuery.GetRunsheet(Date, Agent);


            Console.WriteLine(result);

        }
    }
}
