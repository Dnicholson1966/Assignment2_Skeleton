using FlightBookingSystem.Components.Exceptions;
using FlightBookingSystem.Components.Model;
using FlightBookingSystem.Components.ViewModel;
using FlightBookingSystem.Components.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using CsvHelper; // Used mainly for writing to a csv file.
using System.Globalization; // Used mainly for writing to a csv file.


namespace FlightBookingSystem.Components.Repository
{
    internal class DBManager
    {
        private static readonly string AiportFile = "airports.csv";
        private static readonly string FlightFile = "flights.csv";

        // Note for below: We had to create a new, separate reservations file, since the old one was a read-only embedded resource.
        private static readonly string ReservationFile = "../Reservations.csv";
        private static string workingDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;





        private readonly FlightManager flightManager;


        // using Singleton design pattern for flight manager class
        public static DBManager INSTANCE { get; private set; } = new DBManager();

        private DBManager()
        {
            flightManager = FlightManager.INSTANCE;
            // Had to create a separate file for reservations. I cannot read or write to it as it is an embedded resource.

        }

        public async Task InitializeAsync()
        {

            loadAirports();
            loadFlights();

        }

        public void RefreshFlights()
        {
            loadFlights();
        }

        public void RefreshAirports()
        {
            loadAirports();
        }

        private async Task loadAirports()
        {

            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(AiportFile);
                using var reader = new StreamReader(stream);

                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] parts = line.Split(",");
                    flightManager.AddAirport(parts[0], parts[1]);
                    line = reader.ReadLine();
                }
            }
            catch (FileNotFoundException)
            {
                // Add handling
                throw new FileNotFoundException();
            }
            catch (NullReferenceException)
            {
                // Add handling
                throw new NullReferenceException();
            }

        }


        private async Task loadFlights()
        {
            try
            {
                Console.WriteLine(FileSystem.AppDataDirectory); // Testing. Need more info on this "AppPackage" stuff!
                using var stream = await FileSystem.OpenAppPackageFileAsync(FlightFile);
                Console.WriteLine(FileSystem.Current.AppDataDirectory); // Testing.
                using var reader = new StreamReader(stream);

                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] parts = line.Split(',');
                    flightManager.AddFlight(parts[0],
                        parts[1],
                        GetAirport(parts[2]),
                        GetAirport(parts[3]),
                        parts[4],
                        parts[5],
                        int.Parse(parts[6]),
                        decimal.Parse(parts[7]));

                    line = reader.ReadLine();

                }
            }
            catch (FileNotFoundException)
            {
                // Add handling
                throw new FileNotFoundException();
            }
            catch (NullReferenceException)
            {
                // Add handling
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Writes a given reservation to "reservations.csv" file.
        /// Note we had to create a new, separate reservations file, since the old one was a read-only embedded resource.
        /// </summary>
        /// <param name="reservation">Reservation user has booked.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public async Task WriteReservation(Reservation reservation)
        {
            try
            {
                FileStream fs = File.Create(ReservationFile); // Create the file, or overwrite if the file exists. Not actually fully intended, just testing...
                var records = new List<Reservation>() // Cheap hack to work with csvWriter. Single records: "WriteRecord(records)" doesn't seem to work.
                {
                    reservation
                };

                // Dont' write header again, to allow appending. Doesn't work...
                //var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                //config.HasHeaderRecord = false;

                using (var stream = File.Open(ReservationFile, FileMode.Append))
                {
                    StreamWriter newWriter = new StreamWriter(stream);
                    using var csvAppend = new CsvWriter(newWriter, CultureInfo.InvariantCulture);
                    {
                        csvAppend.WriteRecords(records);
                    }
                }
            }

            catch (FileNotFoundException)
            {
                // Add MORE handling
                throw new FileNotFoundException();
            }
            catch (NullReferenceException)
            {
                // Add MORE handling
                throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        private Airport GetAirport(string code)
        {
            Airport airport = flightManager.GetAirportByCode(code);
            return airport == null ? throw new InvalidAirportInfo("Wrong Airport code provided") : airport;
        }
    }


}