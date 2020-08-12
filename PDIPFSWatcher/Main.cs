using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GT.Shared;

namespace PDIPFSWatcher
{
    public partial class Main : Form
    {
        private VolumeInfo _volInfo;

        public Main()
        {
            InitializeComponent();

            button_startWatching.Enabled = false;
            button_stopWatching.Enabled = false;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            this.Text += $" (Alpha {version})";
        }

        #region Events
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vistaFolderBrowserDialog1.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _volInfo = new VolumeInfo(vistaFolderBrowserDialog1.SelectedPath);
                _volInfo.NewFileAccess += _volInfo_NewFileAccess;

                button_startWatching.Enabled = true;

                toolStripStatusLabel.Text = $"Path loaded";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel.Text = ex.Message;
            }
        }

        private void _volInfo_NewFileAccess(object sender, EventArgs e)
        {
            var fileInfo = (PDIPFSFileAccessEventArgs)e;
            PrintFileAccess(fileInfo.PDIPFS, fileInfo.HRPath, fileInfo.Offset, fileInfo.Size, fileInfo.IrpPtr);
        }

        private void PrintFileAccess(string pdipfsPath, string hrPath, long offset, int size, ulong irpPtr)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, string, long, int, ulong>(PrintFileAccess), pdipfsPath, hrPath, offset,
                    size, irpPtr);
                return;
            }

            richTextBox.AppendWithTime(
                $"{pdipfsPath,-15} | Offset: {offset:X16} | Size: {size:X8} | IrpPtr: {irpPtr:X16} | {hrPath,5}\r\n",
                logType: LogType.NORMAL);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.MessageLoop)
            {
                // WinForms app
                Application.Exit();
            }
            else
            {
                // Console app
                Environment.Exit(1);
            }
        }

        private void button_startWatching_Click(object sender, EventArgs e)
        {
            ChangeWatching(true);
        }

        private void button_stopWatching_Click(object sender, EventArgs e)
        {
            ChangeWatching(false);
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            richTextBox.Clear();
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!checkBox_autoScroll.Checked) return;

            // set the current caret position to the end
            richTextBox.SelectionStart = richTextBox.Text.Length;
            // scroll it automatically
            richTextBox.ScrollToCaret();
        } 
        #endregion

        private void ChangeWatching(bool watch)
        {
            if (watch)
            {
                _volInfo.StartWatching();
                button_startWatching.Enabled = false;
                button_stopWatching.Enabled = true;
            }
            else
            {
                _volInfo.StopWatching();
                button_startWatching.Enabled = true;
                button_stopWatching.Enabled = false;
            }
        }
    }
}
