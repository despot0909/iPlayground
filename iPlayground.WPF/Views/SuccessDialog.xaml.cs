using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iPlayground.WPF.Views
{
    // Views/SuccessDialog.xaml.cs
    public partial class SuccessDialog : UserControl
    {
        public string Message { get; }

        public SuccessDialog(string message)
        {
            InitializeComponent();
            Message = message;
            DataContext = this;
        }
    }
}
