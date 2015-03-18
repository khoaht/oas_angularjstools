using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public abstract class GenBase
    {
        #region fields
        private string tableName = string.Empty;
        private TableData tableData = null;
        //[@Condition]
        #endregion

        #region constructor
        public GenBase(TableData tableData)
        {
            this.tableName = tableData.Name;
            this.tableData = tableData;
        }
        #endregion

        #region Properties

        public TableData TableData { get { return tableData; } }
        public string DomainName
        {
            get
            {
                return FormatName(tableName);
            }
        }

        public string TableName { get { return tableName; } }
        public string TableNameLower
        {
            get
            {
                var fc = DomainName[0].ToString().ToLower();
                return string.Format("{0}{1}", fc, TableName.Substring(1));
            }
        }
        public string ServiceName { get { return FormatServiceName(tableName); } }

        public string ObjectName
        {
            get
            {
                var fc = DomainName[0].ToString().ToLower();
                return string.Format("{0}{1}", fc, DomainName.Substring(1));
            }
        }


        public List<ColumnData> Properties
        {
            get
            {
                return tableData.Columns;
            }
        }

        #endregion

        #region methods
        protected abstract string GetOutPutPath();
        protected virtual string GetDefaultSortName() { return "name"; }
        public abstract void Generate();

        public string GetDomainName(string tableName)
        {
            return FormatName(tableName);
        }

        public string FormatTableNameAsParent(string tableName)
        {

            var fc = tableName[0].ToString().ToLower();
            return string.Format("{0}{1}", fc, tableName.Substring(1));
        }

        protected string ConvertCsharpType(string p)
        {
            var typeName = "string";
            switch (p)
            {
                case "uniqueidentifier":
                    typeName = "Guid";
                    break;
                case "int":
                    typeName = "int";
                    break;
                case "tyniint":
                    typeName = "short";
                    break;
                case "float":
                case "real":
                    typeName = "float";
                    break;
                case "bigint":
                    typeName = "long";
                    break;
                case "decimal":
                    typeName = "decimal";
                    break;
                case "datetime":
                    typeName = "DateTime";
                    break;
                case "bit":
                    typeName = "bool";
                    break;
                default:
                    break;
            }

            return typeName.Trim();
        }

        private string FormatName(string tableName)
        {
            string name = tableName;
            string exp1 = tableName.Substring(tableName.Length - 1).ToLower();
            string exp2 = tableName.Substring(tableName.Length - 2).ToLower();
            string exp3 = tableName.Substring(tableName.Length - 3).ToLower();
            if (tableName.Length > 3)
            {
                if (exp2.Equals("ss"))
                    name = tableName;
                else if (exp2.Equals("es"))
                {
                    if (tableName.Contains("Classes") || tableName.Contains("Businesses"))
                        name = tableName.Substring(0, tableName.Length - 2);
                    else name = tableName.Substring(0, tableName.Length - 1);
                }
                else if (exp1.Equals("s"))
                    name = tableName.Substring(0, tableName.Length - 1);

                if (exp3.Equals("ies"))
                    name = tableName.Substring(0, tableName.Length - 3) + "y";

            }

            return name;
        }


        private string FormatServiceName(string tableName)
        {
            string name = FormatName(tableName);
            name = name.ToLower();
            return name;
        }


        protected void WriteBackendFile(string[] output)
        {
            try
            {

                var fileName = string.Format(GetOutPutPath(), DomainName);
                System.IO.File.WriteAllLines(fileName, output);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, false))
                {
                    foreach (string line in output)
                    {
                        file.WriteLine(line);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void WriteFrontendFile(string[] output)
        {
            try
            {


                var pathString = string.Format(GetOutPutPath());

                //if (!System.IO.Directory.Exists(pathString))
                //    System.IO.Directory.CreateDirectory(pathString);


                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathString,false))
                {
                    foreach (string line in output)
                    {
                        file.WriteLine(line);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}
