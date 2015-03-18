using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AngularAPIGenerate
{
    public partial class Form1 : Form
    {
        private GenerateEngine engine = null;
        public Form1()
        {            
            InitializeComponent();
            engine = new GenerateEngine(txtRootBackEnd.Text.Trim(), txtRootFrontEnd.Text.Trim());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenerateApis();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenerateCriterias();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenerateInterfaceServices();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenerateInstanceServices();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //try
            {
                engine.GenerateListViewHTML();
                MessageBox.Show("Complete!");
            }
            //catch (Exception exception)
            {
                //MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenerateListController();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenerateAngularJSService();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //try
            {
                engine.GenItemView();
                MessageBox.Show("Complete!");
            }
            //catch (Exception exception)
            {
                // MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
           // try
            {
                engine.GenItemController();
                MessageBox.Show("Complete!");
            }
            //catch (Exception exception)
            {
                //MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                engine.GenItemFilter();
                MessageBox.Show("Complete!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnFrontEndRoot_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                txtRootBackEnd.Text = folderDialog.SelectedPath;

            }

        }

        private void btnChooseBackendRoot_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = txtRootFrontEnd.Text;
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                txtRootFrontEnd.Text = folderDialog.SelectedPath;

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRengAll_Click(object sender, EventArgs e)
        {
            var objName = txtTableName.Text.Trim();
            var rootFolder = txtRootFrontEnd.Text.Trim();

            var module = txtModule.Text.Trim();

            if (string.IsNullOrEmpty(rootFolder))
            {
                MessageBox.Show("Nhap root folder can gen");

                return;
            }
            if (!string.IsNullOrEmpty(objName))
            {
                engine.GenerateAllClientFrontEnd(objName, rootFolder,module);
                MessageBox.Show("GenClient Front end ok!");
            }
            else
            {
                MessageBox.Show("Nhap Domain can gen");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var objName = txtTableName.Text.Trim();
            var rootFolder = txtRootBackEnd.Text.Trim();

            var module = txtModule.Text.Trim();

            if (string.IsNullOrEmpty(rootFolder))
            {
                MessageBox.Show("Nhap root folder can gen");

                return;
            }
            if (!string.IsNullOrEmpty(objName))
            {
                engine.GenerateAllBackend(objName, rootFolder, module);
                MessageBox.Show("Gen Back end ok!");
            }
            else
            {
                MessageBox.Show("Nhap Domain can gen");
            }
        }
    }
}
