﻿using FlightBookingSystem.Components.Model;
using FlightBookingSystem.Components.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem.Components.ViewModel
{
    internal class FlightManager
    {
        // using Singleton design pattern for flight manager class
        public static FlightManager INSTANCE = new FlightManager();

        private List<Airport> AirportList = new List<Airport>();

        private List<Flight> FlightList = new List<Flight>();

        private FlightManager()
        {

        }


        public void AddAirport(string code, string name)
        {
            Airport newAirport = new Airport(code, name);

            if (AirportList.Contains(newAirport)) return;

            AirportList.Add(newAirport);

        }

        public void AddFlight(string flight_id, string airline, Airport source, Airport destination, string day, string time, int totalseats, decimal cost)
        {
            Flight f = new Flight(flight_id, day, source, destination, airline, time, totalseats, cost);
            if (FlightList.Contains(f)) return;
            FlightList.Add(f);
        }

        public Airport GetAirportByCode(string code)
        {
            foreach (Airport airport in AirportList)
            {
                if (airport.Code.Equals(code, StringComparison.OrdinalIgnoreCase)) return airport;
            }
            return null;
        }

        public static List<string> GetAiportCodeList()
        {
            List<string> airportName = new List<string>();
            foreach (Airport airport in INSTANCE.AirportList)
            {
                airportName.Add(airport.Name);
            }
            return airportName;
        }



        public List<Flight> FindFlights(string src, string dest, string day)
        {
            if (FlightList.Count == 0)
            {
                DBManager.INSTANCE.RefreshFlights();
            }
            List<Flight> list = new List<Flight>();
            foreach (var flight in FlightList)
            {
                if (flight.Source.Name.Equals(src) &&
                    flight.Destination.Name.Equals(dest) &&
                    (flight.Day.Equals(day) || day.Equals("Any")))
                {

                    list.Add(flight);
                }

            }
            return list;
        }

        /// <summary>
        /// Returns a specific flight object from a List of flights, filtered based on ID.
        /// If not found, throws an exception!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectedFlights"></param>
        /// <returns>Flight: One specific flight.</returns>
        public Flight getFlightById(string id, List<Flight> selectedFlights)
        {
            foreach (Flight f in selectedFlights)
            {
                if (f.FlightId.Equals(id)) { return f; }
            }
            throw new Exception("Flight was not found by \"getFlightbyID\". Check your code!");
        }
    }
}
