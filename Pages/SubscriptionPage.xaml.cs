// SubscriptionPage.xaml.cs

using FitnessOsnova_Kam_Dav.DbModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FitnessOsnova_Kam_Dav.Pages
{
    public partial class SubscriptionPage : Page
    {
        private int userId; // ID пользователя

        public SubscriptionPage(int id)
        {
            InitializeComponent();
            userId = id;
            LoadSubscriptionOptions();
        }

        // Загрузка доступных типов абонементов
        private void LoadSubscriptionOptions()
        {
            var subscriptionTypes = ConnectionClass.connect.MembershipTypes.ToList();
            SubscriptionComboBox.ItemsSource = subscriptionTypes;
            SubscriptionComboBox.DisplayMemberPath = "TypeName"; // Показываем имя типа абонемента
            SubscriptionComboBox.SelectedValuePath = "MembershipTypeID"; // ID типа абонемента
        }

        // Кнопка для покупки абонемента
        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSubscriptionType = (dynamic)SubscriptionComboBox.SelectedItem;

            if (selectedSubscriptionType == null)
            {
                MessageBox.Show("Выберите тип абонемента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Получаем клиента
                var client = ConnectionClass.connect.Clients
                    .FirstOrDefault(c => c.UserID == userId);

                if (client == null)
                {
                    MessageBox.Show("Клиент с таким ID не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем новый абонемент для клиента
                client.MembershipTypeID = selectedSubscriptionType.MembershipTypeID;

                // Добавляем новый абонемент в базу
                ConnectionClass.connect.SaveChanges();

                MessageBox.Show("Абонемент успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new TrainingsPage(userId));
                 // После оформления абонемента возвращаемся на страницу тренировок
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при оформлении абонемента: {ex.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка для возврата на страницу с тренировками
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainingsPage(userId));
        }
    }
}
