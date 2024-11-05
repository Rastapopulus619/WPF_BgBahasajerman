using BgB_TeachingAssistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgB_TeachingAssistant
{
    public static class ViewModelRegistration
    {
        public static void RegisterViewModels(IServiceCollection services)
        {
            // Register individual ViewModels
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<StudentViewModel>();
            services.AddTransient<PackageViewModel>();

            // Register them as IPageViewModel for collection injection
            services.AddTransient<IPageViewModel, DashboardViewModel>();
            services.AddTransient<IPageViewModel, StudentViewModel>();
            services.AddTransient<IPageViewModel, PackageViewModel>();

            // Register the ApplicationViewModel itself
            services.AddTransient<ApplicationViewModel>();
        }
    }

}
