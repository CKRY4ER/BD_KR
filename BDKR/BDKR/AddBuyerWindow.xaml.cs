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
    /// Логика взаимодействия для AddBuyerWindow.xaml
    /// </summary>
    public partial class AddBuyerWindow : Window
    {
        private Buyer _buyer = new Buyer();
        private Passport _passport = new Passport();
        public AddBuyerWindow()
        {
            InitializeComponent();
            DataContext = _buyer;
            _buyer.Passport = new Passport();
        }

        private void SaveChange_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(SerialPassportTb.Text))
                errors.AppendLine("Поле \"Cерия пасспорта\" является обязательным");
            else
                _passport.Serial = int.Parse(SerialPassportTb.Text);

            if (string.IsNullOrWhiteSpace(NumberPassportTb.Text))
                errors.AppendLine("Поле \"Номер пасспорта\" является обязательным");
            else
                _passport.Number = int.Parse(NumberPassportTb.Text);

            if (string.IsNullOrWhiteSpace(DateOfIssueTb.Text))
                errors.AppendLine("Поле \"Дата выдачи пасспорта\" является обязательным");
            else
                _passport.DateOfIssue = DateTime.Parse(DateOfIssueTb.Text);

            if (string.IsNullOrWhiteSpace(IssuedByWhom.Text))
                errors.AppendLine("Поле \"Кем выдан пасспорт\" является обязательным");
            else
                _passport.IssuedByWhom = IssuedByWhom.Text;

            if (string.IsNullOrWhiteSpace(_buyer.Name))
                errors.AppendLine("Поле \"Имя\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Surname))
                errors.AppendLine("Поле \"Фамилия\" является обязательным");

            if (string.IsNullOrWhiteSpace(Birthday.Text))
                errors.AppendLine("Поле \"Дата рождения\" является обязательным");
            else
                _buyer.Birthday = DateTime.Parse(Birthday.Text);

            if (string.IsNullOrWhiteSpace(_buyer.PhoneNumber))
                errors.AppendLine("Поле \"Номер телефона\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            List<Passport> passports = BDKREntities.GetContext().Passport
                .Where(p => p.Serial == _passport.Serial)
                .Where(p => p.Number == _passport.Number).ToList();

            if (passports.Count != 0) 
            {
                MessageBox.Show("Не возможно добавить. Введенные паспортные данные принадлежат другому человеку!");
                return;
            }

            if (_buyer.Passport.PassportID == 0)
            {
                BDKREntities.GetContext().Passport.Add(_buyer.Passport);
            }

            if (_buyer.BuyerId == 0)
            {
                _buyer.PassportId = _buyer.Passport.PassportID;
                BDKREntities.GetContext().Buyer.Add(_buyer);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
            }
        }
    }
    
}
