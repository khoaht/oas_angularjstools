using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class InterfaceServiceEngine : GenBase
    {
        private string serviceTemplate = string.Format("{0}Templates\\serviceInterface.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string outputPath = string.Format("{0}Output\\backend\\services\\interfaces\\I{1}Service.cs", AppDomain.CurrentDomain.BaseDirectory, "{0}");
        private string[] strContents = null;

        public InterfaceServiceEngine(string serviceTemplate, string outputPath, TableData tableData)
            : base(tableData)
        {
            if (!string.IsNullOrEmpty(serviceTemplate))
                this.serviceTemplate = serviceTemplate;


            if (!string.IsNullOrEmpty(outputPath))
                this.outputPath = outputPath;


            strContents = System.IO.File.ReadAllLines(this.serviceTemplate);
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
            var type = string.Format(@"{0}\Services\Interfaces\I{1}Interface.cs", outputPath, DomainName);
            return type;
        }

    }
}
