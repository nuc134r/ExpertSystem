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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRunning;

        private static readonly Color RunningColor = (Color)ColorConverter.ConvertFromString("#CA5100");
        private static readonly Color ReadyColor = SystemColors.MenuHighlightColor;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SourceCodeBox_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            LineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void SourceCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var codeText = new TextRange(SourceCodeBox.Document.ContentStart, SourceCodeBox.Document.ContentEnd).Text;
                var linesCount = codeText.Split('\n').Length;

                LineNumbersBox.Document.Blocks.Clear();

                for (var i = 0; i < linesCount - 1; i++)
                {
                    LineNumbersBox.Document.Blocks.Add(new Paragraph(new Run((i + 1).ToString())));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LaunchStopButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isRunning = !isRunning;

            if (isRunning)
            {
                statusBar.Background = LaunchStopBox.Background = new SolidColorBrush(RunningColor);
                LaunchStopButton.Content = "  Стоп";
                statusBarInfo.Content = "Отладка";
            }
            else
            {
                statusBar.Background = LaunchStopBox.Background = new SolidColorBrush(ReadyColor);
                LaunchStopButton.Content = "Запуск";
                statusBarInfo.Content = "Готов";
            }
        }
    }
}
