namespace HotelReservationSystem
{
    /// <summary>
    /// Клас Кімната.
    /// </summary>
    public class Room : Identifiable
    {
        /// <summary>
        /// Ідентифікатор кімнати (реалізація інтерфейсу).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Номер кімнати.
        /// </summary>
        public string RoomNumber { get; set; }

        /// <summary>
        /// Кількість місць.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Ціна за ніч.
        /// </summary>
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Конструктор кімнати.
        /// </summary>
        public Room(int id, string roomNumber, int capacity, decimal pricePerNight)
        {
            Id = id;
            RoomNumber = roomNumber;
            Capacity = capacity;
            PricePerNight = pricePerNight;
        }
    }
}