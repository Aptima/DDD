using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using LogFileViewer.ViewController;
using System.IO;


namespace LogFileViewer
{
    public partial class LogFileViewer : Form
    {
        private ViewController.ViewController controller;
        private Uri XSLTransform;
        private bool StartedWithArguement = false;

        public LogFileViewer()
        {
            InitializeComponent();
            controller = new ViewController.ViewController();
            controller.LoadReportTransforms();
            webBrowser1.ScriptErrorsSuppressed = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;

            foreach (String report in controller.reportTransforms.Keys)
            {
                tsReportSelector.Items.Add(report);
            }

            StatusMessage("Ready.");
        }

        public LogFileViewer(string filename)
        {
            InitializeComponent();
            controller = new ViewController.ViewController();
            webBrowser1.ScriptErrorsSuppressed = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser1.WebBrowserShortcutsEnabled = false;

            try
            {
                string transform_file = controller.LoadReplayLog(filename);
                if (transform_file != string.Empty)
                {
                    StatusMessageFileAttributes(filename);
                    XSLTransform = new Uri(transform_file);
                    webBrowser1.Navigate(XSLTransform);

                    tsPrint.Enabled = true;
                    tsReportSelector.Enabled = true;
                    tsReportSelector.SelectedIndex = 0;
                }

                StartedWithArguement = true;
                StatusMessage("Ready.");

            }
            catch (System.IO.IOException exc)
            {
                MessageBox.Show(exc.Message, "Unable to open logfile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }


        public void StatusMessage(string message)
        {
            tsProgressText.Text = message;
        }

        public void StatusMessageFileAttributes(string filename)
        {
            FileInfo file = new FileInfo(filename);
            float filesize = (float)file.Length;
            if (file.Length < 1000)
            {
                StatusMessage(string.Format("Filesize: {0} bytes.", filesize));
            }
            else if (file.Length < 1000000)
            {
                filesize /= 1000;
                StatusMessage(string.Format("Filesize: {0} KB.", Math.Round(filesize, 2)));
            }
            else
            {
                filesize /= 1000000;
                StatusMessage(string.Format("Filesize: {0} MB.", Math.Round(filesize, 2)));
                if (filesize > 7)
                {
                    MessageBox.Show("Expect slow response times when navigating to and from\nData views containing 1000 or more records.",
                        "Warning: Reading a Large Data File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {

            if (!StartedWithArguement)
            {
                if (OpenFile() == DialogResult.Cancel)
                {
                    this.Dispose();
                }
            }

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            controller.Dispose();
        }



        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private DialogResult OpenFile()
        {
            // OpenFile
            DialogResult result = DialogResult.Cancel;
            string ddd_client_share = string.Format(@"\\{0}\DDDClient", System.Environment.MachineName);
            OpenFileDialog file = new OpenFileDialog();
            if (Directory.Exists(ddd_client_share))
            {
                file.InitialDirectory = ddd_client_share;
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                    return System.Windows.Forms.DialogResult.Cancel;
                String f = fbd.SelectedPath;
                if (Directory.Exists(f))
                {
                    file.InitialDirectory = f;
                }
            }
            file.Multiselect = false;
            file.DefaultExt = "ddd";
            file.Filter = "DDD 4.0 sp4 Replay files [*.ddd]|*.ddd|DDD 4.0 Replay files [*.txt]|*.txt";

            try
            {

                if ((result = file.ShowDialog()) == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;
                    string transform_file = controller.LoadReplayLog(file.FileName);
                    if (transform_file != string.Empty)
                    {
                        StatusMessageFileAttributes(file.FileName);
                        XSLTransform = new Uri(transform_file);
                        webBrowser1.Navigate(XSLTransform);

                        tsPrint.Enabled = true;
                        tsReportSelector.Enabled = true;
                        tsReportSelector.SelectedItem = "Summary";
                    }
                    else
                    {
                        MessageBox.Show(
                            "The file is not a valid DDD replay file.",
                            "Open File Error",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Warning
                            );
                    }
                    Cursor = Cursors.Default;
                }
                file.Dispose();
            }
            catch (System.IO.IOException exc)
            {
                MessageBox.Show(exc.Message, "Unable to open logfile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                file.Dispose();
                this.Dispose();
            }
            return result;
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //ExportFile
            ExportDialog export = new ExportDialog(controller, ((String)tsReportSelector.SelectedItem));
            if (export.ShowDialog() == DialogResult.OK)
            {
            }
            else
            {
                export.Dispose();
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (webBrowser1.Url != null)
            {
                webBrowser1.ShowPrintDialog();
            }
        }


        private void tsReportSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            String item = (String)tsReportSelector.SelectedItem;
            tsSaveButton.Enabled = controller.reportTransforms[item].HasCSV;

            Cursor = Cursors.WaitCursor;

            string transformfile = controller.DisplayReport(item);

            if (transformfile != string.Empty)
            {
                webBrowser1.Navigate(new Uri(transformfile));
                Cursor = Cursors.Default;
                return;
            }
            else
            {
                MessageBox.Show(
                    "The file is not a valid DDD replay file.",
                    "Open File Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning
                    );
            }

            Cursor = Cursors.Default;

        }

        private void LogFileViewer_Load(object sender, EventArgs e)
        {
            
        }


    }
}