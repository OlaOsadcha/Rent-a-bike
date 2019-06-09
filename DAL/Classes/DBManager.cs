using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Threading;

namespace DAL.Classes
{
    public class DBManager : IDisposable //interfejs, ktory odpowiada za usunięcie zasobów
    {
        RentBikeContext context = null;//zmienna typu RentBikeContext
        public DBManager(RentBikeContext rentBikeContext)//kontruktor
        {
            this.context = rentBikeContext;
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return context.Customer.ToList();

        }//dostaniemy wszystkich customer
        public bool FindOrAddCustomers(string pasportnumber)
        {

            if (context.Customer.AsNoTracking().Any(n => n.PassportNumber == pasportnumber))
            {
                return true;
            }
            else return false;
            //AsNoTracking - tylko czytamy z bazy(NIC NIE ZMIENIAMY TYM)

        }//znalezc albo stworzyc customer
        public Manager GetManager()
        {
            int dateday = DateTime.Now.Day;
            if (dateday % 2 == 0)
            {
                var manager = context.Manager.Single(m => m.Id == 2);
                return manager;
            }
            else
            {
                var manager = context.Manager.Single(m => m.Id == 1);
                return manager;
            }
        }//tworzymy managera(jesli parzysta data, to manager z Id == 2, jesli nieparzyste, to manager z Id==1)
        public void SaveIsActivate(int id)
        {
            var item = context.Bike.Find(id);
            item.IsActive = false;
            context.SaveChanges();
        }//zapisujemy wybrany bike pole isActivate(jesli wypozyczylismy bike, to piszemy, ze jest nieaktywny(false))
        public BikeItems GetBikeItems(Bike bike, OrderBike orderBike)
        {

            DateTime startrent = DateTime.Now;
            Console.WriteLine($" StartRent  Data {startrent}");
            Console.WriteLine("Wprowadz ilosc czasu wypozyczenia(Hours) ");
            int hours = Convert.ToInt32(Console.ReadLine());
            DateTime finishRent = startrent.AddHours(hours);
            Decimal pricerent = hours * bike.Price.Value;
            Console.WriteLine($" cena =============  {pricerent}");

            BikeItems bikeItems = new BikeItems//tworzymy obiekt BikeItems(zamowienie)
            {

                Bike = bike,
                StartRent = startrent,
                FinishRent = finishRent,
                OrderBike = orderBike,
                Pledqe = 200,
                PriceRent = pricerent
            };
            return bikeItems;
        }

        public void CreateOrders(string pasportnumber)//zamowienie dla customer, który jest w bazie
        {
            var manager = GetManager();
            var customer = context.Customer.SingleOrDefault(c => c.PassportNumber == pasportnumber);
            var orderbike = context.OrderBike.Add(new OrderBike { Customer = customer, Manager = manager, Date = DateTime.Now });
            Console.WriteLine("---Types Bykes---");
            var b = GetTypeBike(orderbike);
            if (b == null)
            {
                Console.WriteLine("Zamowienie zostalo odrzucone");
                Thread.Sleep(2000);

            }
            else
            {
                Console.WriteLine(" Zamowienie zostalo przyjete");
                Thread.Sleep(2000);
                context.BikeItems.AddRange(b);//dodajumy Liste z zamowieniami(1 albo wiecej)
                context.SaveChanges();
            }

        }
        public void CreateOrdersNewCustomer(Customer _customer)//gdy uzytkownika nie ma w bazie
        {
            var manager = GetManager();
            var customer = _customer;
            var orderbike = context.OrderBike.Add(new OrderBike { Customer = customer, Manager = manager, Date = DateTime.Now });
            Console.WriteLine("---Types Bykes---");
            var b = GetTypeBike(orderbike);
            if (b == null)
            {
                Console.WriteLine(" Zamowienie zostalo odrzucone");
                Thread.Sleep(2000);


            }
            else
            {
                Console.WriteLine(" Zamowienie zostalo zrealizowane");
                Thread.Sleep(2000);
                context.BikeItems.AddRange(b);
                context.SaveChanges();
            }

        }
        public List<BikeItems> GetTypeBike(OrderBike ordersBike)
        {
            List<BikeItems> list = new List<BikeItems>();
            int flag = 1;
            do
            {
                var types = context.Type.ToList();
                foreach (var t in types)
                {
                    Console.WriteLine("TypeBike --{0}", t.Name);
                }
                Console.WriteLine("\n ");
                Console.WriteLine("Wybierz type bike");
                var typename = Console.ReadLine();//
                var freebykes = context.Bike.AsNoTracking().Where(b => b.Type.Name == typename)//sprawdzamy, czy mamy Bike w bazie
                                                           .Where(b => b.IsActive == true).ToList();
                if (freebykes.Count == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"Biks type {typename} net v nalichii!  Vvedite 1 vybrat  drugoj type.");
                    Console.WriteLine($"Ili  vvedite 0 esli ne podhodit");
                    flag = Convert.ToInt32(Console.ReadLine());
                    if (flag == 0)
                        return list = null;
                }
                else
                {

                    foreach (var b in freebykes)
                    {
                        Console.WriteLine("Free Bike type  -- {0} number -- {1} ", b.Type.Name, b.BikeNumber);//
                    }
                    Console.WriteLine($"Podtverdite zakaz nabrav number ili 0 vyjti ");
                    var number = Console.ReadLine();
                    if (number == "0")
                    {
                        return list = null;
                    }
                    else
                    {
                        var type = context.Type.SingleOrDefault(t => t.Name == typename);
                        Bike bike = context.Bike.Where(b => b.Type.Name == type.Name)
                                                 .Where(b => b.IsActive == true).Where(b => b.BikeNumber == number).Single();

                        var bikeItems = GetBikeItems(bike, ordersBike);
                        list.Add(bikeItems);
                        SaveIsActivate(bike.Id);
                    }


                }
                Console.WriteLine(new string('-', 45));
                Console.WriteLine("Vvedite 1 esli vybrat esce odin ili 0 vyjti ");
                flag = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(new string('-', 45));
            } while (flag == 1);

            return list;
        }
        public void Read(string info)
        {
            Console.Clear();
            Console.Write(info);
            var customers = GetAllCustomers();
            foreach (Customer customer in customers)
               Console.WriteLine($"{customer.Id})FirstName: {customer.FirstName} -- LastName: {customer.LastName}");

        }

        public void Dispose()
        {
            context.Dispose();
        }




    }
}
