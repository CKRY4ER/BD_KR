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
    /// Логика взаимодействия для RedactBuyerWindow.xaml
    /// </summary>
    public partial class RedactBuyerWindow : Window
    {
        private Buyer _buyer;
        public RedactBuyerWindow(Buyer buyer)
        {
            InitializeComponent();
            DeleteSpace(buyer);
            _buyer = buyer;
            DataContext = _buyer;

            DateOfIssueTb.Text = _buyer.Passport.DateOfIssue.ToString();

            Birthday.Text = _buyer.Birthday.ToString();
        }

        private void DeleteSpace(Buyer buyer)
        {
            buyer.Passport.IssuedByWhom = buyer.Passport.IssuedByWhom.Trim();
            buyer.Name = buyer.Name.Replace(" ", "");
            buyer.Surname = buyer.Surname.Replace(" ", "");
            buyer.MiddleName = buyer.MiddleName?.Replace(" ", "");

            buyer.PhoneNumber = buyer.PhoneNumber.Replace(" ", "");
            buyer.CardNumber = buyer.CardNumber?.Replace(" ", "");
        }

        private void SaveChange_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_buyer.Passport.Serial.ToString()))
                errors.AppendLine("Поле \"Cерия пасспорта\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Passport.Number.ToString()))
                errors.AppendLine("Поле \"Номер пасспорта\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Passport.DateOfIssue.ToString()))
                errors.AppendLine("Поле \"Дата выдачи пасспорта\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Passport.IssuedByWhom.ToString()))
                errors.AppendLine("Поле \"Кем выдан пасспорт\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Name.ToString()))
                errors.AppendLine("Поле \"Имя\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Surname.ToString()))
                errors.AppendLine("Поле \"Фамилия\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.Birthday.ToString()))
                errors.AppendLine("Поле \"Дата рождения\" является обязательным");

            if (string.IsNullOrWhiteSpace(_buyer.PhoneNumber.ToString()))
                errors.AppendLine("Поле \"Номер телефона\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            //List<Passport> passports = BDKREntities.GetContext().Passport
            //  .Where(p => p.Serial == _buyer.Passport.Serial)
            //  .Where(p => p.Number == _buyer.Passport.Number).ToList();
            List<Buyer> buyers = BDKREntities.GetContext().Buyer
                .Where(b=>b.Passport.Serial == _buyer.Passport.Serial)
                .Where(b=>b.Passport.Number == _buyer.Passport.Number)
                .ToList();

            if (buyers.Count != 0)
            {
                if (_buyer.BuyerId != buyers[0].BuyerId)
                {
                    MessageBox.Show("Не возможно отредактировать. Введенные паспортные данные принадлежат другому человеку!");
                    return;
                }
            }

            _buyer.Passport.DateOfIssue = DateTime.Parse(DateOfIssueTb.Text);
            _buyer.Birthday = DateTime.Parse(Birthday.Text);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
            }
        }
    }
}
