using System;
using System.Threading.Tasks;
using System.Windows;
using iPlayground.Core.Models;
using iPlayground.Core.Models;
using iPlayground.WPF;
using iPlayground.WPF.Helpers;

namespace iPlayground.WPF.Views
{
    public partial class SecondScreenEndSessionWindow : Window
    {
        private readonly TaskCompletionSource<bool> _tcs = new();

        public SecondScreenEndSessionWindow(Session session, decimal finalAmount)
        {
            InitializeComponent();

            DataContext = new EndSessionDialogModel
            {
                ChildName = $"{session.Child.FirstName} {session.Child.LastName}",
                StartTime = session.StartTime,
                 Amount = finalAmount,
                ConfirmCommand = new RelayCommand(() =>
                {
                    _tcs.SetResult(true);
                    Close();
                }),
                CancelCommand = new RelayCommand(() =>
                {
                    _tcs.SetResult(false);
                    Close();
                })
            };

            DisplayHelper.ShowOnSecondaryScreen(this);
        }

        public Task<bool> ShowAsync()
        {
            Show();
            return _tcs.Task;
        }
    }
}