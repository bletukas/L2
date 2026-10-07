//---------------------------------------------------------------------
// Static class for reading the initial data, forming the tables of the
// results and printing the formed tables on the screen or into a file.
//---------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lab2.CarFleet
{
    /// <summary>
    /// Input and output operations of the car fleet program.
    /// </summary>
    static class InOutUtils
    {
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

        /// <summary>
        /// Reads the data of the cars from a CSV file. Empty lines are skipped.
        /// </summary>
        /// <param name="dataFileName">name of the initial data file</param>
        /// <returns>collection of the cars described in the file</returns>
        public static CarRegister ReadCars(string dataFileName)
        {
           
            string[] FileLines = File.ReadAllLines(dataFileName, Encoding.UTF8);
            string city = FileLines[0];
            string street = FileLines[1];
            string phoneNumber = FileLines[2];
            CarRegister Cars = new CarRegister(city, street, phoneNumber);

           for (int i = 3; i < FileLines.Length; i++)
            {
                if (FileLines[i].Length > 0)
                {
                    string line = FileLines[i];
                    string[] Fields = line.Split(';');
                    string plateNumber = Fields[0];
                    string manufacturer = Fields[1];
                    string model = Fields[2];
                    // The initial data define only the year and the month of
                    // manufacture, therefore the day is always set to the first one
                    DateTime manufactureDate = new DateTime(int.Parse(Fields[3]),int.Parse(Fields[4]), 1);
                    string[] dateParts = Fields[5].Split('-');
                    DateTime inspectionValidUntil = new  DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]));
                    FuelType fuel;
                    Enum.TryParse(Fields[6], out fuel); // tries to convert value to enum
                    double fuelConsumption = double.Parse(Fields[7], CultureInfo.InvariantCulture);
                    Car car = new Car(plateNumber, manufacturer, model, manufactureDate, inspectionValidUntil, fuel, fuelConsumption);
                    Cars.Add(car);
                }
            }
            return Cars;
        }

        /// <summary>
        /// Forms the table that holds all the data of the given cars.
        /// </summary>
        /// <param name="Cars">collection of the cars to place into the table</param>
        /// <param name="tableTitle">explanatory title placed above the table</param>
        /// <returns>formed lines of the table</returns>
        public static List<string> FormCarTable(CarRegister Cars, string tableTitle)
        {
            List<string> TableLines = new List<string>();
            TableLines.Add(tableTitle);
            if (Cars.GetCount() == 0)
            {
                TableLines.Add(CNoDataMessage);
                TableLines.Add("");
                return TableLines;
            }
            TableLines.Add(new string('-', CCarTableWidth));
            TableLines.Add(String.Format(CCarHeadFormat, "Valst.Nr.", "Gamintojas", "Modelis", "Pagaminta", "Tech.apžiūra", "Kuras", "Sąnaudos"));
            TableLines.Add(new string('-', CCarTableWidth));
            for (int i = 0; i < Cars.GetCount(); i++)
            {
                Car selected = Cars.FindCarByIndex(i);
                TableLines.Add(String.Format(CCarRowFormat, selected.PlateNumber, selected.Manufacturer, selected.Model, selected.ManufactureDate, selected.InspectionValidUntil, selected.Fuel, selected.FuelConsumption));
            }
            TableLines.Add(new string('-', CCarTableWidth));
            TableLines.Add("");
            return TableLines;
        }

        /// <summary>
        /// Forms the table that holds the plate number, the model and the year
        /// of manufacture of the given cars.
        /// </summary>
        /// <param name="Cars">collection of the cars to place into the table</param>
        /// <param name="tableTitle">explanatory title placed above the table</param>
        /// <returns>formed lines of the table</returns>
        public static List<string> FormShortCarTable(CarRegister register, string tableTitle)
        {
            List<string> TableLines = new List<string>();
            TableLines.Add(tableTitle);
            if (register.GetCount() == 0)
            {
                TableLines.Add(CNoDataMessage);
                TableLines.Add("");
                return TableLines;
            }
            TableLines.Add(new string('-', CShortCarTableWidth));
            TableLines.Add(String.Format(CShortCarHeadFormat, "Valst.Nr.", "Modelis", "Pagaminimo metai"));
            TableLines.Add(new string('-', CShortCarTableWidth));
            for (int i = 0; i < register.GetCount(); i++)
            {
                Car selected = register.FindCarByIndex(i);
                TableLines.Add(String.Format(CShortCarRowFormat, selected.PlateNumber, selected.Model, selected.ManufactureDate));
            }
            TableLines.Add(new string('-', CShortCarTableWidth));
            TableLines.Add("");
            return TableLines;
        }

        /// <summary>
        /// Forms the table of the manufacturers that all have the same car count.
        /// </summary>
        /// <param name="Manufacturers">names of the manufacturers</param>
        /// <param name="carCount">number of cars that each of them has</param>
        /// <param name="tableTitle">explanatory title placed above the table</param>
        /// <returns>formed lines of the table</returns>
        public static List<string> FormManufacturerTable(List<string> Manufacturers, int carCount, string tableTitle)
        {
            List<string> TableLines = new List<string>();
            TableLines.Add(tableTitle);
            if (Manufacturers.Count == 0)
            {
                TableLines.Add(CNoDataMessage);
                TableLines.Add("");
                return TableLines;
            }
            TableLines.Add(new string('-', CManufacturerTableWidth));
            TableLines.Add(String.Format(CManufacturerRowFormat, "Gamintojas", "Kiekis"));
            TableLines.Add(new string('-', CManufacturerTableWidth));
            foreach (string manufacturer in Manufacturers)
            {
                TableLines.Add(String.Format(CManufacturerRowFormat, manufacturer, carCount));
            }
            TableLines.Add(new string('-', CManufacturerTableWidth));
            TableLines.Add("");
            return TableLines;
        }

        /// <summary>
        /// Prints the formed lines on the screen.
        /// </summary>
        /// <param name="TextLines">lines to print</param>
        public static void PrintToScreen(List<string> TextLines)
        {
            foreach (string textLine in TextLines)
            {
                Console.WriteLine(textLine);
            }
        }

        /// <summary>
        /// Adds the formed lines to the end of a text file.
        /// </summary>
        /// <param name="resultsFileName">name of the results file</param>
        /// <param name="TextLines">lines to write</param>
        public static void PrintToFile(string resultsFileName, List<string> TextLines)
        {
            File.AppendAllLines(resultsFileName, TextLines, Encoding.UTF8);
        }

        /// <summary>
        /// Writes all the data of the given cars into a CSV file.
        /// </summary>
        /// <param name="csvFileName">name of the created CSV file</param>
        /// <param name="Cars">collection of the cars to write</param>
        public static void PrintCarsToCSVFile(string csvFileName, CarRegister register)
        {
            string[] CsvLines = new string[register.GetCount() + 1];
            CsvLines[0] = String.Format("{0};{1};{2};{3};{4};{5};{6}", "Valstybinis numeris", "Gamintojas", "Modelis", "Pagaminimo metai ir mėnuo", "Techninės apžiūros galiojimas", "Kuras", "Vidutinės kuro sąnaudos, l/100km");
            for (int i = 0; i < register.GetCount(); i++)
            {
                Car selected = register.FindCarByIndex(i);
                CsvLines[i + 1] = String.Format("{0};{1};{2};{3:yyyy-MM};{4:yyyy-MM-dd};{5};{6:F1}", selected.PlateNumber, selected.Manufacturer, selected.Model, selected.ManufactureDate, selected.InspectionValidUntil, selected.Fuel, selected.FuelConsumption);
            }
            File.WriteAllLines(csvFileName, CsvLines, Encoding.UTF8);
        }

        public static void PrintExpiredCarsToCSVFile(string csvFileName, CarRegister register)
        {
            string[] CsvLines = new string[register.GetCount() + 1];
            if (register.GetCount() == 0)
            {
                CsvLines[0] = String.Format("Duomenų nėra");
                File.WriteAllLines(csvFileName, CsvLines, Encoding.UTF8);
            }
            else
            {
                CsvLines[0] = String.Format("{0};{1};{2};{3}", "Gamintojas", "Modelis", "Valstybinis numeris",
                    "Techninės apžiūros galiojimas");
                for (int i = 0; i < register.GetCount(); i++)
                {
                    Car selected = register.FindCarByIndex(i);
                    if (selected.InspectionValidUntil.CompareTo(DateTime.Now) > 0)
                    {
                        CsvLines[i + 1] = String.Format("{0};{1};{2};{3:yyyy-MM-dd}", selected.Manufacturer, selected.Model,
                            selected.PlateNumber, selected.InspectionValidUntil);
                    }
                    else
                    {
                        CsvLines[i + 1] = String.Format("{0};{1};{2};{3}", selected.Manufacturer, selected.Model,
                            selected.PlateNumber, "SKUBIAI");
                    }

                    File.WriteAllLines(csvFileName, CsvLines, Encoding.UTF8);
                }
            }
        }
        
    }
}
