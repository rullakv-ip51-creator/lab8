namespace HotelReservationSystem
{
    /// <summary>
    /// Клас Клієнт
    /// </summary>
    public class Client : Person
    {
        public string PhoneNumber { get; set; }

        public Client(int id, string firstName, string lastName, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public override string GetFullName() => $"Клієнт: {base.GetFullName()}";
    }
}