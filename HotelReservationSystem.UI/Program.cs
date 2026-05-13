#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservationSystem;

namespace HotelReservationSystem.UI
{
    /// <summary>
    /// Головний клас програми, що реалізує користувацький консольний інтерфейс.
    /// </summary>
    class Program
    {
        static List<Hotel> hotels = new List<Hotel>();
        static List<Client> clients = new List<Client>();

        static int hotelIdCounter = 1;
        static int clientIdCounter = 1;
        static int resIdCounter = 1;
        static int roomIdCounter = 100;

        /// <summary>
        /// Головна точка входу в програму. Запускає головне меню додатку.
        /// </summary>
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            SeedData();

            while (true)
            {
                Console.Clear();
                PrintHeader("ГОЛОВНЕ МЕНЮ");

                Console.WriteLine("1. Управління готелями");
                Console.WriteLine("2. Управління клієнтами");
                Console.WriteLine("3. Управління замовленнями номерів");
                Console.WriteLine("4. Пошук");
                Console.WriteLine("0. Вихід");
                Console.Write("\nВаш вибір: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1": ManageHotels(); break;
                        case "2": ManageClients(); break;
                        case "3": ManageOrders(); break;
                        case "4": SearchMenu(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ПОМИЛКА]: {ex.Message}");
                    Console.WriteLine("Натисніть будь-яку клавішу...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Допоміжний метод для форматованого виведення заголовків меню по центру.
        /// </summary>
        /// <param name="text">Текст заголовка</param>
        static void PrintHeader(string text)
        {
            string spacedText = string.Join(" ", text.ToCharArray());
            int width = Console.WindowWidth;
            int padding = (width - spacedText.Length) / 2;
            if (padding < 0) padding = 0;
            Console.WriteLine("\n\n" + new string(' ', padding) + spacedText + "\n");
        }

        /// <summary>
        /// 1. Меню управління готелями: додавання, видалення, перегляд та робота з кімнатами.
        /// </summary>
        static void ManageHotels()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("УПРАВЛІННЯ ГОТЕЛЯМИ");
                Console.WriteLine("1. Додати готель");
                Console.WriteLine("2. Видалити готель");
                Console.WriteLine("3. Перегляд конкретного готелю");
                Console.WriteLine("4. Перегляд всіх готелів (опис і місця)");
                Console.WriteLine("5. Додавання заявки на номер (на термін)");
                Console.WriteLine("6. Видалення заявки на номер");
                Console.WriteLine("7. Замінити текст заявки");
                Console.WriteLine("8. Перегляд заявок за термін");
                Console.WriteLine("9. Додати кімнати до готелю");
                Console.WriteLine("0. Назад");
                Console.Write("\nВибір: ");
                string ch = Console.ReadLine();
                if (ch == "0") return;
                Console.Clear();

                if (ch == "1")
                {
                    Console.Write("Назва: "); string n = Console.ReadLine();
                    Console.Write("Опис: "); string d = Console.ReadLine();
                    hotels.Add(new Hotel(hotelIdCounter++, n, d));
                    Console.WriteLine("Готель додано.");
                }
                else if (ch == "2")
                {
                    ShowHotelsBrief();
                    Console.Write("ID для видалення: "); int id = int.Parse(Console.ReadLine());
                    hotels.RemoveAll(h => h.Id == id);
                }
                else if (ch == "3")
                {
                    ShowHotelsBrief();
                    Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                    var h = hotels.Find(x => x.Id == id);
                    if (h != null) Console.WriteLine($"[{h.Id}] {h.Name}\nОпис: {h.Description}\nМісць: {h.GetTotalCapacity()}");
                }
                else if (ch == "4")
                {
                    foreach (var h in hotels) Console.WriteLine($"[{h.Id}] {h.Name} | {h.Description} | Місць: {h.GetTotalCapacity()}");
                }
                else if (ch == "5") AddReservationFlow();
                else if (ch == "6") CancelReservationFlow();
                else if (ch == "7") EditReservationFlow();
                else if (ch == "8") ViewReservationsByPeriod();
                else if (ch == "9") AddRoomToHotelFlow();

                Console.WriteLine("\nНатисніть клавішу..."); Console.ReadKey();
            }
        }

        /// <summary>
        /// Логіка ручного додавання кімнат
        /// </summary>
        static void AddRoomToHotelFlow()
        {
            PrintHeader("ДОДАВАННЯ КІМНАТИ");
            ShowHotelsBrief();
            Console.Write("Виберіть ID готелю: ");
            int hId = int.Parse(Console.ReadLine());
            var hotel = hotels.Find(x => x.Id == hId);

            if (hotel != null)
            {
                Console.Write("Номер кімнати (напр. 101A): "); string num = Console.ReadLine();
                Console.Write("Кількість місць: "); int cap = int.Parse(Console.ReadLine());
                Console.Write("Ціна за добу (грн): "); decimal price = decimal.Parse(Console.ReadLine());

                hotel.AddRoom(roomIdCounter++, num, cap, price);
                Console.WriteLine($"\nКімнату {num} успішно додано до готелю {hotel.Name}!");
            }
            else Console.WriteLine("Готель не знайдено.");
        }

        /// <summary>
        /// 2. Меню управління клієнтами: додавання, редагування, видалення та сортування.
        /// </summary>
        static void ManageClients()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("УПРАВЛІННЯ КЛІЄНТАМИ");
                Console.WriteLine("1. Додавання клієнтів");
                Console.WriteLine("2. Видалення клієнтів");
                Console.WriteLine("3. Змінення даних про клієнтів");
                Console.WriteLine("4. Перегляд даних конкретного клієнта");
                Console.WriteLine("5. Перегляд всіх клієнтів");
                Console.WriteLine("6. Сортувати за ім'ям");
                Console.WriteLine("7. Сортувати за прізвищем");
                Console.WriteLine("0. Назад");
                Console.Write("\nВибір: ");
                string ch = Console.ReadLine();
                if (ch == "0") return;
                Console.Clear();

                if (ch == "1")
                {
                    Console.Write("Ім'я: "); string f = Console.ReadLine();
                    Console.Write("Прізвище: "); string l = Console.ReadLine();
                    Console.Write("Тел: "); string p = Console.ReadLine();
                    clients.Add(new Client(clientIdCounter++, f, l, p));
                }
                else if (ch == "2")
                {
                    ShowClientsBrief();
                    Console.Write("ID для видалення: "); int id = int.Parse(Console.ReadLine());
                    clients.RemoveAll(c => c.Id == id);
                }
                else if (ch == "3")
                {
                    ShowClientsBrief();
                    Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                    var c = clients.Find(x => x.Id == id);
                    if (c != null) { Console.Write("Нове прізвище: "); c.LastName = Console.ReadLine(); }
                }
                else if (ch == "4")
                {
                    ShowClientsBrief();
                    Console.Write("ID: "); int id = int.Parse(Console.ReadLine());
                    var c = clients.Find(x => x.Id == id);
                    if (c != null) Console.WriteLine($"{c.GetFullName()} | {c.PhoneNumber}");
                }
                else if (ch == "5")
                {
                    clients.ForEach(c => Console.WriteLine($"ID: {c.Id} | {c.GetFullName()}"));
                }
                else if (ch == "6")
                {
                    foreach (var c in clients.OrderBy(x => x.FirstName)) Console.WriteLine(c.GetFullName());
                }
                else if (ch == "7")
                {
                    foreach (var c in clients.OrderBy(x => x.LastName)) Console.WriteLine(c.GetFullName());
                }
                Console.WriteLine("\nНатисніть клавішу..."); Console.ReadKey();
            }
        }

        /// <summary>
        /// 3. Меню управління бронюваннямм: перегляд, скасування, статистика.
        /// </summary>
        static void ManageOrders()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("УПРАВЛІННЯ ЗАМОВЛЕННЯМИ");
                Console.WriteLine("1. Замовити номер");
                Console.WriteLine("2. Відмінити замовлення");
                Console.WriteLine("3. Переглянути конкретне замовлення");
                Console.WriteLine("4. Дані про заброньовані місця");
                Console.WriteLine("5. Дані про вільні місця");
                Console.WriteLine("6. Вартість послуг");
                Console.WriteLine("7. Клієнти, які забронювали номери");
                Console.WriteLine("0. Назад");
                Console.Write("\nВибір: ");
                string ch = Console.ReadLine();
                if (ch == "0") return;
                Console.Clear();

                if (ch == "1") AddReservationFlow();
                else if (ch == "2") CancelReservationFlow();
                else if (ch == "3")
                {
                    ShowHotelsBrief();
                    Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
                    var h = hotels.Find(x => x.Id == hId);
                    foreach (var r in h.GetReservations()) Console.WriteLine($"ID: {r.Id} | {r.BookedBy.LastName} | №{r.BookedRoom.RoomNumber}");
                    Console.Write("ID замовлення: "); int rId = int.Parse(Console.ReadLine());
                    var res = h.GetReservations().FirstOrDefault(x => x.Id == rId);
                    if (res != null) Console.WriteLine($"Замовлення №{res.Id}: Клієнт {res.BookedBy.GetFullName()}, Кімната {res.BookedRoom.RoomNumber}, Період {res.StartDate:d} - {res.EndDate:d}");
                }
                else if (ch == "4")
                {
                    ShowHotelsBrief();
                    Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
                    var h = hotels.Find(x => x.Id == hId);
                    var booked = h.GetReservations().Select(r => r.BookedRoom).Distinct().ToList();
                    Console.WriteLine($"Заброньовано місць: {booked.Count}\nНомери: {string.Join(", ", booked.Select(x => x.RoomNumber))}");
                }
                else if (ch == "5")
                {
                    ShowHotelsBrief();
                    Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
                    var h = hotels.Find(x => x.Id == hId);
                    var free = h.GetRooms().Where(r => !h.GetReservations().Any(res => res.BookedRoom.Id == r.Id)).ToList();
                    Console.WriteLine($"Вільних місць: {free.Count}\nНомери: {string.Join(", ", free.Select(x => x.RoomNumber))}");
                }
                else if (ch == "6")
                {
                    ShowHotelsBrief();
                    Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
                    var h = hotels.Find(x => x.Id == hId);
                    foreach (var r in h.GetReservations())
                    {
                        int days = (r.EndDate - r.StartDate).Days;
                        Console.WriteLine($"Бронь №{r.Id}: {r.BookedRoom.PricePerNight}грн/день * {days} днів = {r.CalculateTotalCost()} грн");
                    }
                }
                else if (ch == "7")
                {
                    ShowHotelsBrief();
                    Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
                    var h = hotels.Find(x => x.Id == hId);
                    var bClients = h.GetReservations().Select(r => r.BookedBy).Distinct();
                    foreach (var c in bClients) Console.WriteLine($"- {c.GetFullName()} (Тел: {c.PhoneNumber})");
                }
                Console.WriteLine("\nНатисніть клавішу..."); Console.ReadKey();
            }
        }

        /// <summary>
        /// 4. Меню для пошуку готелів та клієнтів за введеним ключовим словом.
        /// </summary>
        static void SearchMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("ПОШУК");
                Console.WriteLine("1. Пошук готелів");
                Console.WriteLine("2. Пошук клієнтів");
                Console.WriteLine("0. Назад");
                Console.Write("\nВибір: ");
                string ch = Console.ReadLine();
                if (ch == "0") return;
                Console.Clear();

                if (ch == "1")
                {
                    Console.Write("Ключове слово для готелю: "); string k = Console.ReadLine().ToLower();
                    var res = hotels.Where(h => h.Name.ToLower().Contains(k) || h.Description.ToLower().Contains(k));
                    foreach (var h in res) Console.WriteLine($"[{h.Id}] {h.Name}");
                }
                else if (ch == "2")
                {
                    Console.Write("Ключове слово для клієнта: "); string k = Console.ReadLine().ToLower();
                    var res = clients.Where(c => c.FirstName.ToLower().Contains(k) || c.LastName.ToLower().Contains(k));
                    foreach (var c in res) Console.WriteLine($"ID: {c.Id} | {c.GetFullName()}");
                }
                Console.WriteLine("\nНатисніть клавішу..."); Console.ReadKey();
            }
        }

        static void AddReservationFlow()
        {
            ShowClientsBrief(); Console.Write("ID клієнта: "); int cId = int.Parse(Console.ReadLine());
            ShowHotelsBrief(); Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
            var h = hotels.Find(x => x.Id == hId);
            if (!h.GetRooms().Any()) { Console.WriteLine("У готелі немає кімнат! Спочатку додайте кімнату (Меню 1, пункт 9)."); return; }
            foreach (var r in h.GetRooms()) Console.WriteLine($"ID: {r.Id} | №{r.RoomNumber} | Ціна: {r.PricePerNight}");
            Console.Write("ID кімнати: "); int rId = int.Parse(Console.ReadLine());
            Console.Write("Заїзд (рррр-мм-дд): "); DateTime d1 = DateTime.Parse(Console.ReadLine());
            Console.Write("Виїзд (рррр-мм-дд): "); DateTime d2 = DateTime.Parse(Console.ReadLine());
            Console.Write("Нотатки: "); string n = Console.ReadLine();
            h.BookRoom(resIdCounter++, clients.Find(c => c.Id == cId), h.GetRooms().First(r => r.Id == rId), d1, d2, n);
            Console.WriteLine("Готово.");
        }

        static void CancelReservationFlow()
        {
            ShowHotelsBrief(); Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
            var h = hotels.Find(x => x.Id == hId);
            foreach (var r in h.GetReservations()) Console.WriteLine($"ID: {r.Id} | {r.BookedBy.LastName} | №{r.BookedRoom.RoomNumber}");
            Console.Write("ID броні: "); int resId = int.Parse(Console.ReadLine());
            h.CancelReservation(resId);
        }

        static void EditReservationFlow()
        {
            ShowHotelsBrief(); Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
            var h = hotels.Find(x => x.Id == hId);
            foreach (var r in h.GetReservations()) Console.WriteLine($"ID: {r.Id} | Нотатки: {r.Notes}");
            Console.Write("ID броні: "); int resId = int.Parse(Console.ReadLine());
            Console.Write("Новий текст: "); h.GetReservations().First(x => x.Id == resId).Notes = Console.ReadLine();
        }

        /// <summary>
        /// Пошук та виведення списку замовлень, які перетинаються із заданим періодом дат.
        /// </summary>
        static void ViewReservationsByPeriod()
        {
            ShowHotelsBrief(); Console.Write("ID готелю: "); int hId = int.Parse(Console.ReadLine());
            Console.Write("Від (рррр-мм-дд): "); DateTime d1 = DateTime.Parse(Console.ReadLine());
            Console.Write("До (рррр-мм-дд): "); DateTime d2 = DateTime.Parse(Console.ReadLine());
            var list = hotels.Find(x => x.Id == hId).GetReservations().Where(r => r.StartDate >= d1 && r.EndDate <= d2);
            foreach (var r in list) Console.WriteLine($"№{r.Id} | {r.BookedBy.LastName} | {r.StartDate:d}-{r.EndDate:d}");
        }

        /// <summary>
        /// Виводить короткий список доступних готелів для вибору по ID.
        /// </summary>
        static void ShowHotelsBrief() => hotels.ForEach(h => Console.WriteLine($"ID: {h.Id} | {h.Name}"));

        /// <summary>
        /// Виводить короткий список зареєстрованих клієнтів для вибору по ID.
        /// </summary>
        static void ShowClientsBrief() => clients.ForEach(c => Console.WriteLine($"ID: {c.Id} | {c.GetFullName()}"));

        /// <summary>
        /// Ініціалізація системи початковими тестовими даними (готелі, кімнати, клієнти).
        /// </summary>
        static void SeedData()
        {
            var h = new Hotel(hotelIdCounter++, "Hilton", "5 зірок, люкс");
            h.AddRoom(roomIdCounter++, "101", 2, 2000m);
            h.AddRoom(roomIdCounter++, "102", 4, 4500m);
            hotels.Add(h);
            clients.Add(new Client(clientIdCounter++, "Олексій", "Шевченко", "0931112233"));
        }
    }
}