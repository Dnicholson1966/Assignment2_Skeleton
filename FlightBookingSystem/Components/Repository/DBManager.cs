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
        private static readonly string ReservationFile = "reservations.csv";
        private readonly FlightManager flightManager;


        // using Singleton design pattern for flight manager class
        public static DBManager INSTANCE { get; private set; } = new DBManager();

        private DBManager()
        {
            flightManager = FlightManager.INSTANCE;
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
                using var stream = await FileSystem.OpenAppPackageFileAsync(FlightFile);
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
                throw new NullReferenceException()  ;
            }
        }

        public async Task WriteReservation(Reservation reservedFlight)
        {
                try
                {
                using var stream = await FileSystem.OpenAppPackageFileAsync(ReservationFile);
                using var streamWriter = new StreamWriter(stream); // This line is breaking.
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.WriteRecord(reservedFlight); // Needs testing, haven't used before.
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
        }


        private Airport GetAirport(string code)
        {
            Airport airport = flightManager.GetAirportByCode(code);
            return airport == null ? throw new InvalidAirportInfo("Wrong Airport code provided") : airport;
        }
    }


}