using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelReservationSystem
{
    public class Hotel : Identifiable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Основна інформація
        private List<Room> _rooms = new List<Room>();
        private List<Reservation> _reservations = new List<Reservation>();

        public Hotel(int id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public void AddRoom(int id, string number, int capacity, decimal price)
        {
            _rooms.Add(new Room(id, number, capacity, price));
        }

        public IReadOnlyList<Room> GetRooms() => _rooms.AsReadOnly();

        // Підрахунок загальної кількості місць в готелі
        public int GetTotalCapacity() => _rooms.Sum(r => r.Capacity);

        public Reservation BookRoom(int reservationId, Client client, Room room, DateTime start, DateTime end, string notes = "")
        {
            bool isOccupied = _reservations.Any(r => r.BookedRoom.Id == room.Id && !(end <= r.StartDate || start >= r.EndDate));
            if (isOccupied)
                throw new BookingException($"Кімната {room.RoomNumber} вже заброньована на ці дати.");

            var reservation = new Reservation(reservationId, client, room, start, end, notes);
            _reservations.Add(reservation);
            return reservation;
        }

        public IReadOnlyList<Reservation> GetReservations() => _reservations.AsReadOnly();

        // Додано: можливість відмінити замовлення
        public void CancelReservation(int reservationId)
        {
            var res = _reservations.FirstOrDefault(r => r.Id == reservationId);
            if (res != null) _reservations.Remove(res);
            else throw new BookingException("Замовлення не знайдено.");
        }
    }
}