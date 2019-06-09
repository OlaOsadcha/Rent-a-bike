using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Classes;
using DAL.Models;

namespace DAL.Classes
{
    public class Servises
    {
        static string info1 = $"--Press 'C' to create new customer\n--Press 'R' to display all customer\n" +
                             $"--Press 'U' to update customer by ID\n--Press 'D' to delete customer by ID\n" +
                             $"--Press 'Q' to quit\n{new string('-', 45)}\n";

        private Customer customer;//pole customer
        private DBManager dBManager;//pole dbManager
        public DBManager DBManager
        {
            get//mozemy tylko dostac baze(nie korzystamy tu z set)
            {
                if (dBManager == null)

                    dBManager = new DBManager(new RentBikeContext());//dostaniemy tu RentBikeContext, podlaczamy sie
                ///w tym miejscu do bazy danych
                return dBManager;

            }
        }//property dla pola dBManager zrealizowany context bazy danych
        public void Create()
        {
            Console.WriteLine($"Prosze wprowadzic nowego uzytkownika ");
            customer = CreateCustomer();
            if (DBManager.FindOrAddCustomers(customer.PassportNumber))//esli klient jest v base
            {
                Console.WriteLine($"Witamy {customer.FirstName} ponownie w naszym wypozyczalni");
                Console.WriteLine($" ");
                DBManager.CreateOrders(customer.PassportNumber);
                DBManager.Read(info1);
            }
            else
            {
                Console.WriteLine($"Hello {customer.FirstName} create rent ");
                DBManager.CreateOrdersNewCustomer(customer);
                DBManager.Read(info1);
            }//sozdat klienta
        }
        public Customer CreateCustomer()
        {
            Console.WriteLine(new string('-', 45));
            Console.WriteLine("Wwedite Customer FirstNumber");
            string firstname = Console.ReadLine();
            Console.WriteLine("Wwedite Customer LastNumber");
            string lastname = Console.ReadLine();
            Console.WriteLine("Wwedite Customer PhoneNumber");
            string phonenumber = Console.ReadLine();
            Console.WriteLine("Wwedite Customer Email");
            string email = Console.ReadLine();
            Console.WriteLine("Wwedite Customer PasportNumber");
            string paspornumber = Console.ReadLine();
            Customer customer = new Customer()
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                Email = email,
                PassportNumber = paspornumber
            };
            return customer;
        }
        public void Read(string s)
        { DBManager.Read(s); }


    }



}
