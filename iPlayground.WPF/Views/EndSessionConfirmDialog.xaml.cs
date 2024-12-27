using System;
using System.Windows;
using System.Windows.Controls;
using iPlayground.Core.Models;
using MaterialDesignThemes.Wpf;

namespace iPlayground.WPF.Views
{
    public partial class EndSessionConfirmDialog : UserControl
    {
        private readonly EndSessionDialogModel _model;

        public EndSessionConfirmDialog()
        {
            InitializeComponent();
            _model = new EndSessionDialogModel();
            DataContext = _model;
        }

        public void InitializeData(Session session, decimal finalAmount)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _model.ChildName = $"{session.Child?.FirstName} {session.Child?.LastName}";
            _model.StartTime = session.StartTime;
            _model.Duration = DateTime.Now - session.StartTime;
            _model.Amount = finalAmount;

            // Ako želimo da osvježimo UI nakon ažuriranja podataka
            if (this.DataContext is EndSessionDialogModel model)
            {
                model.ChildName = _model.ChildName;
                model.StartTime = _model.StartTime;
                model.Duration = _model.Duration;
                model.Amount = _model.Amount;
            }
        }
    }
}