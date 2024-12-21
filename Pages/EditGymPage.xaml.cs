using FitnessOsnova_Kam_Dav.DbModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FitnessOsnova_Kam_Dav.Pages
{
    public partial class EditGymPage : Page
    {
        private int _gymId;


        public EditGymPage(int gymId)
        {
            InitializeComponent();
            _gymId = gymId;
            LoadGymData();
        }

        private void LoadGymData()
        {
            using (var context = new FitnessClub_Kam_DavEntities())
            {
                var gym = context.Gyms.Find(_gymId);
                if (gym != null)
                {
                    GymNameTextBox.Text = gym.GymName;
                    LocationTextBox.Text = gym.Location;
                    FacilitiesTextBox.Text = gym.Facilities;
                    CapacityTextBox.Text = gym.Capacity.ToString();
                }
                else
                {
                    MessageBox.Show("Зал не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FitnessClub_Kam_DavEntities())
            {
                var gym = context.Gyms.Find(_gymId);
                if (gym != null)
                {
                    gym.GymName = GymNameTextBox.Text;
                    gym.Location = LocationTextBox.Text;
                    gym.Facilities = FacilitiesTextBox.Text;
                    gym.Capacity = int.TryParse(CapacityTextBox.Text, out int capacity) ? capacity : 0;

                    context.SaveChanges();
                    MessageBox.Show("Зал успешно обновлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    //NavigationService.GoBack();
                    NavigationService.Navigate(new GymsPage());
                    // Возвращаемся на предыдущую страницу
                }
                else
                {
                    MessageBox.Show("Зал не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // Возвращаемся на предыдущую страницу
        }
    }
}