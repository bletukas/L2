//---------------------------------------------------------------------
// Static class for the calculations of the car fleet task.
// Every method here only calculates, it never prints, and every
// method contains no more than one loop.
//---------------------------------------------------------------------
using System.Collections.Generic;

namespace Lab2.CarFleet
{
    /// <summary>
    /// Calculations performed on the collection of the cars.
    /// </summary>
    static class TaskUtils
    {
        /// <summary>
        /// Finds all the different manufacturers of the given cars.
        /// </summary>
        /// <param name="Cars">collection of the cars</param>
        /// <returns>manufacturer names without repetitions</returns>
        public static List<string> FindDifferentManufacturers(List<Car> Cars)
        {
            List<string> Manufacturers = new List<string>();
            foreach (Car car in Cars)
            {
                string manufacturer = car.Manufacturer;
                if (!Manufacturers.Contains(manufacturer)) // uses List method Contains()
                {
                    Manufacturers.Add(manufacturer);
                }
            }
            return Manufacturers;
        }

        /// <summary>
        /// Counts how many cars of the given manufacturer there are.
        /// </summary>
        /// <param name="Cars">collection of the cars</param>
        /// <param name="manufacturer">name of the manufacturer</param>
        /// <returns>number of the cars of that manufacturer</returns>
        public static int CountCarsByManufacturer(List<Car> Cars, string manufacturer)
        {
            int carCount = 0;
            foreach (Car car in Cars)
            {
                if (car.Manufacturer.Equals(manufacturer)) // uses string method Equals()
                {
                    carCount++;
                }
            }
            return carCount;
        }

        /// <summary>
        /// Finds the largest number of cars that one manufacturer has.
        /// </summary>
        /// <param name="Cars">collection of the cars</param>
        /// <param name="Manufacturers">names of the different manufacturers</param>
        /// <returns>largest number of cars of one manufacturer</returns>
        public static int FindMaxCarCountByManufacturer(List<Car> Cars, List<string> Manufacturers)
        {
            int maxCarCount = int.MinValue; // smallest value an int can hold
            foreach (string manufacturer in Manufacturers)
            {
                int carCount = CountCarsByManufacturer(Cars, manufacturer);
                if (carCount > maxCarCount)
                {
                    maxCarCount = carCount;
                }
            }
            return maxCarCount;
        }

        /// <summary>
        /// Selects the manufacturers that have exactly the given number of cars.
        /// </summary>
        /// <param name="Cars">collection of the cars</param>
        /// <param name="Manufacturers">names of the different manufacturers</param>
        /// <param name="requiredCarCount">number of cars a manufacturer must have</param>
        /// <returns>names of the selected manufacturers</returns>
        public static List<string> FindManufacturersByCarCount(List<Car> Cars, List<string> Manufacturers, int requiredCarCount)
        {
            List<string> FoundManufacturers = new List<string>();
            foreach (string manufacturer in Manufacturers)
            {
                if (CountCarsByManufacturer(Cars, manufacturer) == requiredCarCount)
                {
                    FoundManufacturers.Add(manufacturer);
                }
            }
            return FoundManufacturers;
        }

        /// <summary>
        /// Selects the cars of the given manufacturer.
        /// </summary>
        /// <param name="Cars">collection of the cars</param>
        /// <param name="manufacturer">name of the manufacturer</param>
        /// <returns>cars of that manufacturer</returns>
        public static List<Car> FilterCarsByManufacturer(List<Car> Cars, string manufacturer)
        {
            List<Car> FilteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.Manufacturer.Equals(manufacturer))
                {
                    FilteredCars.Add(car);
                }
            }
            return FilteredCars;
        }

        /// <summary>
        /// Selects the cars that are older than the given number of years.
        /// </summary>
        /// <param name="Cars">collection of the cars</param>
        /// <param name="ageLimitInYears">age limit in full years</param>
        /// <returns>cars older than the age limit</returns>
        public static List<Car> FilterCarsOlderThan(List<Car> Cars, int ageLimitInYears)
        {
            List<Car> FilteredCars = new List<Car>();
            foreach (Car car in Cars)
            {
                if (car.IsOlderThan(ageLimitInYears))
                {
                    FilteredCars.Add(car);
                }
            }
            return FilteredCars;
        }
    }
}
