using FitnessOsnova_Kam_Dav.DbModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitnessOsnova_Kam_Dav.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataVisualizationPage.xaml
    /// </summary>
    public partial class DataVisualizationPage : Page
    {
        public DataVisualizationPage()
        {
            InitializeComponent();
            LoadCategories();
            UpdatePlot(); // Инициализация графика при загрузке
        }

        private void LoadCategories()
        {
            // Здесь можно загрузить категории, если это необходимо
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlot(); // Обновление графика при изменении выбора
        }

        private void UpdatePlot()
        {
            // Очищаем предыдущие элементы графика
            PlotCanvas.Children.Clear();

            if (CategoryComboBox.SelectedIndex == 0)
            {
                // График на основе данных из Campaigns
                var campaigns = ConnectionClass.connect.MembershipTypes.ToList();
                double maxBudget = (double)campaigns.Max(c => c.Price);
                double barWidth = 50; // Ширина столбца
                double spacing = 10; // Промежуток между столбцами

                for (int i = 0; i < campaigns.Count; i++)
                {
                    var campaign = campaigns[i];
                    double barHeight = ((Convert.ToDouble(campaign.Price) / maxBudget) * 200); // Нормализация высоты

                    Rectangle bar = new Rectangle
                    {
                        Width = barWidth,
                        Height = barHeight,
                        Fill = Brushes.Blue
                    };

                    Canvas.SetLeft(bar, i * (barWidth + spacing));
                    Canvas.SetBottom(bar, 0); // Устанавливаем основание столбца на ноль
                    PlotCanvas.Children.Add(bar);

                    // Добавляем подпись над столбцом
                    TextBlock label = new TextBlock
                    {
                        Text = campaign.Price.ToString(), // Замените на нужное свойство
                        Foreground = Brushes.Black,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = barWidth,
                        TextAlignment = TextAlignment.Center
                    };

                    Canvas.SetLeft(label, i * (barWidth + spacing));
                    Canvas.SetBottom(label, barHeight + 5); // Устанавливаем подпись над столбцом
                    PlotCanvas.Children.Add(label);
                }
            }
            else if (CategoryComboBox.SelectedIndex == 1)
            {
                // График на основе данных из Requests
                var requests = ConnectionClass.connect.FinancialTransactions
                    .GroupBy(r => r.Amount) // Группировка по статусу
                    .Select(g => new
                    {
                        Status = g.Key,
                        Count = g.Count()
                    }).ToList();

                double maxCount = requests.Max(r => r.Count);
                double barWidth = 50; // Ширина столбца
                double spacing = 10; // Промежуток между столбцами

                for (int i = 0; i < requests.Count; i++)
                {
                    var request = requests[i];
                    double barHeight = (request.Count / maxCount) * 200; // Нормализация высоты

                    Rectangle bar = new Rectangle
                    {
                        Width = barWidth,
                        Height = barHeight,
                        Fill = Brushes.Green
                    };

                    Canvas.SetLeft(bar, i * (barWidth + spacing));
                    Canvas.SetBottom(bar, 0); // Устанавливаем основание столбца на ноль
                    PlotCanvas.Children.Add(bar);

                    // Добавляем подпись над столбцом
                    TextBlock label = new TextBlock
                    {
                        Text = request.Count.ToString(), // Замените на нужное свойство
                        Foreground = Brushes.Black,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = barWidth,
                        TextAlignment = TextAlignment.Center
                    };
                    Canvas.SetLeft(label, i * (barWidth + spacing));
                    Canvas.SetBottom(label, barHeight + 5); // Устанавливаем подпись над столбцом
                    PlotCanvas.Children.Add(label);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}


