using CsvHelper;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.ClassExamples;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace TestProject
{
    /// <summary>
    /// Just for testing things in isolation.
    /// </summary>
    internal class TestProgram
    {
        public static void Main(String[] args)
        {
            // Testing relative filepath.
            string filePath = "../TestFile.csv";

            // ExampleObject singleRecord = new ExampleObject("Derrick Nicholson", 32); // Single Record doesn't work.

            var records = new List<ExampleObject>()
            {
                new ExampleObject("Derrick Nicholson", 32),
                new ExampleObject("David Nicholson", 52)
            };

            // Create file, test Appending to the new file.
            // var writer = new StreamWriter(File.Create(filePath));
            var writer = new StreamWriter(filePath);
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture, false))
            {
                csvWriter.WriteRecords(records);
            };
            writer.Close();

            // Append additonal data.

            var newRecords = new List<ExampleObject>()
            {
                new ExampleObject("Casey Marie", 36)
            };

            // Dont' write header again, to allow appending. Doesn't work...
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.HasHeaderRecord = false;


            using (var stream = File.Open(filePath, FileMode.Append))
            {
                var newWriter = new StreamWriter(stream);
                using var csvAppend = new CsvWriter(new StreamWriter(stream), config);
                { 
                    csvAppend.WriteRecords(newRecords);
                }
            }
        }
    }
}
