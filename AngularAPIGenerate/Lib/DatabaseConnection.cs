using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularAPIGenerate.Lib
{
    public class DatabaseConnection
    {
        private string connectionString = @"Data Source=SMK\SQLEXPRESS;Initial Catalog=Logviet2015;Integrated Security=False;User ID=sa;Password=abcde12345-;";
        private string DbName = "Logviet2015";
        private string[] inorgeList = { "__MigrationHistory", "sysdiagramsy", "sysdiagram", "AspNetUserClaims", "AspNetUserLogins" };

        public DatabaseConnection()
        {
        }
        public List<TableData> GetTables()
        {
            var result = new List<TableData>();
            using (var connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from sys.tables", connect);

                connect.Open();
                var reader = cmd.ExecuteReader();
                // print the CategoryName of each record
                while (reader.Read())
                {
                    var tableName = (string)reader["name"];
                    if (!(inorgeList.Any(t => t.ToLower().Contains(tableName.ToLower()))))
                    {
                        var tbl = new TableData()
                        {
                            Name = tableName,
                            Columns = GetColumns(tableName),
                            ChildrenTables = GetChidrenTables(tableName),
                            ParentTables = GetParentTables(tableName)
                        };
                        result.Add(tbl);
                    }
                }
                cmd.Dispose();

            }
            return result;
        }

        private List<TableData> GetChidrenTables(string parentName)
        {
            var result = new List<TableData>();
            using (var connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(string.Format(Constants.SQL_CHILDREN, parentName), connect);

                connect.Open();
                var reader = cmd.ExecuteReader();
                // print the CategoryName of each record
                while (reader.Read())
                {

                    var tableName = reader["Dependant"] == DBNull.Value ? string.Empty : (string)reader["Dependant"];
                    if (!string.IsNullOrEmpty(tableName) && !(inorgeList.Any(t => t.ToLower().Contains(tableName.ToLower()))))
                    {
                        var tbl = new TableData()
                        {
                            Name = tableName,
                            Columns = GetColumns(tableName)
                        };
                        result.Add(tbl);
                    }
                }
                reader.Close();
            }
            return result;
        }

        private List<TableData> GetParentTables(string chilName)
        {
            var result = new List<TableData>();
            using (var connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(string.Format(Constants.SQL_PARENT, chilName), connect);

                connect.Open();
                var reader = cmd.ExecuteReader();
                // print the CategoryName of each record
                while (reader.Read())
                {
                    var tableName = reader["Parent"] == DBNull.Value ? string.Empty : (string)reader["Parent"];
                    if (!string.IsNullOrEmpty(tableName) && !(inorgeList.Any(t => t.ToLower().Contains(tableName.ToLower()))))
                    {
                        var tbl = new TableData()
                        {
                            Name = tableName,
                            Columns = GetColumns(tableName)
                        };
                        result.Add(tbl);
                    }
                }
                reader.Close();
            }
            return result;

        }

        public List<ColumnData> GetColumns(string tableName)
        {
            var result = new List<ColumnData>();
            using (var connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(string.Format("select * from information_schema.columns where table_name ='{0}' and TABLE_CATALOG='{1}'", tableName, DbName), connect);

                connect.Open();
                var reader = cmd.ExecuteReader();
                // print the CategoryName of each record
                while (reader.Read())
                {
                    var columnName = (string)reader["COLUMN_NAME"];
                    var columnType = (string)reader["DATA_TYPE"];
                    var columnNullable = (string)reader["IS_NULLABLE"];
                    var columObj = new ColumnData()
                    {
                        Name = columnName,
                        Type = columnType,
                        IsNullable = columnNullable == "YES"
                    };
                    result.Add(columObj);

                }
                reader.Close();
            }
            return result;
        }
    }
}
