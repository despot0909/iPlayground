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
        public bool IsStorno { get; set; }
        public string? StornoReason { get; set; }
        public DateTime? StornoTime { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? LossAmount { get; set; }
        public bool HasLoss { get; set; }

        public decimal TotalVaucer { get; set; }
        public bool IsFinished { get; set; }
        public bool IsSynced { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPaused { get; set; }
        public DateTime? PauseStartTime { get; set; }
        public bool IsPauseOverdue { get; set; }
        public bool ShowEndButton { get; set; }
        public bool CanPause { get; set; }
        public string? PauseButtonText { get; set; }
        public string? SessionStatus { get; set; }

        public TimeSpan GetPauseDuration()
        {
            if (IsPaused && PauseStartTime.HasValue)
            {
                return DateTime.Now - PauseStartTime.Value;
            }
            return TimeSpan.Zero;
        }

        public bool IsOverPauseLimit(int maxPauseMinutes)
        {
            if (!IsPaused || !PauseStartTime.HasValue)
                return false;

            var pauseDuration = GetPauseDuration();
            return pauseDuration.TotalMinutes > maxPauseMinutes;
        }
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
