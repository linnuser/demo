using LinnScannerLogic.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace LinnScannerLogic
{
    public class BusinessLogic
    {
        private Timer Timer;
        private readonly string ScanDirectoryName;
        private readonly string OutputDirectoryName;
        private readonly DirectoryInfo ScanDirectory;
        private readonly DirectoryInfo OutputDirectory;

        private readonly List<Shipment> Shipments;
        private readonly List<Order> Orders;
        private readonly List<OrderItem> OrderItems;

        public BusinessLogic(string scanDirectory, string outputDirectory)
        {
            ValidateDirectory(scanDirectory);
            ValidateDirectory(outputDirectory);

            ScanDirectoryName = scanDirectory;
            OutputDirectoryName = outputDirectory;

            ScanDirectory = new DirectoryInfo(ScanDirectoryName);
            OutputDirectory = new DirectoryInfo(OutputDirectoryName);

            Shipments = new List<Shipment>();
            Orders = new List<Order>();
            OrderItems = new List<OrderItem>();

            Timer = new Timer
            {
                Interval = 5000,//milliseconds  //todo config item or parse in
                Enabled = true,
            };

            Timer.Elapsed += new ElapsedEventHandler(ProcessDirecotry);

            Timer.Start();
        }

        /// <summary>
        /// Main Businns logic
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void ProcessDirecotry(object source, ElapsedEventArgs e)
        {
            try
            {
                Timer.Stop(); // Stop the timer until we finish processing
                var filesToProces = GetFilesToProcess();
                ProcessFiles<ShipmentRootObject, Shipment>(filesToProces, Shipments);
                ProcessFiles<OrderRootObject, Order>(filesToProces, Orders);
                ProcessFiles<OrderItemRootObject, OrderItem>(filesToProces, OrderItems);
                MoveFile(filesToProces, "error");
                var CompletedOrders = GenerateCompletedOrders();
                CreateCSV(CompletedOrders);
            }
            finally
            {
                Timer.Start();
            }
        }

        /// <summary>
        /// Creates CSV from a completedOrder
        /// </summary>
        /// <param name="completedOrders"></param>
        private void CreateCSV(List<CompletedOrder> completedOrders)
        {
            foreach (var completedOrder in completedOrders)
            {
                var lines = completedOrder.GenerateCsvLines();
                using (var writer = new StreamWriter($"{OutputDirectory}/{completedOrder.Order.Marketplace}-{completedOrder.Order.OrderReference}.csv"))
                {
                    var csv = new CsvHelper.CsvWriter(writer);
                    csv.WriteRecords(lines);
                }
            }
        }

        /// <summary>
        /// Checks to see if all 3 parts of an order are there and id so a completed object is created
        /// </summary>
        /// <returns></returns>
        private List<CompletedOrder> GenerateCompletedOrders()
        {
            var completedOrders = new List<CompletedOrder>();
            var ordersToDelete = new List<Order>();

            foreach (var order in Orders)
            {
                try
                {
                    //check we have a shipment record. We should have 1 or none
                    // if we don't have a shipment then move on
                    var shipment = Shipments.Where(s => s.Marketplace == order.Marketplace && s.OrderRefernce == order.OrderReference).First();

                    // Check to see if we have order items
                    var orderItems = OrderItems.Where(oi => oi.Marketplace == order.Marketplace && oi.OrderReference == order.OrderReference);
                    if (!(orderItems?.Any() ?? false))
                    {
                        throw new Exception("We don't have any order items to continue");
                    }

                    completedOrders.Add(new CompletedOrder
                    {
                        Order = order,
                        Shipment = shipment,
                        OrderItems = orderItems.ToList()
                    });

                    //remove items that have been processed
                    ordersToDelete.Add(order);
                    Shipments.Remove(shipment);
                    foreach (var orderItem in orderItems)
                    {
                        OrderItems.Remove(orderItem);
                    }
                }
                catch (Exception)
                {
                    // We do not have a compelted order at this time
                    // This is fine we will try again later when more files are read
                }
            }

            foreach (var orderToDelte in ordersToDelete)
            {
                Orders.Remove(orderToDelte);
            }

            return completedOrders;
        }

        /// <summary>
        /// Moves Files from one location to another
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type">Error/Done</param>
        private void MoveFile(List<string> files, string type)
        {
            var dir = $"{ScanDirectoryName}/{type}";
            if (files.Any())
            {
                Directory.CreateDirectory(dir);
                foreach (var file in files)
                {
                    File.Move(file, file.Replace(ScanDirectoryName, dir));
                }
            }
        }

        /// <summary>
        /// Get  all Json files to process
        /// </summary>
        /// <returns></returns>
        private List<string> GetFilesToProcess()
        {
            return ScanDirectory.GetFiles("*.json")?.Select(f => f.FullName)?.ToList();//todo make config option
        }

        /// <summary>
        /// Reads a Json file and adds the cotnents to a List of that type
        /// </summary>
        /// <typeparam name="J">RootObjectType of Json File</typeparam>
        /// <typeparam name="T">The Content of the RootObjectType</typeparam>
        /// <param name="files"></param>
        /// <param name="output"></param>
        private void ProcessFiles<J, T>(List<string> files, List<T> output)
        {
            var filesToRemove = new List<string>();
            foreach (var file in files)
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        var json = streamReader.ReadToEnd();
                        dynamic rootObject = JsonConvert.DeserializeObject<J>(json); // Each file type is differnt so dynamic handles this

                        //We have mapped our rootobject collectino inside to be called Data

                        output.AddRange(rootObject.Data);

                        filesToRemove.Add(file);
                    }
                }
                catch (Exception)
                {
                    // Somethign has gone wrong so may be a differnt file
                    // Maye be corrupt data
                    // After all the steps have run wif the file has not been processed we will handle it
                }
            }

            //remove file from processing again
            foreach (var file in filesToRemove)
            {
                files.Remove(file);
            }
            MoveFile(filesToRemove, "Done");
        }

        /// <summary>
        /// Checks to see if a Directory Exists
        /// </summary>
        /// <param name="directory">Path to directory</param>
        /// <returns>True if the directory exisits</returns>
        public static bool ValidateDirectory(string directory)
        {
            try
            {
                return Directory.Exists(directory);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}