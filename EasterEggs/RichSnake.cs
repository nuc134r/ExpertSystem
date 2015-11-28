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
        private const char boneSym = '@';
        private const char foodSym = '$';

        private int length = 4;

        private int ax = +1;
        private int ay = 0;

        private SnakeBone[] body;

        private int width;
        private int height;

        private char[,] field;
        private SnakeBone food;

        private TextPointer pointer;

        private DispatcherTimer timer;

        private SnakeBone head => body[0];
        private SnakeBone tail => body[length - 1];

        public RichSnake(RichTextBox box)
        {
            InitializeField(box);
            InitializeBody();
            GenerateFood();

            Run();
        }

        private void GenerateFood()
        {
            var rnd = new Random(DateTime.Now.Millisecond);

            var x = rnd.Next(2, width - 2);
            var y = rnd.Next(2, height - 2);

            for (var i = 0; i < length; i++)
            {
                if (body[i].x != x && body[i].y != y) continue;
                GenerateFood();
                return;
            }

            food = new SnakeBone(x, y);

            SetSymbol(foodSym, food.x, food.y);
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
                SetSymbol(boneSym, body[i].x, body[i].y);
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
            if (head.x == food.x && head.y == food.y)
            {
                body[length] = new SnakeBone(tail.x, tail.y);
                length++;
                GenerateFood();
            }
            else
                SetSymbol(' ', tail.x, tail.y);

            for (var i = length - 1; i > 0; i--)
            {
                body[i].x = body[i - 1].x;
                body[i].y = body[i - 1].y;
            }

            head.x += ax;
            head.y += ay;
            try
            {
                SetSymbol(boneSym, head.x, head.y);
            }
            catch (Exception)
            {
                timer.Stop();
            }
        }

        private void SetSymbol(char symbol, int x, int y)
        {
            var pos1 = pointer.GetPositionAtOffset(width*(y - 1) + x - 1);
            var pos2 = pointer.GetPositionAtOffset(width*(y - 1) + x);

            new TextRange(pos1, pos2).Text = symbol.ToString();
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

            for (var i = 0; i < width*height; i++) buff += " ";

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