using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.ViewModels;
using System.Windows;

namespace BgB_TeachingAssistant.Views
{
    public partial class ApplicationView : Window
    {
        public ApplicationView(ApplicationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
