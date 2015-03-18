using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class ItemControllerEngine : GenBase
    {
        private string criteriaTemplate = string.Format(@"{0}Templates\frontend\ItemController.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format(@"{0}Output\FrontEnd\{1}", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;
        private List<string> strContentsOutput = new List<string>();
        private string module = string.Empty;

        public ItemControllerEngine(string criteriaTemplate, string outputPath, string module, TableData tableData)
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
                if (item.Contains("[@ParentTables]"))
                {
                    var pObjects = new StringBuilder();
                    if (TableData.ParentTables != null)
                    {
                        foreach (var pName in TableData.ParentTables)
                        {
                            var ctring = String.Format("\t\tvm.{0}=[];", FormatTableNameAsParent(pName.Name));
                            pObjects.Append(ctring);
                            pObjects.Append(Environment.NewLine);
                        }
                    }
                    strContentsOutput.Add(pObjects.ToString());
                }
                //Add Parent Tables
                else if (item.Contains("[@GetParentTables]"))
                {
                    var pObjects = new StringBuilder();
                    if (TableData.ParentTables != null)
                    {
                        foreach (var pName in TableData.ParentTables)
                        {
                            string parten = "\t\tfunction get{2}() {0}\n" +
                                             "\t\t\t\t{1}Service.get{2}().then(function (data) {0}\n" +
                            "\t\t\t\tvm.{1} = data;\n" +
                            "\t\t\t\t{3}, processError);\n" +
                            "\t\t{3}\n";

                            var ctring = String.Format(parten, "{", FormatTableNameAsParent(pName.Name), pName.Name, "}");

                            pObjects.Append(ctring);
                            pObjects.Append(Environment.NewLine);
                        }
                    }
                    strContentsOutput.Add(pObjects.ToString());
                }
                else if (item.Contains("[@CallGetParentTables]"))
                {
                    var pObjects = new StringBuilder();
                    if (TableData.ParentTables != null)
                    {
                        foreach (var pName in TableData.ParentTables)
                        {
                            string pattern = "\t\tget{0}();";

                            var ctring = String.Format(pattern, pName.Name);

                            pObjects.Append(ctring);
                            pObjects.Append(Environment.NewLine);
                        }
                    }
                    strContentsOutput.Add(pObjects.ToString());
                }

                //Add Parent Tables
                else if (item.Contains("[@DependenciesService1]"))
                {
                    var rService = new StringBuilder();
                    if (TableData.ParentTables != null)
                    {
                        foreach (var pName in TableData.ParentTables)
                        {
                            string pattern = "\t\t\t\t\t\t,'{0}Service'";
                            var ctring = String.Format(pattern, FormatTableNameAsParent(pName.Name));
                            rService.Append(Environment.NewLine);
                            rService.Append(ctring);
                        }
                    }
                    strContentsOutput.Add(rService.ToString());
                }
                else if (item.Contains("[@DependenciesService2]"))
                {
                    var iService = new StringBuilder();
                    if (TableData.ParentTables != null)
                    {
                        foreach (var pName in TableData.ParentTables)
                        {
                            string pattern = "\t\t\t\t\t\t,{0}Service";
                            var ctring = String.Format(pattern, FormatTableNameAsParent(pName.Name));
                            iService.Append(Environment.NewLine);
                            iService.Append(ctring);
                        }
                    }
                    strContentsOutput.Add(iService.ToString());
                }



            }//Add Table  body

        }

        protected override string GetOutPutPath()
        {
            var theModule = string.IsNullOrEmpty(module) ? string.Empty : string.Format("\\{0}", module);
            var type = string.Format("{0}\\components{1}\\{2}\\{3}ItemController.js", outputPath, theModule, TableNameLower, ObjectName);
            return type;
        }
    }
}
