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
    /// Логика взаимодействия для FullInfoSypply.xaml
    /// </summary>
    public partial class FullInfoSypply : Window
    {
        public FullInfoSypply(Sypply sypply)
        {
            InitializeComponent();
            DataContext = sypply;
            ProductDataGrid.ItemsSource = sypply.SypplyPosition;
        }
    }
}
