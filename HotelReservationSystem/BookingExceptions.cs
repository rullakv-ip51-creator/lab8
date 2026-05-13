using System;

namespace HotelReservationSystem
{
    /// <summary>
    /// Власне виключення для обробки помилок під час бронювання номерів.
    /// </summary>
    public class BookingException : Exception
    {
        public BookingException(string message) : base(message) { }
    }
}