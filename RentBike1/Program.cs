using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DAL.Classes;

namespace RentBike1
{
    public class Program
    {
        static string info = $"--Press 'C' to create new customer\n--Press 'R' to display all customers\n--Press 'U' to update customer by ID\n--Press 'D' to delete customer by ID\n--Press 'Q' to quit\n{new string('-', 45)}";
        static string header = $"ProductDBManager v0.1(alfa)\nPress 'I' to see informaiton\n{new string('-', 45)}\n";
        static Servises Servises = new Servises();
        static void Main(string[] args)
        {
            Console.WriteLine(header);
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.C:
                        Servises.Create();
                        break;
                    case ConsoleKey.R:
                        Servises.Read(header);
                        break;
                    case ConsoleKey.U:
                        //Update();
                        break;
                    case ConsoleKey.D:
                        //Delete();
                        break;
                    case ConsoleKey.I:
                        Console.WriteLine(info);
                        break;
                    case ConsoleKey.Q:
                        Console.Clear();
                        Console.WriteLine("See you next time!\nBYE-BYE!;)");
                        Thread.Sleep(2000);
                        return;
                    default:
                        continue;
                }
            }
            Console.ReadLine();
        }


    }
}

