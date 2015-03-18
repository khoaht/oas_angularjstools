using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class CriteriaEngine : GenBase
    {
        private string criteriaTemplate = string.Format("{0}Templates\\criteria.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format("{0}Output\\backend\\criterias\\{1}Criteria.cs", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;

        public CriteriaEngine(string criteriaTemplate, string outputPath, TableData tableData)
            : base(tableData)
        {
            if (!string.IsNullOrEmpty(criteriaTemplate))
                this.criteriaTemplate = criteriaTemplate;



            if (!string.IsNullOrEmpty(outputPath))
                this.outputPath = outputPath;


            strContents = System.IO.File.ReadAllLines(this.criteriaTemplate);


        }

        public override void Generate()
        {
            GenerateInterface();
        }


        private void GenerateInterface()
        {

            var output = new List<string>();
            foreach (var item in strContents)
            {
                //1. Relace domain name
                var newItem = item.Replace("[@DomainName]", DomainName);
                if (!item.Contains("[@Properties]"))
                {
                    output.Add(newItem);

                }
                else if (item.Trim().Contains("[@Properties]"))
                {
                    foreach (var property in Properties)
                    {
                        output.Add(Environment.NewLine);
                        var ctype = ConvertCsharpType(property.Type);
                        var proItem = string.Format("\t\t\tpublic {0}{1} {2} {3}", ctype, ctype.Equals("string") ? string.Empty : "?", property.Name, "{get;set;}");
                        output.Add(proItem);
                    }
                }

            }

            WriteBackendFile(output.ToArray());
        }



        protected override string GetOutPutPath()
        {
            var type = string.Format(@"{0}\Criteria\{1}Criteria.cs", outputPath, DomainName);
            return type;
        }
    }
}
