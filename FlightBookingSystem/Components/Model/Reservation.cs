using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBookingSystem.Components.Model
{
    internal class Reservation
    {
        internal readonly string _flightCode;
        internal readonly string _airline;
        internal readonly string _day;
        internal readonly string _time;
        internal readonly decimal _cost;
        internal readonly string _reservationCode; // Primary Key, generated randomly.

        internal string _passengerName;
        internal string _passengerCitizenship;
        


        public string FlightCode { get { return _flightCode; } init { _flightCode = value; } }
        public string Airline { get { return _airline; } init { _airline = value; } }
        public string Day { get { return _day; } init { _day = value; } }
        public string Time { get { return _time; } init { _time = value; } }
        public decimal Cost { get { return _cost; } init { _cost = value; } }
        public string PassengerName { get { return _passengerName; } set { _passengerName = value; } }
        public string CitizenShip { get { return _passengerCitizenship; } set { _passengerCitizenship = value; } }

        public string ReservationCode { get { return _reservationCode; } init { _reservationCode = value; } }

        /// <summary>
        /// Default Constructor for reservation object.
        /// </summary>
        /// <param name="flight">Flight assocatied with this reservation.</param>
        /// <param name="passenger">Passenger's name.</param>
        /// <param name="citizenship">Passenger's citizenship status.</param>
        public Reservation (Flight flight, string passenger, string citizenship)
        {
            _passengerName = passenger;
            _passengerCitizenship = citizenship;

            _flightCode = flight.FlightId;
            _airline = flight.Airline;
            _day = flight.Day;
            _time = flight.Time;
            _cost = flight.Cost;

            // Code to generate Reservation Code. Will do later.
            /* Random rnd = new Random();
            int num = rnd.Next(1000); */
        }

        public override string ToString()
        {
            return $"{FlightCode} : {Airline} : {Day} : {Time} : {Cost} : {PassengerName} : {CitizenShip}";
            // Still needs the Reservation code. Will do later.
        }
    }
}
