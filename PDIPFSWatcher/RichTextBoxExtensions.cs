using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace PDIPFSWatcher
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBoxEx box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void AppendWithTime(this RichTextBoxEx box, string text, LogType logType = LogType.NORMAL,
            bool bold = false)
        {
            box.AppendText($"{DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)} |");

            string logText = "";
            Color textColor = box.SelectionColor;
            Color backgroundColor = box.SelectionBackColor;

            switch (logType)
            {
                case LogType.INFORMATION:
                    logText = "Info";
                    textColor = Color.White;
                    backgroundColor = Color.Green;
                    break;
                case LogType.WARNING:
                    logText = "Warning";
                    textColor = Color.Black;
                    backgroundColor = Color.Yellow;
                    break;
                case LogType.ERROR:
                    logText = "Error";
                    textColor = Color.White;
                    backgroundColor = Color.Red;
                    break;
                case LogType.NORMAL:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }

            box.SuspendPainting();
            if (!string.IsNullOrEmpty(logText))
            {
                box.AppendText(logText, textColor, backgroundColor, bold);
                box.AppendText($" |");
            }

            box.AppendText($" {text}");
            box.ResumePainting();
        }

        public static void AppendText(this RichTextBoxEx box, string text, Color textColor, Color bgColor,
            bool bold = false)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            var oldFG = box.SelectionColor;
            var oldBG = box.SelectionBackColor;
            var oldFont = box.SelectionFont;

            box.SelectionColor = textColor;
            box.SelectionBackColor = bgColor;
            box.SelectionFont = new Font(box.SelectionFont, FontStyle.Bold);
            box.AppendText(text);
            box.SelectionColor = oldFG;
            box.SelectionBackColor = oldBG;
            box.SelectionFont = oldFont;
        }
    }
}
