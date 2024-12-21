using FitnessOsnova_Kam_Dav.DbModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FitnessOsnova_Kam_Dav.Pages
{
    public partial class FeedbackPage : Page
    {
        private int selectedRating = 0;
        private int userId;

        public FeedbackPage(int id)
        {
            InitializeComponent();
            userId = id;
            LoadFeedback();
            InitializeStars();
        }

        private void LoadFeedback()
        {
                var feedbacks = ConnectionClass.connect.Trainers.ToList()
                    .Select(f => new
                    {
                        f.TrainerID,
                        f.FirstName,
                        f.LastName,
                        f.Rating
                        // Получаем дату без форматирования
                    })
                    .ToList();

            FeedbackListView.ItemsSource = feedbacks;
        }

        private void InitializeStars()
        {
            var stars = Enumerable.Range(1, 5).ToList();
            RatingStars.ItemsSource = stars;
        }

        private void StarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && int.TryParse(button.Tag.ToString(), out int rating))
            {
                selectedRating = rating;
                UpdateStarColors();
            }
        }

        private void UpdateStarColors()
        {
            foreach (var item in RatingStars.Items)
            {
                var button = RatingStars.ItemContainerGenerator.ContainerFromItem(item) as Button;
                if (button != null)
                {
                    button.Foreground = RatingStars.Items.IndexOf(item) < selectedRating ?
                        Brushes.Gold : Brushes.Gray;
                }
            }
        }

        private void AddFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var selTrainer = (dynamic)FeedbackListView.SelectedItem;
            var comment = CommentTextBox.Text;

            if (string.IsNullOrEmpty(comment) || selectedRating == 0)
            {
                MessageBox.Show("Пожалуйста, оставьте отзыв и выберите оценку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var context = ConnectionClass.connect.Feedback;

            var client = ConnectionClass.connect.Clients
                    .FirstOrDefault(c => c.UserID == userId);

            var newFeedback = new Feedback
                {
                    ClientID = client.ClientID, // Укажите ID текущего клиента
                    TrainerID = selTrainer.TrainerID, // Укажите ID тренера, если это необходимо
                    Rating = selectedRating,
                    Comment = comment,
                    FeedbackDate = DateTime.Now
                };

                ConnectionClass.connect.Feedback.Add(newFeedback);
            ConnectionClass.connect.SaveChanges(); // Сохранение изменений в базе данных
            

            MessageBox.Show("Ваш отзыв был успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadFeedback(); // Обновляем список отзывов
            CommentTextBox.Clear(); // Очищаем текстовое поле
            selectedRating = 0; // Сбрасываем выбранную оценку
            UpdateStarColors(); // Обновляем цвет звезд
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}