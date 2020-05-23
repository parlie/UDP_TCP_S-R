using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using MahApps.Metro.IconPacks;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;
using System.Windows.Media;

namespace UDP_TCP_S_R
{
    public class Log
    {
        enum HighlightColor { Red, Green, Blue, Black, Orange }; //Color types for text appending

        private static RichTextBox richTextBoxDefault = MainWindow.mw.LogRichBox;

        #region Log console
        /// <summary>
        /// Returns current time in [HH:mm:ss] format.
        /// </summary>
        /// <returns></returns>
        private static string TimeStamp()
        {
            return "[" + DateTime.Now.ToString("HH:mm:ss") + "] ";
        }

        /// <summary>
        /// Logs string with red highlight; intended for errors.
        /// </summary>
        /// <param name="data">String of information to write.</param>
        public static void WriteError(string data)
        {
            string message = TimeStamp() + "! " + data;
            richTextBoxDefault.AppendText(message + "\n");
            Coloring(richTextBoxDefault, data, Colors.Red);
            richTextBoxDefault.ScrollToEnd();
        }

        /// <summary>
        /// Logs string with green highlight; intended for general succsesfulnes.
        /// </summary>
        /// <param name="data">String of information to write.</param>
        public static void WriteSucces(string data)
        {
            string message = TimeStamp() + "- " + data;
            richTextBoxDefault.AppendText(message + "\n");
            Coloring(richTextBoxDefault, data, Colors.Green);
            richTextBoxDefault.ScrollToEnd();
        }

        /// <summary>
        /// Logs string with no highlight; intended for plain text.
        /// </summary>
        /// <param name="data">String of information to write.</param>
        public static void Write(string data)
        {
            string message = TimeStamp() + "- " + data;
            richTextBoxDefault.AppendText(message + "\n");
            Coloring(richTextBoxDefault, data, Colors.Black);
            richTextBoxDefault.ScrollToEnd();
        }

        /// <summary>
        /// Logs string with orange highlight; intended for informative statements.
        /// </summary>
        /// <param name="data">String of information to write.</param>
        public static void WriteInfo(string data)
        {
            string message = TimeStamp() + "> " + data;
            richTextBoxDefault.AppendText(message + "\n");
            Coloring(richTextBoxDefault, data, Colors.Orange);
            richTextBoxDefault.ScrollToEnd();
        }

        /// <summary>
        /// Logs string with blue highlight; intended for responses.
        /// </summary>
        /// <param name="data">String of information to write.</param>
        public static void WriteResponse(string data)
        {
            string message = TimeStamp() + "< " + data;
            richTextBoxDefault.AppendText(message + "\n");
            Coloring(richTextBoxDefault, data, Colors.Blue);
            richTextBoxDefault.ScrollToEnd();
        }

        /// <summary>
        /// Specifies color of text in log text field.
        /// </summary>
        /// <param name="data">Data to log.</param>
        /// <param name="color">Color.</param>
        private static void Coloring(RichTextBox richTextBox, string data, Color color)
        {
            int nlcount = data.ToCharArray().Count(a => a == '\n');
            int lenght = data.Length + 3 * (nlcount) + 2;
            TextPointer textPointer1 = richTextBox.Document.ContentEnd.GetPositionAtOffset(-lenght - 4);
            TextPointer textPointer2 = richTextBox.Document.ContentEnd.GetPositionAtOffset(-1);
            richTextBox.Selection.Select(textPointer1, textPointer2);
            SolidColorBrush solidColorBrush = new SolidColorBrush(color);
            richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, solidColorBrush);
        }
        #endregion
    }
}
