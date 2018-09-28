using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace Katarai.Wpf.Utils
{
    public class ListBoxTraceLogger : TraceListener 
    {
        private readonly ListBox _output ;

        public ListBoxTraceLogger(ListBox output)
        {
            _output = output;
        }

        public override void Write(string message)
        {
            var listBoxItem = new ListBoxItem
            {
                Content = string.Format("{0}{1}{0}{2}", Environment.NewLine, DateTime.Now, message)
            };
            _output.Items.Insert(0,listBoxItem);
            _output.ScrollIntoView(listBoxItem);
        }

        public override void WriteLine(string message)
        {
            Write(string.Format("{0}{1}",message,Environment.NewLine));
        }
    }
}