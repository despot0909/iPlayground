using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iPlayground.WPF.Views
{
    // Views/ErrorDialog.xaml.cs
    public partial class ErrorDialog : UserControl
    {
        public string Message { get; }

        public ErrorDialog(string message)
        {
            InitializeComponent();
            Message = message;
            DataContext = this;
        }
    }
}
