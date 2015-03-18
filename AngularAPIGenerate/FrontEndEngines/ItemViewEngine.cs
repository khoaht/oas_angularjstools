using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class ItemViewEngine : GenBase
    {
        private string criteriaTemplate = string.Format(@"{0}Templates\frontend\itemView.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string controlTemplate = string.Format(@"{0}Templates\frontend\controls", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format(@"{0}Output\FrontEnd\{1}", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] controls = { "stringControl.txt", "numberControl.txt", "boolControl.txt", "dateTimeControl.txt", "emailControl.txt" };
        private string[] strContents = null;
        private List<string> strContentsOutput = new List<string>();
        private Dictionary<string, string[]> dicControls = new Dictionary<string, string[]>();
        private string p;
        private string rootFolder;
        private string module;
        private Lib.TableData item;

        public ItemViewEngine(string criteriaTemplate, string outputPath, string module, TableData tableData)
            : base(tableData)
        {
            if (!string.IsNullOrEmpty(criteriaTemplate))
                this.criteriaTemplate = criteriaTemplate;



            if (!string.IsNullOrEmpty(outputPath))
                this.outputPath = outputPath;

            if (!string.IsNullOrEmpty(module))
                this.module = module;
            

            // load main template
            strContents = System.IO.File.ReadAllLines(this.criteriaTemplate);

            // load control tempates
            dicControls.Add("string", System.IO.File.ReadAllLines(string.Format(@"{0}\{1}", this.controlTemplate, "stringControl.txt")));
            dicControls.Add("number", System.IO.File.ReadAllLines(string.Format(@"{0}\{1}", this.controlTemplate, "numberControl.txt")));
            dicControls.Add("email", System.IO.File.ReadAllLines(string.Format(@"{0}\{1}", this.controlTemplate, "emailControl.txt")));
            dicControls.Add("DateTime", System.IO.File.ReadAllLines(string.Format(@"{0}\{1}", this.controlTemplate, "dateTimeControl.txt")));
            dicControls.Add("bool", System.IO.File.ReadAllLines(string.Format(@"{0}\{1}", this.controlTemplate, "boolControl.txt")));
            dicControls.Add("parent", System.IO.File.ReadAllLines(string.Format(@"{0}\{1}", this.controlTemplate, "parentControl.txt")));
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
                if (item.Contains("[@FormControls]"))
                {
                    var headers = new StringBuilder();
                    foreach (var property in Properties)
                    {
                        var pType = ConvertCsharpType(property.Type);
                        string[] pTemplate = null;
                        if (!property.Name.Contains("Id") && pType.IndexOf("Guid") < 0)
                        {

                            switch (pType)
                            {
                                case "string":
                                case "DateTime":
                                case "bool":
                                    pTemplate = dicControls[pType];
                                    break;
                                case "int":
                                case "decimal":
                                case "long":
                                case "float":
                                    pTemplate = dicControls["number"];
                                    break;
                                default:
                                    break;
                            }
                            foreach (var controlItem in pTemplate)
                            {
                                var ctring = String.Format("\t\t\t\t\t" + controlItem, ObjectName, property.Name);
                                headers.Append(ctring);
                                headers.Append(Environment.NewLine);
                            }

                        }
                        if (property.Name.Contains("Id") && property.Name.Length > 2)
                        {
                            pTemplate = dicControls["parent"];
                            var parentName = property.Name.Substring(0, property.Name.Length - 2);
                            var parent = TableData.ParentTables.Where(t => t.Name.Contains(parentName)).FirstOrDefault();
                            if (parent != null)
                            {
                                foreach (var controlItem in pTemplate)
                                {
                                    var ctring = String.Format("\t\t\t\t\t" + controlItem, DomainName, FormatTableNameAsParent(parent.Name),ObjectName, property.Name);
                                    headers.Append(ctring);
                                    headers.Append(Environment.NewLine);
                                }
                            }
                        }
                    }
                    strContentsOutput.Add(headers.ToString());
                }//Add Table  body

            }
        }



        protected override string GetOutPutPath()
        {
            var theModule = string.IsNullOrEmpty(module) ? string.Empty : string.Format("\\{0}", module);
            var type = string.Format("{0}\\Components{1}\\{2}\\{3}Item.html", outputPath, theModule, TableNameLower, ObjectName);
            return type;
        }
    }
}
