﻿using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

        public ApplicationViewModel(IServiceFactory serviceFactory, IDataAccess dataAccess, IQueryLoader queryLoader)
            : base(serviceFactory)
        {
            // Resolve dependencies using the service factory
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;

            // Add available pages
            PageViewModels.Add(new DashboardViewModel(serviceFactory));
            PageViewModels.Add(new StudentViewModel(serviceFactory));
            PageViewModels.Add(new PackageViewModel(serviceFactory));
            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (viewModel != null && CurrentPageViewModel != viewModel)
            {
                CurrentPageViewModel = viewModel;
            }
        }
    }
}
