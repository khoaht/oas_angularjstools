using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class ListViewEngine : GenBase
    {
        private string criteriaTemplate = string.Format(@"{0}Templates\frontend\listView.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format(@"{0}Output\FrontEnd\{1}", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;
        private List<string> strContentsOutput = new List<string>();
        private string module = string.Empty;
        public ListViewEngine(string criteriaTemplate, string outputPath, string module, TableData tableData)
            : base(tableData)
        {
            if (!string.IsNullOrEmpty(criteriaTemplate))
                this.criteriaTemplate = criteriaTemplate;



            if (!string.IsNullOrEmpty(outputPath))
                this.outputPath = outputPath;
            if (!string.IsNullOrEmpty(module))
                this.module = module;


            strContents = System.IO.File.ReadAllLines(this.criteriaTemplate);


        }


        public override void Generate()
        {
            GenerateDynamicFields();
            WriteFrontendFile(strContentsOutput.ToArray());
        }

        private void GenerateDynamicFields()
        {
            //1. Table header            
            foreach (var item in strContents)
            {
                //1. Relace domain name
                var strReplace = item;
                if (item.Contains("[@DomainName]"))
                {
                    strReplace = strReplace.Replace("[@DomainName]", DomainName);

                }
                if (item.Contains("[@ObjectName]"))
                {
                    strReplace = strReplace.Replace("[@ObjectName]", ObjectName);
                }
                if (item.Contains("[@TableName]"))
                {
                    strReplace = strReplace.Replace("[@TableName]", TableName);

                }
                if (item.Contains("[@TableNameLower]"))
                {
                    strReplace = strReplace.Replace("[@TableNameLower]", TableNameLower);
                }

                strContentsOutput.Add(strReplace);

                //Add Table  headers
                if (item.Contains("[@TableHeaders]"))
                {
                    var headers = new StringBuilder();
                    foreach (var property in Properties)
                    {
                        if (!property.Name.Contains("Id"))
                        {
                            headers.AppendFormat("\t\t\t\t\t\t\t\t\t<th data-ng-click=\"vm.setOrder('{0}')\">{0}</th>", property.Name);
                            headers.Append(Environment.NewLine);
                        }
                        else if (property.Name.Contains("Id") && property.Name.Length > 2)
                        {
                            var parentName = property.Name.Substring(0, property.Name.Length - 2);
                            var parent = TableData.ParentTables.Where(t => t.Name.Contains(parentName)).ToList();
                            if (parent != null && parent.Count > 0)
                            {
                                var pStrPros = parent[0].Columns.Where(t => ConvertCsharpType(t.Type) == "string").ToList();
                                foreach (var pPro in pStrPros)
                                {
                                    headers.AppendFormat("\t\t\t\t\t\t\t\t\t<th data-ng-click=\"vm.setOrder('{0}')\">{0}.{1}</th>", parentName, pPro.Name);
                                    headers.Append(Environment.NewLine);
                                }
                            }
                        }
                    }
                    strContentsOutput.Add(headers.ToString());
                }//Add Table  body
                else if (item.Contains("[@TableBodies]"))
                {
                    var bodies = new StringBuilder();
                    foreach (var property in Properties)
                    {
                        if (!property.Name.Contains("Id"))
                        {
                            bodies.AppendFormat("\t\t\t\t\t\t\t\t\t<td>{0}{1}.{2}{3}</td>", "{{", ObjectName, property.Name, "}}");
                            bodies.Append(Environment.NewLine);
                        }
                        else if (property.Name.Contains("Id") && property.Name.Length > 2)
                        {
                            var parentName = property.Name.Substring(0, property.Name.Length - 2);
                            var parent = TableData.ParentTables.Where(t => t.Name.Contains(parentName)).ToList();
                            if (parent != null && parent.Count > 0)
                            {
                                var pStrPros = parent[0].Columns.Where(t => ConvertCsharpType(t.Type) == "string").ToList();
                                foreach (var pPro in pStrPros)
                                {
                                    bodies.AppendFormat("\t\t\t\t\t\t\t\t\t<td>{0}{1}.{2}.{3}{4}</td>", "{{", ObjectName, parentName, pPro.Name, "}}");
                                    bodies.Append(Environment.NewLine);
                                }
                            }
                        }
                    }

                    bodies.Append(Environment.NewLine);
                    strContentsOutput.Add(bodies.ToString());
                }

            }
        }

        protected override string GetOutPutPath()
        {
            var theModule = string.IsNullOrEmpty(module) ? string.Empty : string.Format("\\{0}", module);
            var type = string.Format("{0}\\Components{1}\\{2}\\{2}.html", outputPath, theModule, TableNameLower);
            return type;
        }
    }
}
