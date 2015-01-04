using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DumpDatabaseToSql;

namespace DumpDatabaseToSqlConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("USAGE: DumpDatabaseToSqlConsole.exe <databasename>\r\nExample: DumpDatabaseToSqlConsole.exe TestDR");
            //    return;
            //}
            string databasename = "TestDR";
            if (args.Length > 0)
            {
                databasename = args[0];
            }

            Dumper d = new Dumper();
            Console.WriteLine("Starting dump...");
            Console.WriteLine("Dumping tables");
            d.DumpTables(databasename);
            Console.WriteLine("Dumping Data");
            d.DumpTableData(databasename);
            Console.WriteLine("Dumping Stored Procedures");
            d.DumpProcedures(databasename);
            Console.WriteLine("Dumping Keys and Contraints");
            d.DumpKeyAndConstraint(databasename);
            Console.WriteLine("Dumping Triggers");
            d.DumpTriggers(databasename);
            Console.WriteLine("Dumping Views");
            d.DumpViews(databasename);
            Console.WriteLine("Done.");
        }
    }
}
