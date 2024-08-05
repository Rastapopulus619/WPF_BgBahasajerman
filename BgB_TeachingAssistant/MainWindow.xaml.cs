using BgB_TeachingAssistant.Data;
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

namespace BgB_TeachingAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccess _dataAccess;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _dataAccess = new DataAccess(_configuration);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up configuration: {ex.Message}");
            }
        }

        private void ButtonDanCuk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve all student names
                List<string> studentNames = QueryExecutor.ExecuteQuery<string>(_dataAccess, "GetStudentlist");

                // Bind the student names to the ComboBox
                StudentComboBox.ItemsSource = studentNames;

                // Optionally, select the first item if the list is not empty
                if (studentNames.Any())
                {
                    StudentComboBox.SelectedIndex = 0;
                }

                MessageBox.Show("Student list loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving student list: {ex.Message}");
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
}