//---------------------------------------------------------------------
// Data class that describes one car of the fleet of UAB "Zaibas".
//---------------------------------------------------------------------
using System;

namespace Lab2.CarFleet
{
    /// <summary>
    /// Stores the data of a single car of the fleet.
    /// </summary>
    class Car
    {
        /// <summary>State registration number of the car.</summary>
        public string PlateNumber { get; set; }

        /// <summary>Company that manufactured the car.</summary>
        public string Manufacturer { get; set; }

        /// <summary>Model of the car.</summary>
        public string Model { get; set; }

        /// <summary>Year and month of manufacture. The day is always 1.</summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>Date until which the technical inspection stays valid.</summary>
        public DateTime InspectionValidUntil { get; set; }

        /// <summary>Type of fuel that the car uses.</summary>
        public FuelType Fuel { get; set; }

        /// <summary>Average fuel consumption, litres per 100 km.</summary>
        public double FuelConsumption { get; set; }

        /// <summary>
        /// Creates a car object and assigns the initial values of its properties.
        /// </summary>
        /// <param name="plateNumber">state registration number</param>
        /// <param name="manufacturer">company that manufactured the car</param>
        /// <param name="model">model of the car</param>
        /// <param name="manufactureDate">year and month of manufacture</param>
        /// <param name="inspectionValidUntil">date until which the inspection stays valid</param>
        /// <param name="fuel">type of fuel that the car uses</param>
        /// <param name="fuelConsumption">average fuel consumption per 100 km</param>
        public Car(string plateNumber, string manufacturer, string model, DateTime manufactureDate, DateTime inspectionValidUntil, FuelType fuel, double fuelConsumption)
        {
            this.PlateNumber = plateNumber;
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.ManufactureDate = manufactureDate;
            this.InspectionValidUntil = inspectionValidUntil;
            this.Fuel = fuel;
            this.FuelConsumption = fuelConsumption;
        }

        /// <summary>
        /// Checks whether the car is older than the given number of years on the
        /// current date. A car that is exactly that old is not older than it.
        /// </summary>
        /// <param name="ageLimitInYears">age limit in years</param>
        /// <returns>true when the car is older than the age limit</returns>
        public bool IsOlderThan(int ageLimitInYears)
        {
            DateTime oldestAllowedDate = DateTime.Today.AddYears(-ageLimitInYears);
            return this.ManufactureDate < oldestAllowedDate;
        }

        public bool InspectionExpiring()
        {
            DateTime today =  DateTime.Today;
            DateTime threshold = today.AddMonths(1);
            if (InspectionValidUntil.CompareTo(today) < 0)
            {
                return true;
            }
            else if (InspectionValidUntil.CompareTo(threshold) <= 0)
            {
                return true;
            }
            else
            {
                return false; 
            }
            
        }
    }
}
