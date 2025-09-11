using System;
using System.Globalization;
namespace BaiTap2
{
    class Person
    {
        private string name;
        private string address;
        private double salary;
        public Person(string name, string address, double salary)
        {
            this.name = name;
            this.address = address;
            this.salary = salary;
        }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public double Salary { get => salary; set => salary = value; }
    }
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=====Management Person programer=====");

            Person[] people = new Person[3];
            for (int i = 0; i < people.Length; i++)
            {
                Console.WriteLine("Input Information of Person {0}", i + 1);
                people[i] = InputPersonInfo();
            }

            SortBySalary(people);

            Console.WriteLine("\nInformation of Person you have entered (sorted):");
            foreach (var p in people)
            {
                DisplayPersonInfo(p);
            }
        }
        static Person InputPersonInfo()
        {
            string name;
            while (true)
            {
                Console.Write("Please input name: ");
                name = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(name))
                    break;
                Console.WriteLine("Name cannot be empty. Please input again.");
            }
            string address;
            while (true)
            {
                Console.Write("Please input address: ");
                address = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(address))
                    break;
                Console.WriteLine("Address cannot be empty. Please input again.");
            }
            double salary;
            while (true)
            {
                Console.Write("Please input salary: ");
                string? sSalary = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sSalary))
                {
                    Console.WriteLine("You must input Salary.");
                    continue;
                }

                if (!double.TryParse(sSalary, NumberStyles.Any, CultureInfo.InvariantCulture, out salary))
                {
                    Console.WriteLine("You must input digit.");
                    continue;
                }

                if (salary <= 0)
                {
                    Console.WriteLine("Salary is greater than zero");
                    continue;
                }
                break;
            }

            return new Person(name ?? "", address ?? "", salary);
        }
        static void DisplayPersonInfo(Person person)
        {
            Console.WriteLine("Name: {0}", person.Name);
            Console.WriteLine("Address: {0}", person.Address);
            Console.WriteLine("Salary: {0}", person.Salary);
            Console.WriteLine();
        }
        static void SortBySalary(Person[] people)
        {
            bool swapped = false;
            for (int i = 0; i < people.Length - 1; i++)
            {
                for (int j = 0; j < people.Length - i - 1; j++)
                {
                    if (people[j].Salary > people[j + 1].Salary)
                    {
                        var tmp = people[j];
                        people[j] = people[j + 1];
                        people[j + 1] = tmp;
                        swapped = true;
                    }
                }
                if (!swapped)
                {
                    break;
                }
            }
        }
    }
}