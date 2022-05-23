/*
 * FILE             : main.cs
 * PROJECT          : Data Analysis
 * FIRST VERSION    : May 3rd 2021
 * LAST UPDATE      : May 22nd 2022
 * Parameters       : String of filename/path to open, else uses the default
 * DESCRIPTION      : Simple program to parse through a csv file to calculate some useful metrics
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sneakerData
{
    /* 
     * Struct       : sneakerSale
     * Description  : Struct to hold the record data for a single transaction of sneakers
     */
    public struct sneakerSale
    {
        public int OrderID;
        public int ShopID;
        public int UserID;
        public int OrderAmount;
        public int TotalItems;
        public string PaymentMethod;
        public string DateTimeMade;
    }


    class main
    {
        /*
         * METHOD       : main
         * PARAMETERS   : path + file name of a .csv as a string, otherwise will use a bad default name
         * RETURNS      : int - 0 on success, 1 on unable to read file
         */
        public static int Main(string[] args)
        {
            List<sneakerSale> ShopOrders = new List<sneakerSale>();
            string fileLocation = "";

            //Check to see if a filename was given
            if (args.Length != 0)
            {
                fileLocation = args[0];
            }
            else
            {
                //This assumes the file is named exactly this and in the same folder as the .exe
                fileLocation = "2019 Winter Data Science Intern Challenge Data Set - Sheet1.csv";
            }

            try
            {
                //Try reading the file
                readValues(ShopOrders, fileLocation);
                //process the values from the file and print to console
                produceValues(ShopOrders);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return 0;
            }
            catch
            {
                //File not found or unable to open
                Console.WriteLine("Could not open file {0}", fileLocation);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return 1;
            }
        }

        /*
         * METHOD       : readValues
         * PARAMETERS   : ShopOrders    : List of sneakerSale structs, containing the seperate 
         * DESCRIPTION  : Calculates the given metric of Average Order Value, and compares it to the Average Item Value instead
         *              : This includes an exclusion of outliars like the massive order from likely a supplier
         */
        static void produceValues(List<sneakerSale> ShopOrders)
        {
            int totalOrderValue = 0;
            int orderCount = ShopOrders.Count();
            int itemCount = 0;
            int[] orderValue = new int[ShopOrders.Count()];
            int[] orderVolume = new int[ShopOrders.Count()];
            int i = 0;

            //All inclusive
            foreach (sneakerSale order in ShopOrders)
            {
                totalOrderValue += order.OrderAmount;
                itemCount += order.TotalItems;
                orderValue[i] = order.OrderAmount;
                orderVolume[i] = order.TotalItems;
                i++;
            }
            //print useful values to the screen
            Console.WriteLine("With Outliers (All Data)");
            Console.WriteLine("Average Order Value : $" + Math.Round((float)(totalOrderValue/(float)orderCount), 2));
            Console.WriteLine("Average Item Value  : $" + Math.Round((float)(totalOrderValue / (float)itemCount), 2));
            Console.WriteLine("Average Order Volume: " + Math.Round((float)(itemCount / (float)orderCount), 2) + " items per order\n");

            //Median
            Array.Sort(orderValue);
            Array.Sort(orderVolume);
            Console.WriteLine("Median Order Value  : " + orderValue[(orderValue.Length/2)]);
            Console.WriteLine("Median Order Volume : " + orderVolume[(orderVolume.Length/2)]);

        }
        /*
         * METHOD       : readValues
         * PARAMETERS   : ShopOrders    : List of sneakerSale structs, containing the seperate 
         * PARAMETERS   : fileLocation  : String representing the filename / path
         */
        static void readValues(List<sneakerSale> ShopOrders, string fileLocation)
        {
            string newData = "";
            string[] splitData;
            sneakerSale newOrder;
            StreamReader fileReader = new StreamReader(fileLocation);
            
            //First row is column names, toss it out
            newData = fileReader.ReadLine();

            //While not end of file
            while (!fileReader.EndOfStream)
            {
                newData = fileReader.ReadLine();
                splitData = newData.Split(',');

                //Load the struct
                newOrder = new sneakerSale();
                newOrder.OrderID = int.Parse(splitData[0]);
                newOrder.ShopID = int.Parse(splitData[1]);
                newOrder.UserID = int.Parse(splitData[2]);
                newOrder.OrderAmount = int.Parse(splitData[3]);
                newOrder.TotalItems = int.Parse(splitData[4]);
                newOrder.PaymentMethod = splitData[5];
                newOrder.DateTimeMade = splitData[6];

                ShopOrders.Add(newOrder);
            }
            //Close the file when done
            fileReader.Close();
        }
    }
}
