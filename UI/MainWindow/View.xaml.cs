using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UI.Interfaces;
using Utils;

namespace UI.MainWindow
{
    public partial class View : IMainView
    {
        private readonly IMainViewModel viewModel;

        private Dictionary<Key, HotkeyDelegate> hotkeys;

        private SolidColorBrush launchGlowBrush;
        private SolidColorBrush outputBrush;
        private SolidColorBrush sourceBrush;

        public View()
        {
            InitializeComponent();

            viewModel = new ViewModel(this, this);

            InitailizeAnimationBrushes();
            RegisterHotkeys();
            WireHandlers();
        }

        public string SourceCodeText
        {
            get
            {
                var document = SourceCodeBox.Document;
                var code = new TextRange(document.ContentStart, document.ContentEnd).Text;
                return code;
            }
            set
            {
                ClearSourceCode(true);
                SourceCodeBox.Document.AppendText(value);
            }
        }

        public void IndicateLaunch()
        {
            HighlightSyntax();
            AnimateModeChange(ApplicationMode.Running);
            LaunchButtonBox.Background = new SolidColorBrush(Colors.Transparent);
            StopButtonBox.Background = new SolidColorBrush(AppColors.RunningAccent);
            SourceCodeBox.IsReadOnly = true;
            InterpreterBox.IsReadOnly = false;
            InterpreterBox.Focus();
        }

        public void IndicateErrorsOccurence()
        {
            var animation = ColorUtils.CreateColorAnimation(AppColors.InactiveBoxBg, Color.FromRgb(149, 0, 0),
                "outputBrush", true, autoreverse: true);

            var sb = new Storyboard();
            sb.Children.Add(animation);

            sb.Begin(this);
        }

        public void IndicateStop()
        {
            InterpreterBox.Text = "";

            AnimateModeChange(ApplicationMode.Ready);
            LaunchButtonBox.Background = launchGlowBrush;
            StopButtonBox.Background = new SolidColorBrush(Colors.Transparent);
            InterpreterBox.IsReadOnly = true;
            SourceCodeBox.IsReadOnly = false;
            SourceCodeBox.Focus();
        }

        public void ClearOutput()
            => OutputBox.Document.Blocks.Clear();

        public void PrintOutput(Color color, string text)
            => OutputBox.Document.AppendText(color, text);

        public void ClearSourceCode(bool deep = false)
        {
            SourceCodeBox.Document.Blocks.Clear();
            if (!deep)
            {
                SourceCodeBox.Document.Blocks.Add(new Paragraph());
            }
        }

        public void UpdateFilename(string fileName)
        {
            Title = (fileName ?? "Новый файл") + " - Logikek";
        }

        public void HighlightSyntax(bool clearHistory = false)
        {
            using (SourceCodeBox.DeclareChangeBlock())
            {
                viewModel.Format(SourceCodeBox.Document);
            }

            if (clearHistory)
            {
                SourceCodeBox.IsUndoEnabled = false;
                SourceCodeBox.IsUndoEnabled = true;
            }
        }

        private void RegisterHotkeys()
        {
            hotkeys = new Dictionary<Key, HotkeyDelegate>
            {
                {
                    Key.F5, () => { viewModel.StartStop(); }
                },
                {
                    Key.Enter, () =>
                    {
                        if (!InterpreterBox.IsFocused) return;
                        viewModel.Evaluate(InterpreterBox.Text);
                        InterpreterBox.Text = "";
                    }
                },
                {
                    Key.Up, () =>
                    {
                        if (InterpreterBox.IsFocused)
                            InterpreterBox.Undo();
                    }
                },
                {
                    Key.Down, () =>
                    {
                        if (InterpreterBox.IsFocused)
                            InterpreterBox.Redo();
                    }
                }
            };
        }

        private void SourceCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var document = SourceCodeBox.Document;
            var code = new TextRange(document.ContentStart, document.ContentEnd).Text;
            var linesCount = code.Lines();

            if (LineNumbersBox.Text.Lines() == linesCount) return;

            LineNumbersBox.Text = "";
            for (var i = 0; i < linesCount - 1; i++)
            {
                LineNumbersBox.Text += i + 1 + "\n";
            }
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (hotkeys.ContainsKey(e.Key))
            {
                hotkeys[e.Key].Invoke();
            }
        }

        private void SourceCodeBox_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            // В исходный код нельзя вставлять картинки
            if (e.FormatToApply == "Bitmap")
            {
                e.CancelCommand();
            }
        }

        private void WireHandlers()
        {
            SourceCodeBox.TextChanged += SourceCodeBox_TextChanged;
        }

        private void AnimateModeChange(ApplicationMode mode)
        {
            var isReversed = mode == ApplicationMode.Running;

            var sourceAnimation = ColorUtils.CreateColorAnimation(AppColors.ActiveBoxBg, AppColors.InactiveBoxBg,
                "sourceBrush", isReversed);
            var outputAnimation = ColorUtils.CreateColorAnimation(AppColors.InactiveBoxBg, AppColors.ActiveBoxBg,
                "outputBrush", isReversed);

            var sb = new Storyboard();
            sb.Children.Add(sourceAnimation);
            sb.Children.Add(outputAnimation);

            sb.Begin(this);
        }

        private void InitailizeAnimationBrushes()
        {
            launchGlowBrush = new SolidColorBrush();
            sourceBrush = new SolidColorBrush();
            outputBrush = new SolidColorBrush();

            launchGlowBrush.Color = AppColors.ReadyAccent;
            sourceBrush.Color = AppColors.ActiveBoxBg;
            outputBrush.Color = AppColors.InactiveBoxBg;

            LaunchButtonBox.Background = launchGlowBrush;
            SourceCodeWindow.Background = sourceBrush;
            OutputWindow.Background = outputBrush;

            RegisterName("launchGlowBrush", launchGlowBrush);
            RegisterName("sourceBrush", sourceBrush);
            RegisterName("outputBrush", outputBrush);

            var launchGlowAnimation = ColorUtils.CreateColorAnimation(AppColors.ReadyAccent, AppColors.ReadyGlowAccent,
                "launchGlowBrush", false, autoreverse: true, repeat: true, durationMs: 1500);
            var sb = new Storyboard();
            sb.Children.Add(launchGlowAnimation);

            sb.Begin(this);
        }

        private void LaunchButton_MouseDown(object sender, MouseButtonEventArgs e)
            => viewModel.Launch();

        private void SourceCodeBox_OnScrollChanged(object sender, ScrollChangedEventArgs e)
            => LineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset);

        private void StopButton_OnMouseDown(object sender, MouseButtonEventArgs e)
            => viewModel.Stop();

        private void OutputBox_OnTextChanged(object sender, TextChangedEventArgs e)
            => OutputBox.ScrollToEnd();

        private void ClearOutputButton_OnMouseDown(object sender, MouseButtonEventArgs e)
            => ClearOutput();

        private void FormatCode_OnMouseDown(object sender, MouseButtonEventArgs e)
            => HighlightSyntax();

        private void NewFileButton_OnMouseDown(object sender, MouseButtonEventArgs e)
            => viewModel.NewFile();

        private void OpenFileButton_OnMouseDown(object sender, MouseButtonEventArgs e)
            => viewModel.OpenFile();

        private void SaveFileButton_OnMouseDown(object sender, MouseButtonEventArgs e)
            => viewModel.SaveFile();

        private delegate void HotkeyDelegate();
    }
}