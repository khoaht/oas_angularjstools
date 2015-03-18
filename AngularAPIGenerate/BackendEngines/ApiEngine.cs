using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class ApiEngine : GenBase
    {
        private string template = string.Format("{0}Templates\\apiController.txt", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string outputPath = string.Format("{0}Output\\backend\\controllers\\{1}Controller.cs", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;

        public ApiEngine(string template, string outputPath, TableData tableData)
            : base(tableData)
        {
            if (!string.IsNullOrEmpty(template))
                this.template = template;
            if (!string.IsNullOrEmpty(outputPath))
                this.outputPath = outputPath;
            strContents = System.IO.File.ReadAllLines(this.template);

            
        }

        public override void Generate()
        {
            var output = new string[strContents.Length];
            int count = 0;
            foreach (var item in strContents)
            {
                //1. Relace domain name
                var newItem = item.Replace("[@DomainName]", DomainName);
                //2. Replece service name
                newItem = newItem.Replace("[@ObjectName]", ServiceName);
                newItem = newItem.Replace("[@TableName]", TableName);

                output[count++] = newItem;
            }
            WriteBackendFile(output);
        }

        
        protected override string GetOutPutPath()
        {
            outputPath = @"E:\Google Drive\Projects\TDevs\2015\Logviet2015\Source\V1.0.0\Oas.LV2015\Controllers";
            var type = string.Format(@"{0}\{1}Controller.cs", outputPath, DomainName);
            return type;

        }
    }
}
