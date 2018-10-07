using LinnScannerLogic;
using System;
using System.Linq;

namespace ConsoleApp
{
    // working C# console application, that periodically scans a location specified in the command​ ​line
    //outputs​ ​files​ ​into​ ​a​ ​location​ ​specified​ ​in​ ​command​ ​line​ ​parameter
    internal class Program
    {
        private static readonly string HelpCommand = "-?";

        private static void Main(string[] args)
        {
            try
            {
                CheckForHelp(args);
                ValidateArgs(args);
                var logic = new BusinessLogic(args[0], args[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void ValidateArgs(string[] args)
        {
            ValiateNumberOrArgs(args);

            //ValiateNumberOrArgs only allows 2 file arguments. We need to check these.
            ValidateArgsFileDir(args[0]);
            ValidateArgsFileDir(args[1]);
        }

        private static void ValidateArgsFileDir(string arg)
        {
            try
            {
                if (!BusinessLogic.ValidateDirectory(arg))
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new Exception($"{arg} is not a an existing file directory");
            }
        }

        private static void ValiateNumberOrArgs(string[] args)
        {
            // The help option has already been checked so we do not need to validate it

            var numberOfArgs = args?.Length ?? 0;
            var requiredNumberOfArge = 2;

            if (numberOfArgs != requiredNumberOfArge)
            {
                throw new Exception($"This application expects {requiredNumberOfArge} arguments to run. {numberOfArgs} was sent. Try {HelpCommand} for help");
            }
        }

        private static void CheckForHelp(string[] args)
        {
            if (args.Any(a => a == HelpCommand))
            {
                DisplayHelp();
                Environment.Exit(0); //close application
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine(@"
       Usage:

            LinnScanner  [/? | [scanLocation] | [outputLocation]

        Where:

            /?                  Display this help message
            [scanlocation]      The file directory to scan
            [outputLocation]    The location to output CSV files

        Example:

            LinnScanner C:\Scan C:\Csv");
        }
    }
}