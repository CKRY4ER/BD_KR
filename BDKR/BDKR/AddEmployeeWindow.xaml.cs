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

namespace BDKR
{

    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private Employee _employee = new Employee();

        private List<Post> postName = new List<Post>() { new Post() { PostName = "Директор" }, new Post() { PostName = "Заместитель директора"},
            new Post() { PostName= "Главный кассир"}, new Post() { PostName= "Расфасовщик"},
            new Post() { PostName= "Кассир"}, new Post() { PostName= "Администратор"} };
        public AddEmployeeWindow()
        {
            InitializeComponent();
            DataContext = _employee;

            CompanyCombo.ItemsSource = BDKREntities.GetContext().Company.ToList();
            PostCombo.ItemsSource = postName.ToList();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (CompanyCombo.Text == "")
                errors.AppendLine("Поле \"Место работы\" является обязательным");
            if (PostCombo.Text=="")
                errors.AppendLine("Поле \"Должность\" является обязательным");
            if (string.IsNullOrWhiteSpace(_employee.Surname))
                errors.AppendLine("Поле \"Фамилия\" является обязательным");
            if (string.IsNullOrWhiteSpace(_employee.Name))
                errors.AppendLine("Поле \"Имя\" является обязательным");

            if (CompanyCombo.Text.Contains("Директор"))
            {
                List<Employee> employees = BDKREntities.GetContext().Employee
                    .Where(empl => empl.Post.PostName.Contains("Директор"))
                    .Where(empl => empl.Post.Company.Adress.Contains(CompanyCombo.Text))
                    .ToList();

                if (employees.Count != 0)
                {
                    MessageBox.Show("В выбранной компании уже есть директор");
                    return;
                }
            }

            List<Post> post = BDKREntities.GetContext().Post.Where(p => p.PostName.Contains(PostCombo.Text))
                .Where(p => p.Company.Adress.Contains(CompanyCombo.Text)).ToList();

            if (post.Count == 0)
            {
                MessageBox.Show("Выбранная должность не доступна в данном месте работы");
                return;

            }
            _employee.PostId = post[0].PostId;

            BDKREntities.GetContext().Employee.Add(_employee);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                this.Close();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }
    }
}
