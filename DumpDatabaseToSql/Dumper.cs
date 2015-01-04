using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace DumpDatabaseToSql
{
    public class Dumper
    {
        private string databaseName = "unset";
        private Server srv;

        public Dumper()
        {
            srv = new Server();
        }

        private void RecreateDirectory(string dirpart)
        {
            string currentdir = Environment.CurrentDirectory;
            if (Directory.Exists(dirpart)) Directory.Delete(dirpart, true);
            Directory.CreateDirectory(dirpart);
        }

        public bool DumpTableData(string name)
        {
            string dirpart = @"DefaultData\";

            this.databaseName = name;
            if (string.IsNullOrEmpty(name) || !srv.Databases.Contains(name))
            {
                throw new Exception(string.Format("Database ({0}) does not exist", name));
            }
            Database db = srv.Databases[name];
            Scripter scripter = new Scripter(srv);
            scripter.Options.ScriptData = true;
            scripter.Options.ScriptSchema = false;
            scripter.Options.ScriptDrops = false;
            RecreateDirectory(dirpart);
            foreach (Table t in db.Tables)
            {
//                if (t.Name.StartsWith("tpl_")) continue;
                string filename = string.Format("{0}dbo.{1}.sql", dirpart, t.Name);
                if (!t.IsSystemObject)
                {
                    //StringCollection sc = scripter.Script(new Urn[] { t.Urn });
                    //if (sc.Count == 0) continue;
                    TextWriter writer = new StreamWriter(filename);
//                    StringBuilder sb = new StringBuilder();
                    foreach (string s in scripter.EnumScript(new Urn[] {t.Urn}))
                    {
                        writer.WriteLine(s);
                        writer.WriteLine();
//                        sb.Append(s).AppendLine();
                    }
                    writer.Flush();
                    writer.Close();
//                    if (sb.Length > 0) File.AppendAllText(filename, sb + "\r\n");
                }
            }
            return true;
        }


        public bool DumpTables(string name)
        {
            string dirpart = @"Tables\";

            this.databaseName = name;
            if (string.IsNullOrEmpty(name) || !srv.Databases.Contains(name))
            {
                throw new Exception(string.Format("Database ({0}) does not exist", name));
            }
            Database db = srv.Databases[name];
            Scripter scripter = new Scripter(srv);
            scripter.Options.Indexes = true;
            scripter.Options.IncludeIfNotExists = true;
            RecreateDirectory(dirpart);
            foreach (Table t in db.Tables)
            {
                string filename = string.Format("{0}dbo.{1}.sql", dirpart, t.Name);
                if (!t.IsSystemObject)
                {
                    
                    StringCollection sc = scripter.Script(new Urn[] {t.Urn});
                    if (sc.Count == 0) continue;
                    File.AppendAllText(filename, string.Format(" --- scripting for table {0}\r\n", t.Name));
                    foreach (string s in sc)
                    {
                        File.AppendAllText(filename,s+"\r\n");
                    }
                }
            }
            return true;
        }

        public bool DumpKeyAndConstraint(string name)
        {
            string dirpart = @"KeyAndConstraint\";


            this.databaseName = name;
            if (string.IsNullOrEmpty(name) || !srv.Databases.Contains(name))
            {
                throw new Exception(string.Format("Database ({0}) does not exist", name));
            }
            Database db = srv.Databases[name];
            Scripter scripter = new Scripter(srv);
            scripter.Options.PrimaryObject = false;
            scripter.Options.IncludeIfNotExists = true;
            scripter.Options.DriForeignKeys = true;
            RecreateDirectory(dirpart);
            foreach (Table t in db.Tables)
            {
                string filename = string.Format("{0}dbo.{1}.sql", dirpart, t.Name);
                if (!t.IsSystemObject)
                {
                    StringCollection sc = scripter.Script(new Urn[] { t.Urn });
                    if (sc.Count == 0) continue;
                    File.AppendAllText(filename, string.Format(" --- scripting for table {0}\r\n", t.Name));
                    foreach (string s in sc)
                    {
                        File.AppendAllText(filename, s + "\r\n");
                    }
                }
            }
            return true;
        }

        public bool DumpViews(string name)
        {
            string dirpart = @"Views\";

            this.databaseName = name;
            if (string.IsNullOrEmpty(name) || !srv.Databases.Contains(name))
            {
                throw new Exception(string.Format("Database ({0}) does not exist", name));
            }
            Database db = srv.Databases[name];
            Scripter scripter = new Scripter(srv);
            scripter.Options.IncludeIfNotExists = true;

            RecreateDirectory(dirpart);

            foreach (View v in db.Views)
            {
                string filename = string.Format("{0}dbo.{1}.sql", dirpart, v.Name);
                if (!v.IsSystemObject)
                {
                    File.AppendAllText(filename, string.Format(" --- scripting for table {0}\r\n", v.Name));
                    StringCollection sc = scripter.Script(new Urn[] { v.Urn });
                    foreach (string s in sc)
                    {
                        File.AppendAllText(filename, s + "\r\n");
                    }
                }
            }

            return true;
        }

        public bool DumpProcedures(string name)
        {
            string dirpart = @"Procedures\";

            this.databaseName = name;
            if (string.IsNullOrEmpty(name) || !srv.Databases.Contains(name))
            {
                throw new Exception(string.Format("Database ({0}) does not exist", name));
            }
            Database db = srv.Databases[name];

            RecreateDirectory(dirpart);

            foreach (StoredProcedure s in db.StoredProcedures)
            {
                string filename = string.Format("{0}dbo.{1}.sql", dirpart, s.Name);
                if (!s.IsSystemObject)
                {
                    Scripter scripter = new Scripter(srv);
                    scripter.Options.ScriptDrops = true;
                    scripter.Options.IncludeIfNotExists = true;

                    File.AppendAllText(filename, string.Format(" --- scripting for table {0}\r\n", s.Name));
                    StringCollection sc = scripter.Script(new Urn[] { s.Urn });
                    foreach (string str in sc)
                    {
                        File.AppendAllText(filename, str + "\r\n");
                    }

                    scripter = new Scripter(srv);
                    sc = scripter.Script(new Urn[] { s.Urn });
                    foreach (string str in sc)
                    {
                        File.AppendAllText(filename, str + "\r\n");
                    }

                }
            }



            return true;
        }

        public bool DumpTriggers(string name)
        {
            string dirpart = @"Triggers\";

            this.databaseName = name;
            if (string.IsNullOrEmpty(name) || !srv.Databases.Contains(name))
            {
                throw new Exception(string.Format("Database ({0}) does not exist", name));
            }
            Database db = srv.Databases[name];
            Scripter scripter = new Scripter(srv);
            scripter.Options.Triggers = true;
            scripter.Options.IncludeIfNotExists = true;

            RecreateDirectory(dirpart);

            foreach (Table t in db.Tables)
            {
                if (t.Triggers.Count == 0) continue;
                string filename = string.Format("{0}dbo.{1}.sql",dirpart, t.Name);
                if (!t.IsSystemObject)
                {
                    StringCollection sc = scripter.Script(new Urn[] { t.Urn });
                    if (sc.Count == 0) continue;
                    File.AppendAllText(filename, string.Format(" --- scripting for table {0}\r\n", t.Name));
                    foreach (string s in sc)
                    {
                        File.AppendAllText(filename, s + "\r\n");
                    }
                }
            }
            return true;
        }
    }
}
