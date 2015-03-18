using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class NameFilterEngine : GenBase
    {
        private string criteriaTemplate = string.Format(@"{0}Templates\frontend\itemFilter.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format(@"{0}Output\FrontEnd\{1}", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;
        private List<string> strContentsOutput = new List<string>();
        private string module;

        public NameFilterEngine(string criteriaTemplate, string outputPath, string module, TableData tableData)
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
                if (item.Contains("[@Conditions]"))
                {
                    var headers = new StringBuilder();
                    bool first = true;
                    foreach (var property in Properties)
                    {
                        var pType = ConvertCsharpType(property.Type);

                        if (!property.Name.Contains("Id") && pType.Equals("string"))
                        {
                            headers.AppendFormat("\t\t\t\t{0}(obj.{1}.toLowerCase().indexOf(filterValue) > -1)", first ? string.Empty : "|| ", property.Name);
                            headers.Append(Environment.NewLine);
                            first = false;
                        }

                    }
                    strContentsOutput.Add(headers.ToString());
                }

            }
        }



        protected override string GetOutPutPath()
        {
            var theModule = string.IsNullOrEmpty(module) ? string.Empty : string.Format("\\{0}", module);
            var type = string.Format("{0}\\Shared\\Filters{1}\\{2}Filter.js", outputPath, theModule, TableNameLower);
            return type;
        }
    }
}
