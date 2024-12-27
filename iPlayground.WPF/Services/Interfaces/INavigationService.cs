using iPlayground.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.WPF.Services.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : BaseViewModel;
        event Action<object> OnViewChanged;
    }
}
