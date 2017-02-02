using SmartTrans_Importer.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartTrans_Importer.Core
{
    public class DriverDB
    {
        private List<Driver> Drivers;

        public DriverDB()
        {
            try
            {
                // load from file
                var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SmartTrans\Drivers.xml");

                Drivers = ReadFromXmlFile<List<Driver>>(fileName);
            }
            catch (Exception)
            {
                InitialSeed();
            }

        }

        public void InitialSeed()
        {
            Drivers = new List<Driver>();

            Drivers.Add(new Driver { AgentId = "TK", Name = "T KERR" });
            Drivers.Add(new Driver { AgentId = "JH", Name = "J HILLMAN" });
            Drivers.Add(new Driver { AgentId = "JVW", Name = "J WALSUM" });
            Drivers.Add(new Driver { AgentId = "SI", Name = "S INNES" });
            Drivers.Add(new Driver { AgentId = "SIT", Name = "S TURNER" });
            Drivers.Add(new Driver { AgentId = "JS", Name = "J SEATON" });
            Drivers.Add(new Driver { AgentId = "JT", Name = "J THABANO" });
            Drivers.Add(new Driver { AgentId = "CH", Name = "C HAGART" });
            Drivers.Add(new Driver { AgentId = "MK", Name = "M KOH" });
        }

        internal string FindDriverCode(string driver)
        {
            return Drivers.First(u => u.Name.ToUpper() == driver.ToUpper()).AgentId;
        }

        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SmartTrans\");

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                    //File.Create(@"D:\MyDir\First.txt").Close();

                    // Do stuff here
                    var serializer = new XmlSerializer(typeof(T));
                    writer = new StreamWriter(filePath, append);
                    serializer.Serialize(writer, objectToWrite);
                }
                else
                {
                    var serializer = new XmlSerializer(typeof(T));
                    writer = new StreamWriter(filePath, append);
                    serializer.Serialize(writer, objectToWrite);
                }

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public bool SaveDrivers()
        {
            try
            {

                // Get filename from Appdata
                var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SmartTrans\Drivers.xml");

                // Write the list of objects to the file.
                WriteToXmlFile<List<Driver>>(fileName, Drivers);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Add(String name, String AgentID)
        {
            Drivers.Add(new Driver { AgentId = AgentID, Name = name });
        }

        public bool Remove(String AgentID)
        {
            var driverToRemove = Drivers.Single(d => d.AgentId == AgentID);

            if (Drivers.Remove(driverToRemove))
            {
                return true;
            }

            return false;
        }

        public List<Driver> GetDrivers()
        {
            return Drivers.ToList();
        }

        public string GetCollectInitials(String DriverName)
        {
            var itemToGet = Drivers.Single(u => u.Name == DriverName);

            if (itemToGet != null)
            {
                return itemToGet.AgentId;
            }
            else
            {
                return "";
            }
        }
    }

}
