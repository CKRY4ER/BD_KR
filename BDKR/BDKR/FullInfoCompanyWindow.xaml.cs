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
    /// Логика взаимодействия для FullInfoCompanyWindow.xaml
    /// </summary>
    public partial class FullInfoCompanyWindow : Window
    {
        public FullInfoCompanyWindow(Company company)
        {
            InitializeComponent();

            EmployeeGrid.ItemsSource = BDKREntities.GetContext().Employee
                .Where(emp => emp.Post.Company.CompanyId == company.CompanyId).ToList();

            InicializeStoregRoomGrid();
        }

        private void StoregeRoomGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void InicializeStoregRoomGrid()
        {
            var storegList = BDKREntities.GetContext().StoregeRoom.ToList();

            List<StoregeRoom> storegeRooms = new List<StoregeRoom>();

            StoregeRoom room = new StoregeRoom() { StorageRoomId = 0 };

            foreach (StoregeRoom storeg in storegList)
            {
                if (storeg.StorageRoomId != room.StorageRoomId)
                {
                    room = storeg;
                    storegeRooms.Add(room);
                }
            }

            StoregeRoomGrid.ItemsSource = storegeRooms;
        }
    }
}
