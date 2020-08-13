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
using PDIPFSWatcher.Models;
using PDIPFSWatcher.Tracing;
using PDIPFSWatcher.Tracing.Filters;

namespace PDIPFSWatcher
{
    public partial class Main : Form
    {
        private VolumeInfo _volInfo;
        private ComboboxItem[] _processes;
        private int _selectedPid = -1;

        public Main()
        {
            InitializeComponent();

            button_startWatching.Enabled = false;
            button_stopWatching.Enabled = false;
            button_refreshProcesses.Enabled = false;

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
                button_refreshProcesses.Enabled = true;

                PopulateProcessPicker();

                toolStripStatusLabel.Text = $"Path loaded";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel.Text = ex.Message;
            }
        }

        private void _volInfo_NewFileAccess(object sender, EventArgs e)
        {
            var eventInfo = (PDIPFSFileAccessEventArgs)e;
            PrintFileAccess(eventInfo);
        }

        private void PrintFileAccess(PDIPFSFileAccessEventArgs eventInfo)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<PDIPFSFileAccessEventArgs>(PrintFileAccess), eventInfo);
                return;
            }

            richTextBox.AppendWithTime(
                $"{eventInfo.PDIPFS,-10} | " +
                $"Offset: {eventInfo.Offset:X16} | " +
                $"Size: {eventInfo.Size:X8} | " +
                $"IrpPtr: {eventInfo.IrpPtr:X16} | " +
                $"ProcessID: {eventInfo.ProcessID:X8} | " +
                $"ProcessName: {(eventInfo.ProcessName.Length > 10 ? $"{eventInfo.ProcessName.Left(7)}..." : eventInfo.ProcessName),-10} | " +
                $"{eventInfo.HRPath,5}\r\n",
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

        private void comboBox_processPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(comboBox_processPicker.SelectedItem is ComboboxItem selectedItem) || selectedItem.Value == -1 ||
                    selectedItem.Value == -2)
                {
                    _selectedPid = -2;
                    _volInfo.TraceManager.Filter = null;
                }
                else
                {
                    _selectedPid = selectedItem.Value;

                    _volInfo.TraceManager.Filter = new TraceEventFilter
                    {
                        FilterRules = { new ProcessIdFilter(true, selectedItem.Value) }
                    };
                }
            }
            catch (Exception ex)
            {
                // Hmm do nothing, at least for now.
            }
        }

        private void button_refreshProcesses_Click(object sender, EventArgs e)
        {
            if (_volInfo != null)
                PopulateProcessPicker();
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

        private void PopulateProcessPicker()
        {
            comboBox_processPicker.Items.Clear();

            Process[] processCollection = Process.GetProcesses()
                .OrderBy(x => x.ProcessName)
                .ToArray();
            _processes = new ComboboxItem[processCollection.Length + 1];
            _processes[0] = new ComboboxItem
            {
                Text = "All",
                Value = -2
            };

            for (int i = 0; i < processCollection.Length; i++)
            {
                var process = processCollection[i];
                _processes[i + 1] = new ComboboxItem
                {
                    Text = process.ProcessName,
                    Value = process.Id
                };
            }

            comboBox_processPicker.Items.AddRange(_processes);

            switch (_selectedPid)
            {
                case -1:
                    var rpcs3 = _processes.FirstOrDefault(x => x.Text.Contains("rpcs3"));
                    if (rpcs3 != null)
                    {
                        comboBox_processPicker.SelectedIndex = comboBox_processPicker.Items.IndexOf(rpcs3);
                    }
                    else
                    {
                        goto case -2;
                    }
                    break;
                case -2:
                    comboBox_processPicker.SelectedIndex = 0;
                    break;
                default:
                    var selectedProcess = _processes.FirstOrDefault(x => x.Value == _selectedPid);
                    if (selectedProcess != null)
                    {
                        comboBox_processPicker.SelectedIndex = comboBox_processPicker.Items.IndexOf(selectedProcess);
                    }
                    else
                    {
                        goto case -2;
                    }
                    break;
            }
        }
    }
}
