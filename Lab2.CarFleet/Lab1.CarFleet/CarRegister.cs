using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.CarFleet
{
    class CarRegister
    {
        private List<Car> AllCars;
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber {  get; set; }
        // Width of the table that holds all the data of a car
        private const int CCarTableWidth = 94;

        // Width of the table that holds the plate number, the model and the year
        private const int CShortCarTableWidth = 47;

        // Width of the table that holds the manufacturers and their car counts
        private const int CManufacturerTableWidth = 28;

        // Text that is placed instead of a table that would have no rows
        private const string CNoDataMessage = "Sąlygą atitinkančių duomenų nėra.";

        // Format of one data row of the full car table
        private const string CCarRowFormat = "| {0,-9} | {1,-13} | {2,-12} | {3,9:yyyy-MM} | {4,12:yyyy-MM-dd} | {5,-9} | {6,8:F1} |";

        // Format of the heading row of the full car table
        private const string CCarHeadFormat = "| {0,-9} | {1,-13} | {2,-12} | {3,9} | {4,12} | {5,-9} | {6,8} |";

        // Format of one data row of the shortened car table
        private const string CShortCarRowFormat = "| {0,-9} | {1,-12} | {2,16:yyyy} |";

        // Format of the heading row of the shortened car table
        private const string CShortCarHeadFormat = "| {0,-9} | {1,-12} | {2,16} |";

        // Format of one row of the manufacturer table
        private const string CManufacturerRowFormat = "| {0,-13} | {1,8} |";

        public CarRegister()
        {
            AllCars = new List<Car>();
        }
        public CarRegister(string city, string street, string phoneNumber)
        {
            AllCars = new List<Car> ();
            this.City = city;
            this.Street = street;
            this.PhoneNumber = phoneNumber;
        }
        public void Add(Car car) 
        { 
            AllCars.Add (car);
        }
        public Car FindCarByIndex(int index)
        {
            return AllCars [index];
        }
        public int GetCount ()
        {
            return AllCars.Count;
        }
        public List<string> FindDifferentManufacturers()
        {
            List<string> Manufacturers = new List<string>();
            foreach (Car car in this.AllCars)
            {
                string manufacturer = car.Manufacturer;
                if (!Manufacturers.Contains(manufacturer)) // uses List method Contains()
                {
                    Manufacturers.Add(manufacturer);
                }
            }
            return Manufacturers;
        }
        public int CountCarsByManufacturer(string manufacturer)
        {
            int carCount = 0;
            foreach (Car car in this.AllCars)
            {
                if (car.Manufacturer.Equals(manufacturer)) // uses string method Equals()
                {
                    carCount++;
                }
            }
            return carCount;
        }
        public int FindMaxCarCountByManufacturer(List<string> Manufacturers)
        {
            int maxCarCount = int.MinValue; // smallest value an int can hold
            foreach (string manufacturer in Manufacturers)
            {
                int carCount = CountCarsByManufacturer(manufacturer);
                if (carCount > maxCarCount)
                {
                    maxCarCount = carCount;
                }
            }
            return maxCarCount;
        }
        public CarRegister FindManufacturersByCarCount(int requiredCarCount)
        {
            CarRegister register = new CarRegister();
            foreach (Car car in this.AllCars)
            {
                if (CountCarsByManufacturer(car.Manufacturer) == requiredCarCount)
                {
                    register.Add(car);
                }
            }
            return register;
        }
        public CarRegister FilterCarsByManufacturer(string manufacturer)
        {
            CarRegister FilteredCars = new CarRegister();
            foreach (Car car in this.AllCars)
            {
                if (car.Manufacturer.Equals(manufacturer))
                {
                    FilteredCars.Add(car);
                }
            }
            return FilteredCars;
        }
        public CarRegister FilterCarsOlderThan(int ageLimitInYears)
        {
            CarRegister FilteredCars = new CarRegister();
            foreach (Car car in this.AllCars)
            {
                if (car.IsOlderThan(ageLimitInYears))
                {
                    FilteredCars.Add(car);
                }
            }
            return FilteredCars;
        }
        public List<string> FormCarTable(string tableTitle)
        {
            List<string> TableLines = new List<string>();
            TableLines.Add(tableTitle);
            if (this.AllCars.Count == 0)
            {
                TableLines.Add(CNoDataMessage);
                TableLines.Add("");
                return TableLines;
            }
            TableLines.Add(new string('-', CCarTableWidth));
            TableLines.Add(String.Format(CCarHeadFormat, "Valst.Nr.", "Gamintojas", "Modelis", "Pagaminta", "Tech.apžiūra", "Kuras", "Sąnaudos"));
            TableLines.Add(new string('-', CCarTableWidth));
            foreach (Car car in this.AllCars)
            {
                TableLines.Add(String.Format(CCarRowFormat, car.PlateNumber, car.Manufacturer, car.Model, car.ManufactureDate, car.InspectionValidUntil, car.Fuel, car.FuelConsumption));
            }
            TableLines.Add(new string('-', CCarTableWidth));
            TableLines.Add("");
            return TableLines;
        }

        public DateTime FindNewestDate()
        {
            DateTime newestDate = new DateTime();
            foreach (Car car in this.AllCars)
            {
                if (newestDate.CompareTo(car.ManufactureDate) < 0)
                {
                    newestDate = car.ManufactureDate;
                }
            }
            return newestDate;
        }

        public CarRegister FindNewestCar()
        {
            DateTime newestDate = FindNewestDate();
            CarRegister newest = new CarRegister();
            foreach (Car car in this.AllCars)
            {
                if (car.ManufactureDate.Equals(newestDate))
                {
                    newest.Add(car);
                }
            }
            return newest;
        }

        public CarRegister FindExpiringInspection()
        {
            CarRegister expiring = new CarRegister();
            foreach (Car car in this.AllCars)
            {
                if (car.InspectionExpiring())
                {
                    expiring.Add(car);
                }
            }
            return expiring;
        }
        public List<string> FormInitialDataTableHeader()
        {
            List<string> header = new List<string>();
            header.Add(this.City);
            header.Add(this.Street);
            header.Add(this.PhoneNumber);
            return header;
        }
        public CarRegister CompareMaxCarCount(CarRegister register)
        {
            if (register.GetCount()/register.FindDifferentManufacturers().Count() > this.GetCount()/this.FindDifferentManufacturers().Count)
            {
                return register;
            }
            else
            {
                return this;
            }
        }
        public static CarRegister operator +(CarRegister register1, CarRegister register2)
        {
            CarRegister Cars = register1;

            for (int i = 0; i < register2.GetCount(); i++)
            {
                Car selected = register2.FindCarByIndex(i);
                Cars.Add(selected);
            }
            return Cars;
        }
    }
}
