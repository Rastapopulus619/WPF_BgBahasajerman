using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Protobuf;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.QueryLoaders;
using MySqlX.XDevAPI.Common;
using Bgb_DataAccessLibrary.Models.StudentModels;
using BgB_TeachingAssistant.ViewModels;

namespace BgB_TeachingAssistant.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMessages _messages;
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

        public MainWindow(IMessages messages, IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            InitializeComponent();
            _messages = messages;
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;

            // Set DataContext to a new instance of StudentViewModel
            DataContext = new StudentViewModel(dataAccess, queryLoader);

        }

        private async void ButtonDanCuk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve all student names

                // with dynamic type:
                //var result = _dataAccess.QueryAsync<dynamic>(_queryLoader.GetQuery("GetStudentList")).Result.ToList();

                // Convert the result to a list of strings
                //List<string> studentNames = result
                //    .Select(item => (string)item.Name)  // Assuming the dynamic object has a property called StudentName
                //    .ToList();


                // Retrieve all student names
                List<StudentModel> studentList = await GetStudentsAsync();

                // Convert the list of students to a list of names
                List<string> studentNames = studentList.Select(s => s.Name).ToList();

                // Set the ComboBox's ItemsSource to the list of names
                StudentComboBox.ItemsSource = studentNames;


                MessageBox.Show("Student list loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving student list: {ex.Message}");
            }
        }
        private async Task<List<StudentModel>> GetStudentsAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            return (await _dataAccess.QueryAsync<StudentModel>(query)).ToList();
        }
    }
}

//private void ButtonDanCuk_Click(object sender, RoutedEventArgs e)
//    {
//        string studentName = "Aaron";
//MessageBox.Show(studentName);

//DataAccess db = new DataAccess();

//string studentID = db.GetStudentID(studentName).ToString();

//string studentID = _dataAccess.GetStudentID(studentName).ToString();

//myLabel.Content = $"Welcome, {studentName} with StudentID {studentID}!";

/* ///
string connectionString = "SERVER=localhost; DATABASE=bgbahasajerman; UID=root; PASSWORD=Burungnuri1212";
MySqlConnection con = new MySqlConnection(connectionString);

MySqlCommand cmd = new MySqlCommand("SELECT StudentID FROM students WHERE Name = 'Aaron'", con);
con.Open();

DataTable dt = new DataTable();
dt.Load(cmd.ExecuteReader());

con.Close();

dtGrid.DataContext = dt;

myLabel.Content = dt.Rows[0]["StudentID"].ToString();
/// */



//con.Open();
//MessageBox.Show(dt.);

//myLabel.Foreground = Brushes.Green;
//}
//}
