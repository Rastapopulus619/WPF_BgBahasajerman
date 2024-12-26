using BgB_TeachingAssistant.ViewModels;
using BgB_TeachingAssistant.Helpers.UIInteraction; // For FindVisualChild
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BgB_TeachingAssistant.Commands;
using System.Windows.Media;

namespace BgB_TeachingAssistant.Services
{
    public class PromptService : IPromptService
    {
        public bool ShowOkCancelCommandChoicePrompt(string title, string message, ICommand okCommand, ICommand cancelCommand)
        {
            Window promptWindow = null; // Declare before creating the view model

            // Wrap commands to include window-closing logic
            var wrappedOkCommand = new RelayCommand(_ =>
            {
                okCommand?.Execute(null);
                promptWindow.DialogResult = true;
                promptWindow.Close();
            });

            var wrappedCancelCommand = new RelayCommand(_ =>
            {
                cancelCommand?.Execute(null);
                promptWindow.DialogResult = false;
                promptWindow.Close();
            });

            // Create the PromptViewModel with the wrapped commands
            var promptViewModel = new PromptViewModel(wrappedOkCommand, wrappedCancelCommand)
            {
                Title = title,
                Content = message
            };

            promptWindow = new Window
            {
                Style = (Style)Application.Current.FindResource("BasicPromptWindowStyle"), // Use shared style
                Content = new Views.Resources.UserControls.ModularPrompt { DataContext = promptViewModel },
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Owner = Application.Current.MainWindow
            };

            // Handle Esc key press to close the window
            promptWindow.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    promptWindow.DialogResult = false;
                    promptWindow.Close();
                }
            };

            // Set focus to the OK button when the window is loaded
            promptWindow.Loaded += (sender, e) =>
            {
                var okButton = VisualTreeHelperExtensions.FindVisualChild<Button>(promptWindow, b => b.Content?.ToString() == "OK");
                okButton?.Focus();
            };

            return promptWindow.ShowDialog() == true;
        }
        public bool ShowOkCancelPrompt(string title, string message)
        {
            Window promptWindow = null;

            // Define the OK and Cancel actions
            var okCommand = new RelayCommand(_ =>
            {
                promptWindow.DialogResult = true;
                promptWindow.Close();
            });

            var cancelCommand = new RelayCommand(_ =>
            {
                promptWindow.DialogResult = false;
                promptWindow.Close();
            });

            // Create the ViewModel
            var promptViewModel = new PromptViewModel(okCommand, cancelCommand)
            {
                Title = title,
                Content = message,
            };

            // Create the Window
            promptWindow = new Window
            {
                Style = (Style)Application.Current.FindResource("BasicPromptWindowStyle"), // Use shared style
                Content = new Views.Resources.UserControls.ModularPrompt { DataContext = promptViewModel },
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Owner = Application.Current.MainWindow
            };

            // Handle Esc key press to close the window
            promptWindow.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    promptWindow.DialogResult = false;
                    promptWindow.Close();
                }
            };

            // Set focus to the OK button when the window is loaded
            promptWindow.Loaded += (sender, e) =>
            {
                var okButton = VisualTreeHelperExtensions.FindVisualChild<Button>(promptWindow, b => b.Content?.ToString() == "OK");
                okButton?.Focus();
            };

            return promptWindow.ShowDialog() == true;
        }


        public void ShowInformationPrompt(string title, string message)
        {
            Window promptWindow = null;

            // Define the OK command to close the window
            var okCommand = new RelayCommand(_ =>
            {
                promptWindow.DialogResult = true;
                promptWindow.Close();
            });

            // Create the ViewModel
            var viewModel = new InformationalPromptViewModel(okCommand)
            {
                Title = title,
                Message = message
            };

            // Create the Window
            promptWindow = new Window
            {
                Content = new Views.Resources.UserControls.InformationalPrompt { DataContext = viewModel },
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Owner = Application.Current.MainWindow
            };

            // Set focus to the OK button when the window is loaded
            promptWindow.Loaded += (sender, e) =>
            {
                var okButton = VisualTreeHelperExtensions.FindVisualChild<Button>(promptWindow, b => b.Content?.ToString() == "OK");
                okButton?.Focus();
            };

            promptWindow.ShowDialog();
        }



    }
}
