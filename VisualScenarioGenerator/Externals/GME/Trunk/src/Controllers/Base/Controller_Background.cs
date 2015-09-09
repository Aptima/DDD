using System;
using System.Collections.Generic;
using System.Text;
using AME.Tools;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace AME.Controllers
{
    public partial class Controller
    {
        private ProgressDialog pdialog;
        private BackgroundWorker backgroundworker;
        private Form topForm;
        protected void BackgroundSetup(String title, String message, Int32 min, Int32 max, Int32 step)
        {
            backgroundworker = new BackgroundWorker();
            backgroundworker.WorkerSupportsCancellation = false;
            backgroundworker.WorkerReportsProgress = true;
            backgroundworker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);

            topForm = Application.OpenForms[Application.OpenForms.Count - 1];
            pdialog = new ProgressDialog();
            pdialog.SetTitle(title);
            pdialog.SetMessage(message);
            pdialog.SetMinimum(min);
            pdialog.SetMaximum(max);
            pdialog.SetStep(step);
            topForm.Enabled = false;
            pdialog.Show(topForm);
        }

        protected void BackgroundProgress()
        {
            if (pdialog != null)
            {
                backgroundworker.ReportProgress(1, pdialog);
                pdialog.Refresh();
                Application.DoEvents();
            }
        }

        protected void BackgroundDone()
        {
            topForm.Enabled = true;
            if (pdialog != null)
            {
                pdialog.SetDone();
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressDialog pdialog = e.UserState as ProgressDialog;
            pdialog.PerformStep();
        }
    }
}
