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
    /// Логика взаимодействия для AddCompanyWindow.xaml
    /// </summary>
    public partial class AddCompanyWindow : Window
    {
        private Company _newCompany = new Company();
        public AddCompanyWindow(Company company)
        {
            InitializeComponent();

            if (company == null)
            {
                DataContext = _newCompany;
                return;
            }
            if (company.WebSite != null)
            {
                company.WebSite = company.WebSite.Trim();
            }
            AddCompany.Click -= AddConpany_Click;
            AddCompany.Click += RedactCompany_Click;
            DataContext = company;
            this.Title = "Редактирование компании";
        }

        private void AddConpany_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_newCompany.Adress))
                errors.AppendLine("Поле \"Адрес\" является обязательным");

            if (string.IsNullOrWhiteSpace(_newCompany.PhoneNumber))
                errors.AppendLine("Поле \"Номер телефона\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var comp = BDKREntities.GetContext().Company.Where(c=>c.Adress.Contains(_newCompany.Adress)).ToList();

            if (comp.Count != 0)
            {
                MessageBox.Show("Данная компанию уже есть в бд!");
                return;
            }

            BDKREntities.GetContext().Company.Add(_newCompany);


            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void RedactCompany_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            Company company = (Company)DataContext;

            if (string.IsNullOrWhiteSpace(company.Adress))
                errors.AppendLine("Поле \"Адрес\" является обязательным");

            if (string.IsNullOrWhiteSpace(company.PhoneNumber))
                errors.AppendLine("Поле \"Номер телефона\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var comp = BDKREntities.GetContext().Company.Where(c => c.Adress.Contains(_newCompany.Adress)).ToList();


            if (comp.Count != 0)
            {
                MessageBox.Show("Данная компанию уже есть в бд!");
                return;
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }
    }
}
