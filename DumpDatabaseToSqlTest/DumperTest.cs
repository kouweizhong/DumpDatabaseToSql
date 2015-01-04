using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DumpDatabaseToSql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DumpDatabaseToSqlTest
{
    [TestClass]
    public class DumperTest
    {
        private Dumper d;
        private string databasename = "JD_TestDR";

        [TestInitialize]
        public void InitTest()
        {
            d = new Dumper();
        }

        [TestMethod]
        public void RunTest()
        {
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void GetTablesTest()
        {
            Assert.IsTrue(d.DumpTables(databasename));
        }

        [TestMethod]
        public void GetTriggersTest()
        {
            Assert.IsTrue(d.DumpTriggers(databasename));
        }

        [TestMethod]
        public void GetKeyAndConstraintTest()
        {
            Assert.IsTrue(d.DumpKeyAndConstraint(databasename));
        }

        [TestMethod]
        public void GetViewsTest()
        {
            Assert.IsTrue(d.DumpViews(databasename));
        }

        [TestMethod]
        public void GetProceduresTest()
        {
            Assert.IsTrue(d.DumpProcedures(databasename));
        }

        [TestMethod]
        public void GetTableDataTest()
        {
            Assert.IsTrue(d.DumpTableData(databasename));
        }

        [TestMethod]
        public void DumpAll()
        {
            GetTablesTest();
            GetTriggersTest();
            GetKeyAndConstraintTest();
            GetViewsTest();
            GetProceduresTest();
        }
    }
}
