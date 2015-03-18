using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularAPIGenerate.Lib;

namespace AngularAPIGenerate
{
    public class GenerateEngine
    {
        #region fields
        private string strApiTemplatePath = string.Format(@"{0}\Templates\apiController.txt", AppDomain.CurrentDomain.BaseDirectory);
        private string strApiControllerPath = string.Format(@"{0}\Output\Backend\Controllers", AppDomain.CurrentDomain.BaseDirectory);
        private string strServiceControllerPath = string.Format(@"{0}\Output\Backend\Serviecs", AppDomain.CurrentDomain.BaseDirectory);
        private DatabaseConnection dbOjbect = null;
        private List<TableData> lstTables = null;
        private string outPutMain = @"E:\Google Drive\Projects\TDevs\2015\Logviet2015\Source\V1.0.0\Oas.LV2015\codeGens\{0}";
        private string rootBackend;
        private string rootFrontEnd;
        #endregion

        #region constructors

        public GenerateEngine()
        {
            dbOjbect = new DatabaseConnection();
            lstTables = new List<TableData>();
            lstTables = dbOjbect.GetTables();
        }

        public GenerateEngine(string p1, string p2)
        {
            // TODO: Complete member initialization
            this.rootBackend = string.Format(p1, "{0}\\{1}");
            this.rootFrontEnd = string.Format(p2, "{0}\\{1}");
            dbOjbect = new DatabaseConnection();
            lstTables = new List<TableData>();
            lstTables = dbOjbect.GetTables();
        }

        #endregion

        #region public methods
        public void GenerateApis()
        {
            foreach (var item in lstTables)
            {
                var api = new ApiEngine(string.Empty, string.Empty, item);
                api.Generate();

            }

        }

        public void GenerateCriterias()
        {
            foreach (var item in lstTables)
            {
                var api = new CriteriaEngine(string.Empty, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");
            }

        }

        public void GenerateInstanceServices()
        {
            foreach (var item in lstTables)
            {
                var api = new InstanceServiceEngine(string.Empty, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");
            }

        }

        public void GenerateInterfaceServices()
        {
            foreach (var item in lstTables)
            {
                var api = new InterfaceServiceEngine(string.Empty, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");
            }

        }

        public void GenerateListViewHTML()
        {
            foreach (var item in lstTables)
            {
                var api = new ListViewEngine(string.Empty, outPutMain, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");

            }

        }

        public void GenerateListController()
        {
            foreach (var item in lstTables)
            {
                var api = new ListControllerEngine(string.Empty, outPutMain, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");

            }

        }

        public void GenerateAngularJSService()
        {
            foreach (var item in lstTables)
            {
                var api = new AngularJSServiceEngine(string.Empty, outPutMain, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");

            }

        }
        #endregion

        public void GenItemView()
        {
            foreach (var item in lstTables)
            {
                var api = new ItemViewEngine(string.Empty, outPutMain, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");

            }
        }

        internal void GenItemController()
        {
            foreach (var item in lstTables)
            {
                var api = new ItemControllerEngine(string.Empty, outPutMain, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");

            }
        }

        internal void GenItemFilter()
        {
            foreach (var item in lstTables)
            {
                var api = new NameFilterEngine(string.Empty, outPutMain, string.Empty, item);
                api.Generate();
                Console.WriteLine("Generated Criterias success!");

            }
        }


        internal void GenerateAllClientFrontEnd(string objName, string rootFolder, string pModule)
        {

            var item = lstTables.SingleOrDefault(t => t.Name.Equals(objName));
            if (item != null)
            {
                var module = pModule;//string.Format("{0}\\{1}", pModule, item.Name.ToLower());
                var lstHtml = new ListViewEngine(string.Empty, rootFolder, module, item);
                lstHtml.Generate();
                var lstController = new ListControllerEngine(string.Empty, rootFolder, module, item);
                lstController.Generate();
                var service = new AngularJSServiceEngine(string.Empty, rootFolder, module, item);
                service.Generate();
                var itemView = new ItemViewEngine(string.Empty, rootFolder, module, item);
                itemView.Generate();
                var itemController = new ItemControllerEngine(string.Empty, rootFolder, module, item);
                itemController.Generate();
                var filter = new NameFilterEngine(string.Empty, rootFolder, module, item);
                filter.Generate();
            }
        }

        internal void GenerateAllBackend(string objName, string rootFolder, string module)
        {
             var item = lstTables.SingleOrDefault(t => t.Name.Equals(objName));
             if (item != null)
             {
                 var iservice = new InterfaceServiceEngine(string.Empty, rootFolder, item);
                 iservice.Generate();
                 var service = new InstanceServiceEngine(string.Empty, rootFolder, item);
                 service.Generate();
                 var criteria = new CriteriaEngine(string.Empty, rootFolder, item);
                 criteria.Generate();
                 var apiController = new ApiEngine(string.Empty, rootFolder, item);
                 apiController.Generate();

             }
        }
    }
}
