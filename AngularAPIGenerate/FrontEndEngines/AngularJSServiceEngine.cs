using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class AngularJSServiceEngine : GenBase
    {
        private string criteriaTemplate = string.Format(@"{0}Templates\frontend\service.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format(@"{0}Output\FrontEnd\{1}", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;
        private List<string> strContentsOutput = new List<string>();
        private string module = string.Empty;

        public AngularJSServiceEngine(string criteriaTemplate, string outputPath, string module, TableData tableData)
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
            WriteFrontendFile(strContents.ToArray());
        }

        private void GenerateDynamicFields()
        {


            for (int i = 0; i < strContents.Length; i++)
            {
                strContents[i] = strContents[i].Replace("[@DomainName]", DomainName);
                strContents[i] = strContents[i].Replace("[@ObjectName]", ObjectName);
                strContents[i] = strContents[i].Replace("[@TableNameLower]", TableNameLower);
                strContents[i] = strContents[i].Replace("[@TableName]", TableName);
            }
        }



        protected override string GetOutPutPath()
        {
            var theModule = string.IsNullOrEmpty(module) ? string.Empty : string.Format("\\{0}", module);
            var type = string.Format("{0}\\Services{1}\\{2}Service.js", outputPath, theModule, TableNameLower);
            return type;
        }
    }
}
