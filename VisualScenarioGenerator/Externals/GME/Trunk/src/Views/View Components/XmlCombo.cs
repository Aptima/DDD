using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Data;
using AME.Controllers;
using AME.Model;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Security.Cryptography;

namespace AME.Views.View_Components
{
    public partial class XmlCombo : ComboBox, ISimulationComponent
    {
        private ModelingController myController;

        private String xsl;
        private XslCompiledTransform transform;

        public String Xsl
        {
            get
            {
                return xsl;
            }
            set
            {
                xsl = value;
                if (xsl != null || myController != null)
                {
                    transform = new XslCompiledTransform();
                    try
                    {
                        XmlReader xslreader = myController.GetXSL(value);
                        transform.Load(xslreader);
                        xslreader.Close();
                    }
                    catch (Exception ex)
                    {
                        transform = null;
                        xsl = null;
                        MessageBox.Show(ex.Message, "Failed to load transform", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string NameAttribute;

        private string m_XPath;

        public string XPath
        {
            get { return m_XPath; }
            set { m_XPath = value; }
        }

        private XPathNavigator m_XPathDocument;

        public XPathNavigator XmlDocument
        {
            get { return m_XPathDocument; }
            set { m_XPathDocument = value; }
        }

        private string crc = "";

        private string selectedElement = "";

        public XmlCombo()
            : base()
        {
            this.InitializeComponent();

            NameAttribute = "Name";

            this.DropDownStyle = ComboBoxStyle.DropDownList; // non editable list
        }

        #region ViewComponentUpdate Members

        public ModelingController Controller
        {
            get
            {
                return myController;
            }
            set
            {
                myController = value;
            }
        }

        public void UpdateViewComponent()
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            Byte[] bs = System.Text.Encoding.UTF8.GetBytes(m_XPathDocument.OuterXml);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (Byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String newCrc = s.ToString();

            if (!crc.Equals(newCrc)) // CRC check - see CustomTreeView and ParameterTable
            {
                DrawingUtility.SuspendDrawing(this);

                this.Items.Clear();

                crc = newCrc;

                MemoryStream ms = new MemoryStream();
                transform.Transform(m_XPathDocument, null, ms);

                // load the results into an XPathDocument object
                ms.Seek(0, SeekOrigin.Begin);
                XPathDocument doc = new XPathDocument(ms);

                XPathNavigator nav = doc.CreateNavigator();

                XPathNodeIterator listOfNodes = nav.Select("/*/*");

                String itemToAdd;

                foreach (XPathNavigator childNode in listOfNodes)
                {
                    itemToAdd = childNode.GetAttribute(NameAttribute, childNode.NamespaceURI);
                    this.Items.Add(itemToAdd);

                    if (this.selectedElement.Equals(itemToAdd))
                    {
                        this.SelectedItem = itemToAdd;
                    }
                }

                DrawingUtility.ResumeDrawing(this);
            }
        }
        #endregion
    }
}
