using iPlayground.WPF.Services.Interfaces;
using iPlayground.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.WPF.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        public event Action<object> OnViewChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : BaseViewModel
        {
            Debug.WriteLine($"NavigationService.NavigateTo<{typeof(T).Name}>");
            var viewModel = _serviceProvider.GetRequiredService<T>();
            OnViewChanged?.Invoke(viewModel);
        }


    }
}
