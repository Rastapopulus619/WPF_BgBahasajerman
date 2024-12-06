using BgB_TeachingAssistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BgB_TeachingAssistant
{
    public static class ViewModelRegistration
    {
        public static void RegisterViewModels(IServiceCollection services)
        {
            // Register individual ViewModels as specific types and as IPageViewModel
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<StudentViewModel>();
            services.AddTransient<PackageViewModel>();
            services.AddTransient<TestPage1ViewModel>();
            services.AddTransient<ApplicationViewModel>();

            // probably from older navigation system: ******** delete if not needed ********
            // Register them as IPageViewModel for collection injection
            //services.AddTransient<IPageViewModel>(provider => provider.GetRequiredService<DashboardViewModel>());
            //services.AddTransient<IPageViewModel>(provider => provider.GetRequiredService<StudentViewModel>());
            //services.AddTransient<IPageViewModel>(provider => provider.GetRequiredService<PackageViewModel>());
            //services.AddTransient<IPageViewModel>(provider => provider.GetRequiredService<TestPage1ViewModel>());


            // Register descriptors as IEnumerable<PageDescriptor>
            services.AddSingleton<IEnumerable<IPageDescriptor>>(new List<PageDescriptor>
    {
        new PageDescriptor
        {
            Name = "Dashboard",
            ViewModelType = typeof(DashboardViewModel),
            Icon = "icon-dashboard"
        },
        new PageDescriptor
        {
            Name = "Student",
            ViewModelType = typeof(StudentViewModel),
            Icon = "icon-student"
        },
        new PageDescriptor
        {
            Name = "Packages",
            ViewModelType = typeof(PackageViewModel),
            Icon = "icon-packages"
        },
        new PageDescriptor
        {
            Name = "Test1",
            ViewModelType = typeof(TestPage1ViewModel),
            Icon = "icon-test"
        }
    });
        }
    }
}
