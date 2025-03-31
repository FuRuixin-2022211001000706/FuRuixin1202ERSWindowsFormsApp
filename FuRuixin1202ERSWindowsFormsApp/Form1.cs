using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace FuRuixin1202ERSWindowsFormsApp
{
    public partial class EmployeeRecordForm: System.Windows.Forms.Form
    {
        private TreeNode tvRootNode;

        public EmployeeRecordForm()
        {
            InitializeComponent();
            PopulateTreeView();
            InitalizeListControl();
        }

        private void EmployeeRecordForm_Load(object sender, EventArgs e)
        {

        }
        private void PopulateTreeView()
        {
            statusBarPanel1.Tag = "Refreshing Employee Code: Please wait...";
            this.Cursor = Cursors.WaitCursor;
            treeView1.Nodes.Clear();
            tvRootNode = new TreeNode("Emploee Records");
            this.Cursor = Cursors.Default;
            treeView1.Nodes.Add(tvRootNode);

            TreeNodeCollection nodeCollection = tvRootNode.Nodes;
            XmlTextReader reader = new XmlTextReader("C:\\Users\\asus\\Desktop\\C#code\\FuRuixin1202ERSWindowsFormsApp\\FuRuixin1202ERSWindowsFormsApp\\EmpRec.xml");
            reader.MoveToElement();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasAttributes && reader.NodeType == XmlNodeType.Element)
                    {
                        reader.MoveToElement();
                        reader.MoveToElement();

                        reader.MoveToAttribute("Id");//Id="E001"
                        String strVal = reader.Value;//E001

                        reader.Read();
                        reader.Read();
                        if (reader.Name == "Dept")
                        {
                            reader.Read();
                        }
                        //create a child node
                        TreeNode EcodeNode = new TreeNode(strVal);
                        //add the node
                        nodeCollection.Add(EcodeNode);
                    }
                }
                statusBarPanel1.Text = "Click on an emplyee code to see their record.";
            }
            catch (XmlException ex)
            {
                MessageBox.Show(ex.Message);
            }//end
        }
        protected void InitalizeListControl()
        {
            listView1.Clear();
            listView1.Columns.Add("Employee Name", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("Date of Join", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("Gread", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("Salary", 70, HorizontalAlignment.Left);
        }
        protected void PopulateListView(TreeNode crrNode)
        {
            InitalizeListControl();
            XmlTextReader listRead = new XmlTextReader("C:\\Users\\asus\\Desktop\\C#code\\FuRuixin1202ERSWindowsFormsApp\\FuRuixin1202ERSWindowsFormsApp\\EmpRec.xml");
            listRead.MoveToElement();
            while (listRead.Read())
            {
                String strNodeName;
                String strNodePath;
                String name;
                String gread;
                String doj;
                String sal;
                String[] strItemArr = new string[4];
                listRead.MoveToFirstAttribute();//Id="E001"
                strNodeName = listRead.Value;
                strNodePath = crrNode.FullPath.Remove(0, 17);
                if(strNodePath == strNodeName)
                {
                    ListViewItem lvi;
                    listRead.MoveToNextAttribute();
                    name = listRead.Value;//"Michael Perry"
                    lvi = listView1.Items.Add(listRead.Value);

                    listRead.Read();
                    listRead.Read();

                    listRead.MoveToFirstAttribute();
                    doj = listRead.Value;//"02-02-1999"
                    lvi.SubItems.Add(doj);

                    listRead.MoveToNextAttribute();
                    gread = listRead.Value;// Gread="B"
                    lvi.SubItems.Add(gread);

                    listRead.MoveToNextAttribute();
                    sal = listRead.Value; //Salary = "1750"
                    lvi.SubItems.Add(sal);

                    listRead.MoveToNextAttribute();
                    listRead.MoveToElement();
                    listRead.ReadString();

                }

            }
        }//end PopulatelListView

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currNode = e.Node;
            if(tvRootNode == currNode)
            {
                InitalizeListControl();
                statusBarPanel1.Text = "Double Click the Emploee Records";
                return;
            }
            else
            {
                statusBarPanel1.Text = "Click an Employee code to view individual record";
            }
            PopulateListView(currNode);
        }
    }
}
