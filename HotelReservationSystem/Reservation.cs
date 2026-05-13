using System;

namespace HotelReservationSystem
{
    /// <summary>
    /// Клас Бронювання
    /// </summary>
    public class Reservation : Identifiable
    {
        public int Id { get; set; }

        public Client BookedBy { get; set; }

        public Room BookedRoom { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Конструктор бронювання.
        /// </summary>
        public Reservation(int id, Client client, Room room, DateTime start, DateTime end, string notes = "")
        {
            if (end <= start)
                throw new ArgumentException("Дата виїзду повинна бути пізніше дати заїзду.");

            Id = id;
            BookedBy = client;
            BookedRoom = room;
            StartDate = start;
            EndDate = end;
            Notes = notes;
        }

        /// <summary>
        /// Обчислення загальної вартості
        /// </summary>
        public decimal CalculateTotalCost()
        {
            int days = (EndDate - StartDate).Days;
            return days * BookedRoom.PricePerNight;
        }
    }
}