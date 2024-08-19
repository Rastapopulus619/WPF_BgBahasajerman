using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.ViewModels;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BgB_TeachingAssistant.Views
{
    /// <summary>
    /// Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : Window
    {
        private readonly IMessages _messages;
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        public ApplicationView(IMessages messages, IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            InitializeComponent();

            _messages = messages;
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;

            DataContext = new ApplicationViewModel(dataAccess, queryLoader);
        }
    }
}
