using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
namespace AttendanceApp
{
    public partial class MainWindow : Window
    {
        private AttendanceData attendanceData;
        private int currentDay;
        public MainWindow()
        {
            InitializeComponent();
            attendanceData = new AttendanceData();
            currentDay = 1;
            UpdateInfoText();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDay > 31)
            {
                MessageBox.Show("Вы ввели данные для всех 31 дня месяца.");
                return;
            }
            if (int.TryParse(AttendanceTextBox.Text, out int attendance) && attendance >= 0)
            {
                attendanceData.SetAttendance(currentDay, attendance);
                DrawGraph();
                AttendanceTextBox.Clear();
                currentDay++; // Переход к следующему дню
                UpdateInfoText();
                UpdateMinMaxDays();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное число посещений.");
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            attendanceData = new AttendanceData(); // Создаем новый экземпляр AttendanceData
            currentDay = 1; // Сброс текущего дня
            AttendanceCanvas.Children.Clear(); // Очистка графика
            AttendanceTextBox.Clear(); // Очистка текстового поля
            UpdateInfoText(); // Обновление информации
            MinMaxTextBlock.Text = ""; // Очистка текста минимального и максимального дней
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Завершение приложения
        }
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программа для учета посещаемости.\nВариант №6.\nЖурина А.А.\nИСП-32", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void DrawGraph()
        {
            AttendanceCanvas.Children.Clear();
            double canvasWidth = AttendanceCanvas.ActualWidth;
            double canvasHeight = AttendanceCanvas.ActualHeight;
            var attendanceList = attendanceData.GetAttendanceList();
            if (attendanceList.Count == 0) return;
            double maxAttendance = Math.Max(1, attendanceList.Max());
            double pointWidth = canvasWidth / (attendanceList.Count - 1); // Расстояние между точками
            Point previousPoint = new Point(0, canvasHeight - (attendanceList[0] / maxAttendance) * (canvasHeight - 20));
            for (int i = 0; i < attendanceList.Count; i++)
            {
                // Вычисляем координаты текущей точки
                double x = i * pointWidth;
                double y = canvasHeight - (attendanceList[i] / maxAttendance) * (canvasHeight - 20);
                Point currentPoint = new Point(x, y);
                // Рисуем линию от предыдущей точки к текущей
                if (i > 0)
                {
                    Line line = new Line
                    {
                        X1 = previousPoint.X,
                        Y1 = previousPoint.Y,
                        X2 = currentPoint.X,
                        Y2 = currentPoint.Y,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 2
                    };
                    AttendanceCanvas.Children.Add(line);
                }
                // Рисуем круг для текущей точки
                Ellipse pointEllipse = new Ellipse
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.Red
                };
                Canvas.SetLeft(pointEllipse, x - pointEllipse.Width / 2);
                Canvas.SetBottom(pointEllipse, y - pointEllipse.Height / 2);
                AttendanceCanvas.Children.Add(pointEllipse);
                // Добавляем текст с количеством посещений
                TextBlock attendanceTextBlock = new TextBlock
                {
                    Text = attendanceList[i].ToString(),
                    Foreground = Brushes.Black,
                    FontSize = 12
                };
                Canvas.SetLeft(attendanceTextBlock, x - attendanceTextBlock.ActualWidth / 2);
                Canvas.SetBottom(attendanceTextBlock, y + 5); // Сдвигаем текст немного выше точки
                AttendanceCanvas.Children.Add(attendanceTextBlock);
                previousPoint = currentPoint; // Обновляем предыдущую точку
            }
        }
        private void UpdateInfoText()
        {
            InfoTextBlock.Text = $"День: {currentDay}\nВведено посещаемости: {currentDay - 1} из 31.";
        }
        private void UpdateMinMaxDays()
        {
            var attendanceList = attendanceData.GetAttendanceList();

            if (attendanceList.Count == 0)
            {
                MinMaxTextBlock.Text = "Нет данных о посещаемости.";
                return;
            }
            int maxAttendance = attendanceList.Max();
            int minAttendance = attendanceList.Min();
            List<int> maxDays = new List<int>();
            List<int> minDays = new List<int>();
            for (int i = 0; i < attendanceList.Count; i++)
            {
                if (attendanceList[i] == maxAttendance)
                {
                    maxDays.Add(i + 1); // +1 для отображения дня (1-индексация)
                }
                if (attendanceList[i] == minAttendance)
                {
                    minDays.Add(i + 1); // +1 для отображения дня (1-индексация)
                }
            }
            string maxDaysOutput = maxDays.Count > 1 ? string.Join(", ", maxDays) + " дней" : $"{maxDays[0]} день";
            string minDaysOutput = minDays.Count > 1 ? string.Join(", ", minDays) + " дней" : $"{minDays[0]} день";
            MinMaxTextBlock.Text = $"Максимальное количество посещений: {maxAttendance} ({maxDaysOutput})\n" +
                                    $"Минимальное количество посещений: {minAttendance} ({minDaysOutput})";
        }

    }
}
