#nullable disable
namespace HotelReservationSystem
{
    public abstract class Person : Identifiable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual string GetFullName() => $"{FirstName} {LastName}";
    }
}