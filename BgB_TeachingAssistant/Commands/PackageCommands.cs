using BgB_TeachingAssistant.Services;
using BgB_TeachingAssistant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BgB_TeachingAssistant.Commands
{
    public class PackageCommands
    {
        private readonly PackageNavigationService _packageNavigationService;
        private readonly PackageViewModel _viewModel;

        public ICommand PreviousPackageCommand { get; }
        public ICommand NextPackageCommand { get; }

        public PackageCommands(PackageViewModel viewModel, PackageNavigationService packageNavigationService)
        {
            _viewModel = viewModel;
            _packageNavigationService = packageNavigationService;

            PreviousPackageCommand = new RelayCommand(
                execute => _viewModel.SelectedPackageNumber = _packageNavigationService.PreviousPackage(_viewModel.SelectedPackageNumber),
                canExecute => _packageNavigationService.CanExecutePreviousPackage(_viewModel.SelectedPackageNumber));

            NextPackageCommand = new RelayCommand(
                execute => _viewModel.SelectedPackageNumber = _packageNavigationService.NextPackage(_viewModel.SelectedPackageNumber),
                canExecute => _packageNavigationService.CanExecuteNextPackage(_viewModel.SelectedPackageNumber));
        }
    } 
}
