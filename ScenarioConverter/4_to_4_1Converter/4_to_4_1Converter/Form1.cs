using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace _4_to_4_1Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void inputXMLBrowseBtn_Click(object sender, EventArgs e)
        {
            String fileChosen = null;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            dialog.Title = "Select the xml file";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileChosen = dialog.FileName;
                this.inputXMLTxtBox.Text = fileChosen;
                this.toolStripStatusLabel.Text = "";
            }
        }

        private void outputXMLBrowseBtn_Click(object sender, EventArgs e)
        {
            String fileChosen = null;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter =
               "xml files (*.xml)|*.xml";
            dialog.Title = "Select the xsl/xslt file";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileChosen = dialog.FileName;
                this.outputXMLTxtBox.Text = fileChosen;
                this.toolStripStatusLabel.Text = "";
            }
        }

        private void transformBtn_Click(object sender, EventArgs e)
        {
            try
            {               
                if (isInputValid())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this.toolStripStatusLabel.Text = "Transforming XML";
                    //load the Xml doc
                    XPathDocument myXPathDoc = new XPathDocument(this.inputXMLTxtBox.Text);
                    XslTransform myXslTrans = new XslTransform();

                    myXslTrans.Load(new XmlTextReader(new StringReader(Properties.Resources._4_to_4_1)));

                    //create the output stream
                    XmlTextWriter myWriter = new XmlTextWriter
                        (this.outputXMLTxtBox.Text, null);
                    myWriter.Formatting = Formatting.Indented;

                    myWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

                    //do the actual transform of Xml
                    myXslTrans.Transform(myXPathDoc, null, myWriter);
                    myWriter.Close();
                    Cursor.Current = Cursors.Default;
                    this.toolStripStatusLabel.Text = "Transformation Complete";
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
                Cursor.Current = Cursors.Default;
                this.toolStripStatusLabel.Text = ex.Message;
                MessageBox.Show("Error Transforming XML " + ex.Message);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isInputValid()
        {
            if (this.inputXMLTxtBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter input XML file to Transform");
                return false;
            }
            if (this.outputXMLTxtBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter the name of output file");
                return false;
            }
            return true;
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            this.inputXMLTxtBox.Clear();
            this.outputXMLTxtBox.Clear();
            this.toolStripStatusLabel.Text = "";
        }
    }
}
