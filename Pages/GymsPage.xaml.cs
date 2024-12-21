using FitnessOsnova_Kam_Dav.DbModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FitnessOsnova_Kam_Dav.Pages
{
    public partial class GymsPage : Page
    {
        public GymsPage()
        {
            InitializeComponent();
            LoadGyms();
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
        }

        private void LoadGyms()
        {
            using (var context = new FitnessClub_Kam_DavEntities())
            {
                var gyms = context.Gyms
                    .Select(g => new
                    {
                        g.GymID,
                        g.GymName,
                        g.Location,
                        g.Facilities,
                        g.Capacity
                    })
                    .ToList();

                GymsListView.ItemsSource = gyms;
            }
        }
        private void ShowEquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGym = (dynamic)GymsListView.SelectedItem;

            if (selectedGym == null)
            {
                MessageBox.Show("Выберите зал для отображения инвентаря", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Передаем GymID на страницу инвентаря
            NavigationService.Navigate(new InventoryPage(selectedGym.GymID));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainingsPage(CurrentUser.ID));
        }

        private void EditGymButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGym = (dynamic)GymsListView.SelectedItem;

            if (selectedGym == null)
            {
                MessageBox.Show("Выберите зал для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            NavigationService.Navigate(new EditGymPage(selectedGym.GymID));
        }

        private void DeleteGymButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGym = (dynamic)GymsListView.SelectedItem;

            if (selectedGym == null)
            {
                MessageBox.Show("Выберите зал для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этот зал?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new FitnessClub_Kam_DavEntities())
                {
                    var gymToDelete = context.Gyms.Find(selectedGym.GymID);
                    if (gymToDelete != null)
                    {
                        context.Gyms.Remove(gymToDelete);
                        context.SaveChanges();
                        MessageBox.Show("Зал успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadGyms(); // Обновляем список залов
                    }
                    else
                    {
                        MessageBox.Show("Зал не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}