using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularAPIGenerate.Lib
{
    public class TableData
    {
        public string Name { get; set; }

        public List<ColumnData> Columns { get; set; }

        public List<TableData> ChildrenTables { get; set; }
        public List<TableData> ParentTables { get; set; }
    }

    public class ColumnData
    {
        public string Name { get; set; }

        public bool IsNullable{ get; set; }

        public string Type { get; set; }
    }
}
