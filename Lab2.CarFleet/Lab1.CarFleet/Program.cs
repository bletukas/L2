//---------------------------------------------------------------------
// P175B118 Objektinis programavimas 1
// Laboratorinis darbas 1. Duomenu klase. Uzduotis U1-2. Automobiliu parkas.
// Author: Vladas Jagorkinas, group IFF-6/4
//
// The program reads the data about the cars of UAB "Zaibas", finds the
// manufacturer that has the largest number of cars, selects the cars of
// the "Volvo" make and writes the cars older than ten years into a
// separate CSV file.
//---------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab2.CarFleet
{
    /// <summary>
    /// Main class of the program. Only calls the methods of the other classes.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method of the program.
        /// </summary>
        /// <param name="args">arguments of the command line, not used</param>
        static void Main(string[] args)
        {
            // Name of the file that holds the initial data
            const string CDataFile1 = @"Filialas1_2var.csv";
            const string CDataFile2 = @"Filialas2_2var.csv";
            // Name of the file that receives the table of the initial data
            const string CResultsFile = @"Pradiniai_duomenys_lentele.txt";
            // Name of the file that receives the cars older than the age limit
            const string COldCarsFile = @"Senienos.csv";
            const string CExpiringCarsFile = @"Apziura.csv";
            // Manufacturer whose cars have to be selected
            const string CSelectedManufacturer = "Volvo";
            // Age limit of an old car in full years
            const int CAgeLimitInYears = 10;

            Console.OutputEncoding = Encoding.UTF8;

            // Both result files are rewritten on every run of the program, so
            // that the output of an earlier run can never be mistaken for the
            // result of the current one
            if (File.Exists(CResultsFile))
            {
                File.Delete(CResultsFile);
            }
            if (File.Exists(COldCarsFile))
            {
                File.Delete(COldCarsFile);
            }

            if (File.Exists(CExpiringCarsFile))
            {
                File.Delete(CExpiringCarsFile);
            }

            List<string> Messages = new List<string>();

            if (!File.Exists(CDataFile1) && !File.Exists(CDataFile2))
            {
                Messages.Add(String.Format("Duomenų failai {0} nerasti.", CDataFile1));
                InOutUtils.PrintToScreen(Messages);
                InOutUtils.PrintToFile(CResultsFile, Messages);
            }
            else
            {
                

                if (new FileInfo(CDataFile1).Length == 0)
                {
                    Messages.Add(String.Format("Duomenų faile {0} duomenų nėra.", CDataFile1));
                    InOutUtils.PrintToScreen(Messages);
                    InOutUtils.PrintToFile(CResultsFile, Messages);
                    Messages.Clear();
                }
                if (new FileInfo(CDataFile2).Length == 0)
                {
                    Messages.Add(String.Format("Duomenų faile {0} duomenų nėra.", CDataFile2));
                    InOutUtils.PrintToScreen(Messages);
                    InOutUtils.PrintToFile(CResultsFile, Messages);
                }
                else
                {
                    CarRegister branch1 = InOutUtils.ReadCars(CDataFile1);
                    CarRegister branch2 = InOutUtils.ReadCars(CDataFile2);
                    List<string> InitialDataTable = branch1.FormInitialDataTableHeader();
                    List<string> InitialDataTable2 = branch2.FormInitialDataTableHeader();
                    InitialDataTable.AddRange(branch1.FormCarTable(String.Format("Pradiniai duomenys (iš viso automobilių: {0}):", branch1.GetCount())));
                    InitialDataTable2.AddRange(branch2.FormCarTable(String.Format("Pradiniai duomenys (iš viso automobilių: {0}):", branch2.GetCount())));
                    InitialDataTable.AddRange(InitialDataTable2);
                    InOutUtils.PrintToScreen(InitialDataTable);
                    InOutUtils.PrintToFile(CResultsFile, InitialDataTable);

                    // First task. The manufacturer that has the largest number of cars
                    List<string> AllManufacturersBranch1 = branch1.FindDifferentManufacturers();
                    int maxCarCountBranch1 = branch1.FindMaxCarCountByManufacturer(AllManufacturersBranch1);
                    CarRegister MostFrequentCarsBranch1 = branch1.FindManufacturersByCarCount(maxCarCountBranch1);
                    List<string> AllManufacturersBranch2 = branch1.FindDifferentManufacturers();
                    int maxCarCountBranch2 = branch2.FindMaxCarCountByManufacturer(AllManufacturersBranch1);
                    CarRegister MostFrequentCarsBranch2 = branch1.FindManufacturersByCarCount(maxCarCountBranch2);
                    CarRegister MostFrequentCars = MostFrequentCarsBranch1 + MostFrequentCarsBranch2;
                    MostFrequentCars.

                    List<string> MostFrequentTable = InOutUtils.FormManufacturerTable(MostFrequentCars.FindDifferentManufacturers(), maxCarCount, "Daugiausiai automobilių turintys gamintojai:");
                    InOutUtils.PrintToScreen(MostFrequentTable);

                    // Second task. The cars of the selected manufacturer
                    CarRegister SelectedCars = combined.FilterCarsByManufacturer(CSelectedManufacturer);
                    List<string> SelectedCarsTable = InOutUtils.FormShortCarTable(SelectedCars, String.Format("Gamintojo {0} automobiliai:", CSelectedManufacturer));
                    InOutUtils.PrintToScreen(SelectedCarsTable);

                    // Third task. The cars that are older than the age limit
                    CarRegister OldCars = combined.FilterCarsOlderThan(CAgeLimitInYears);
                    List<string> OldCarsTable = InOutUtils.FormCarTable(OldCars, String.Format("Senesni nei {0} metų automobiliai:", CAgeLimitInYears));
                    InOutUtils.PrintToScreen(OldCarsTable);
                    InOutUtils.PrintCarsToCSVFile(COldCarsFile, OldCars);
                    
                    //Fourth task. Find the newest car and print it to console
                    CarRegister newest = combined.FindNewestCar();
                    List<string> NewestCarsTable = newest.FormCarTable("Naujausi automobiliai:");
                    InOutUtils.PrintToScreen(NewestCarsTable);
                    
                    //Fifth task. Find expiring or expired tech inspection and print to .csv file
                    CarRegister expiring = combined.FindExpiringInspection();
                    InOutUtils.PrintExpiredCarsToCSVFile(CExpiringCarsFile, expiring);
                }
            }

            Console.ReadKey();
        }
    }
}
