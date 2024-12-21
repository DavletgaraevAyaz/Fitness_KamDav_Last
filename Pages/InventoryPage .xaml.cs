using FitnessOsnova_Kam_Dav.DbModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FitnessOsnova_Kam_Dav.Pages
{
    public partial class InventoryPage : Page
    {
        private int gymId; // ID зала


        public InventoryPage(int gymId)
        {
            InitializeComponent();
            this.gymId = gymId; // Сохраняем переданный GymID
            LoadInventory();
            if (CurrentUser.UserRoleType == UserRoleEnum.Admin)
            {
                deleteBtn.Visibility = Visibility.Visible;
                editBtn.Visibility = Visibility.Visible;
            }
            else if (CurrentUser.UserRoleType == UserRoleEnum.Client)
            {
                deleteBtn.Visibility = Visibility.Hidden;
                editBtn.Visibility = Visibility.Hidden;
            }
            // Загружаем инвентарь для выбранного зала
        }

        private void LoadInventory()
        {
            var inventory = ConnectionClass.connect.Inventory
                .Where(i => i.GymID == gymId) // Фильтруем инвентарь по GymID
                .Select(i => new
                {
                    i.ItemName,
                    i.Quantity,
                    i.Condition,
                    i.LastMaintenanceDate
                })
                .ToList();

            InventoryListView.ItemsSource = inventory; // Отображаем инвентарь в ListView
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new GymsPage()); // Переходим назад на страницу с залами
        }

        private void Edit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Получаем выбранный элемент из списка
            var selectedItem = InventoryListView.SelectedItem;
            if (selectedItem != null)
            {
                // Здесь вы можете открыть окно редактирования
                // Например, передать данные в новое окно или диалог
                var itemToEdit = (dynamic)selectedItem; // Приводим к динамическому типу для доступа к свойствам
                                                        // Открыть окно редактирования (например, EditInventoryWindow)
                                                        // var editWindow = new EditInventoryWindow(itemToEdit);
                                                        // editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования.");
            }
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItem = InventoryListView.SelectedItem as Inventory;
            if (selectedItem != null)
            {
                var inventoryItem = ConnectionClass.connect.Inventory
                    .FirstOrDefault(i => i.ItemName == selectedItem.ItemName && i.GymID == gymId);

                if (inventoryItem != null)
                {
                    ConnectionClass.connect.Inventory.Remove(inventoryItem);
                    ConnectionClass.connect.SaveChanges();
                    LoadInventory();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
            }
        }
    }
}
