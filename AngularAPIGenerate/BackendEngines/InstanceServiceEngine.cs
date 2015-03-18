using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class InstanceServiceEngine : GenBase
    {
        private string serviceInstanceTemplate = string.Format("{0}Templates\\serviceInstance.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format("{0}Output\\backend\\services\\{1}Service.cs", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;

        public InstanceServiceEngine(string serviceInstanceTemplate, string outputPath, TableData tableData)
            : base(tableData)
        {
            if (!string.IsNullOrEmpty(serviceInstanceTemplate))
                this.serviceInstanceTemplate = serviceInstanceTemplate;


            if (!string.IsNullOrEmpty(outputPath))
                this.outputPath = outputPath;


            strContents = System.IO.File.ReadAllLines(this.serviceInstanceTemplate);
        }

        public override void Generate()
        {
            var output = new List<string>();

            foreach (var item in strContents)
            {
                if (!(item.Contains("[@InClude]") || item.Contains("[@Condition]") || item.Contains("[@Sorting]")))
                {
                    //1. Relace domain name
                    var newItem = item.Replace("[@DomainName]", DomainName);
                    //2. Replece service name
                    newItem = newItem.Replace("[@ObjectName]", ObjectName);
                    newItem = newItem.Replace("[@TableName]", TableName);

                    output.Add(newItem);
                }
                else if (item.Contains("[@Condition]"))
                {
                    var where = new StringBuilder();
                    where.Append("\t\t\t\t\t\t.Where(t=>");
                    bool first = true;
                    foreach (var property in Properties)
                    {
                        if (!property.Name.Contains("_") && !property.Name.Equals("Discriminator"))
                        {
                            var nullCond = string.Empty;
                            var notNullCond = string.Empty;
                            var ctype = ConvertCsharpType(property.Type);
                            switch (ctype)
                            {
                                case "string":
                                    nullCond = string.Format("(string.IsNullOrEmpty(criteria.{0})", property.Name);
                                    notNullCond = string.Format("||( t.{0}.Contains(criteria.{0}) || criteria.{0}.Contains(t.{0}) ))", property.Name);
                                    break;
                                case "DateTime":
                                case "DateTime?":
                                    nullCond = string.Format("(criteria.{0}==null ", property.Name);
                                    notNullCond = string.Format("|| t.{0}{1}.CompareTo(criteria.{0}) == 0  )", property.Name, property.IsNullable ? ".Value" : string.Empty);
                                    break;
                                case "Guid":
                                    nullCond = string.Format("(criteria.{0}==null || criteria.{0} == Guid.Empty ", property.Name);
                                    notNullCond = string.Format("|| t.{0}{1}.Equals(criteria.{0}) )", property.Name, property.IsNullable ? ".Value" : string.Empty);
                                    break;
                                case "int":
                                case "decimal":
                                case "float":
                                case "float?":
                                case "long":
                                    nullCond = string.Format("(criteria.{0}==null ", property.Name);
                                    notNullCond = string.Format("|| t.{0}{1}.Equals(criteria.{0}) )", property.Name, property.IsNullable ? ".Value" : string.Empty);
                                    break;
                                default:
                                    nullCond = string.Format("(criteria.{0}==null ", property.Name);
                                    notNullCond = string.Format("|| t.{0}.Equals(criteria.{0}) )", property.Name);
                                    break;
                            }
                            if (first)
                            {
                                where.Append(nullCond);
                                where.Append(notNullCond);
                                first = false;
                            }
                            else
                            {
                                where.Append(string.Format("\t\t\t\t\t\t\t&&{0}", nullCond));
                                where.Append(notNullCond);
                            }

                            where.Append(Environment.NewLine);
                        }
                    }
                    where.Append("\t\t\t\t\t\t)");
                    output.Add(where.ToString());
                }
                else if (item.Contains("[@Sorting]"))
                {
                    var sorting = new StringBuilder();
                    sorting.Append("\t\t\tswitch (criteria.SortColumn){");
                    sorting.Append(Environment.NewLine);
                    foreach (var property in Properties)
                    {
                        if (!property.Name.Contains("_") && !property.Name.Equals("Discriminator"))
                        {
                            var nullCond = string.Empty;
                            var notNullCond = string.Empty;
                            var ctype = ConvertCsharpType(property.Type);
                            switch (ctype)
                            {
                                case "string":
                                case "DateTime":
                                    sorting.Append(string.Format("\t\t\t\tcase \"{0}\" :", property.Name.ToLower()));
                                    sorting.Append(Environment.NewLine);
                                    sorting.AppendFormat("\t\t\t\t\tquery = isAsc ? query.OrderBy(t => t.{0}) : query.OrderByDescending(t => t.{0});", property.Name);
                                    sorting.Append(Environment.NewLine);
                                    sorting.Append("\t\t\t\t\tbreak;");
                                    sorting.Append(Environment.NewLine);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    sorting.Append("\t\t\t\tdefault: break;}");
                    output.Add(sorting.ToString());
                }

                else if (item.Contains("[@InClude]"))
                {
                    var includes = new StringBuilder();

                    // 1. All Parents
                    foreach (var parent in TableData.ParentTables)
                    {
                        includes.AppendFormat("\t\t\t\t\t\t.Include(t=>t.{0})", GetDomainName(parent.Name));
                        includes.Append(Environment.NewLine);
                    }

                    //2. All children
                    foreach (var child in TableData.ChildrenTables)
                    {
                        includes.AppendFormat("\t\t\t\t\t\t.Include(t=>t.{0})", child.Name);
                        includes.Append(Environment.NewLine);
                    }

                    output.Add(includes.ToString());
                }

            }
            WriteBackendFile(output.ToArray());
        }

        protected override string GetOutPutPath()
        {
            var type = string.Format(@"{0}\Services\{1}Service.cs", outputPath, DomainName);
            return type;
        }

    }
}
