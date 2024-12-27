using iPlayground.Core.Models.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iPlayground.Core.Models
{
    public class Session : BaseEntity, INotifyPropertyChanged
    {
        private DateTime _startTime;
        private DateTime? _endTime;

        public int Id { get; set; }
        public int ChildId { get; set; }
        public virtual ICollection<SessionVoucher> Vouchers { get; set; } = new List<SessionVoucher>();


        public decimal VouchersDiscountAmount
        {
            get
            {
                decimal totalDiscount = Vouchers?.Where(v => v.IsValid)
                                               .Sum(v => v.DiscountAmount) ?? 0;
                return Math.Min(totalDiscount, TotalAmount); // Ne može biti veće od ukupnog iznosa
            }
        }

        public decimal FinalAmount => Math.Max(0, TotalAmount - VouchersDiscountAmount);
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(CompletedHours));
            }
        }

        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(CompletedHours));
            }
        }

        public decimal TotalAmount { get; set; }

        public decimal TotalVaucer { get; set; }
        public bool IsFinished { get; set; }
        public bool IsSynced { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Child Child { get; set; }

        public TimeSpan Duration => EndTime.HasValue
            ? EndTime.Value - StartTime
            : DateTime.Now - StartTime;

        public int CompletedHours => (int)Math.Ceiling(Duration.TotalHours);
   

        public string FormattedDuration => $"{(int)Duration.TotalHours:D2}:{Duration.Minutes:D2}";

 

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
