﻿using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        public override string Name => "Student";
        private readonly GeneralDataService _generalDataService;
        public ICommand DanCukCommand { get; }
        public ICommand LoadStudentsCommand { get; }

        private ObservableCollection<StudentModel> _students;
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value, nameof(Students));  // Provide the property name
        }

        private ObservableCollection<string> _studentNames;
        public ObservableCollection<string> StudentNames
        {
            get => _studentNames;
            set => SetProperty(ref _studentNames, value, nameof(StudentNames));
        }
        public StudentViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
            _generalDataService = serviceFactory.CreateGeneralDataService();
            LoadStudentsCommand = new RelayCommand(async () => await LoadStudentsAsync());
            DanCukCommand = new RelayCommand(DanCukMethod);
        }
        private async Task LoadStudentsAsync()
        {
                // Retrieve students using the data service
                var students = await _generalDataService.GetStudentsAsync();
                Students = new ObservableCollection<StudentModel>(students);
        }
        private async void DanCukMethod()
            {
                try
                {
                    // Retrieve and set student names
                    var studentNames = await _generalDataService.GetStudentNamesAsync();
                    MessageBox.Show($"first value in student list: {studentNames[0]}");
                    StudentNames = new ObservableCollection<string>(studentNames);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving student list: {ex.Message}");
                }
            }
    }
}
