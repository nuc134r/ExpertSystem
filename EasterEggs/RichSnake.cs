using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace EasterEggs
{
    internal class SnakeBone
    {
        public int x;
        public int y;

        public SnakeBone(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class RichSnake
    {
        private readonly int length = 4;
        private int ax = +1;
        private int ay;

        private SnakeBone[] body;

        private readonly char bone = '@';
        private char[,] field;
        private int height;
        private TextPointer pointer;

        private DispatcherTimer timer;
        private int width;

        public RichSnake(RichTextBox box)
        {
            InitializeField(box);
            InitializeBody();

            Run();
        }

        public void SetDirection(int dir)
        {
            switch (dir)
            {
                case 1:
                    ax = 0;
                    ay = -1;
                    break;
                case 2:
                    ax = +1;
                    ay = 0;
                    break;
                case 3:
                    ax = 0;
                    ay = +1;
                    break;
                case 4:
                    ax = -1;
                    ay = 0;
                    break;
            }
        }

        private void InitializeBody()
        {
            body = new SnakeBone[width*height];

            body[0] = new SnakeBone(4, 1);
            body[1] = new SnakeBone(3, 1);
            body[2] = new SnakeBone(2, 1);
            body[3] = new SnakeBone(1, 1);

            for (var i = 0; i < length; i++)
                SetSymbol(bone, body[i].x, body[i].y);
        }

        private void Run()
        {
            timer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher.CurrentDispatcher)
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };

            timer.Tick += Tick;
            timer.IsEnabled = true;
        }

        private void Tick(object sender, EventArgs e)
        {
            SetSymbol(' ', body[length - 1].x, body[length - 1].y);

            for (var i = length - 1; i > 0; i--)
            {
                body[i].x = body[i - 1].x;
                body[i].y = body[i - 1].y;
            }

            body[0].x += ax;
            body[0].y += ay;

            SetSymbol(bone, body[0].x, body[0].y);
        }

        private void SetSymbol(char symbol, int x, int y)
        {
            var pos1 = pointer.GetPositionAtOffset(width*(y - 1) + x - 1);
            var pos2 = pointer.GetPositionAtOffset(width*(y - 1) + x);

            new TextRange(pos1, pos2).Text = symbol.ToString();
        }

        private void SetSymbol(char symbol, int x, int y, Color color)
        {
            var pos1 = pointer.GetPositionAtOffset(width*(y - 1) + x - 1);
            var pos2 = pointer.GetPositionAtOffset(width*(y - 1) + x);

            var text = new TextRange(pos1, pos2) {Text = symbol.ToString()};

            text.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }

        private void InitializeField(RichTextBox box)
        {
            var typicalSymbol = new FormattedText(
                " ",
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(box.FontFamily, box.FontStyle, box.FontWeight, box.FontStretch),
                box.FontSize,
                Brushes.Black);

            width = (int) (box.ActualWidth/typicalSymbol.WidthIncludingTrailingWhitespace) - 1;
            height = (int) (box.ActualHeight/typicalSymbol.Height);

            field = new char[width, height];

            var buff = "";

            for (var i = 0; i < width*height; i++)
                buff += " ";

            box.AppendText(buff);

            var document = box.Document;

            pointer = document.ContentStart;
            while (pointer?.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            {
                pointer = pointer?.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        public void Stop()
        {
            timer.IsEnabled = false;
        }
    }
}