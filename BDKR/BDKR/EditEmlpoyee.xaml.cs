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
    /// Логика взаимодействия для EditEmlpoyee.xaml
    /// </summary>
    public partial class EditEmlpoyee : Window
    {
        private Employee _employee;
        public EditEmlpoyee(Employee employee)
        {
            InitializeComponent();
            _employee = employee;
            DeleteSpace();
            DataContext = _employee;
            CompanyCombo.ItemsSource = BDKREntities.GetContext().Company.ToList();
            CompanyCombo.Text = _employee.Post.Company.Adress;

            PostCombo.ItemsSource = BDKREntities.GetContext().Post.ToList();
            PostCombo.Text = _employee.Post.PostName;
        }

        private void SaveChange_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_employee.Surname))
                errors.AppendLine("Поле \"Фамилия\" является обязательным");
            if (string.IsNullOrWhiteSpace(_employee.Name))
                errors.AppendLine("Поле \"Имя\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            List<Employee> empl = BDKREntities.GetContext().Employee
                .Where(emp => emp.Post.PostName.Contains("Директор"))
                .Where(emp=>emp.Post.Company.Adress==_employee.Post.Company.Adress)
                .ToList();

            if (empl.Count != 0)
            {
                if (PostCombo.Text.Contains("Директор") && empl[0].EmployeeId != _employee.EmployeeId)
                {
                    MessageBox.Show("В данной компании уже есть директор");
                    return;
                }
            }

            List<Post> post = BDKREntities.GetContext().Post.Where(p => p.PostName.Contains(PostCombo.Text))
                .Where(p => p.Company.Adress.Contains(CompanyCombo.Text)).ToList();
            try
            {
                _employee.PostId = post[0].PostId;
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }

        }

        private void DeleteSpace()
        {
            _employee.Name = _employee.Name.Replace(" ", "");
            _employee.Surname = _employee.Surname.Replace(" ", "");
            _employee.MiddleName = _employee.MiddleName?.Replace(" ", "");
        }
    }
}
