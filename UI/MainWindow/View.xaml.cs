using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using UI.Interfaces;
using Utils;

namespace UI.MainWindow
{
    public partial class View : IMainView
    {
        private readonly ViewModel viewModel;
        private bool editorHasChanges;

        private SolidColorBrush glowBrush;

        private Dictionary<Key, HotkeyDelegate> hotkeys;

        private string fileName;
        private string fullFileName;

        private readonly OpenFileDialog openFileDialog;
        private SolidColorBrush outputBrush;
        private readonly SaveFileDialog saveFileDialog;
        private SolidColorBrush sourceBrush;

        public View()
        {
            viewModel = new ViewModel(this);

            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();

            InitializeComponent();
            InitailizeAnimationBrushes();
            RegisterHotkeys();
            WireHandlers();
        }

        public string SourceCode
        {
            get
            {
                var document = SourceCodeBox.Document;
                var code = new TextRange(document.ContentStart, document.ContentEnd).Text;
                return code;
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
            LaunchButtonBox.Background = glowBrush;
            StopButtonBox.Background = new SolidColorBrush(Colors.Transparent);
            InterpreterBox.IsReadOnly = true;
            SourceCodeBox.IsReadOnly = false;
            SourceCodeBox.Focus();
        }

        public void ClearOutput()
            => OutputBox.Document.Blocks.Clear();

        public void PrintOutput(Color color, string text)
            => OutputBox.Document.AppendText(color, text);

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
            editorHasChanges = true;

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
            glowBrush = new SolidColorBrush();
            sourceBrush = new SolidColorBrush();
            outputBrush = new SolidColorBrush();

            glowBrush.Color = AppColors.ReadyAccent;
            sourceBrush.Color = AppColors.ActiveBoxBg;
            outputBrush.Color = AppColors.InactiveBoxBg;

            LaunchButtonBox.Background = glowBrush;
            SourceCodeWindow.Background = sourceBrush;
            OutputWindow.Background = outputBrush;

            RegisterName("glowBrush", glowBrush);
            RegisterName("sourceBrush", sourceBrush);
            RegisterName("outputBrush", outputBrush);

            var glowAnimation = ColorUtils.CreateColorAnimation(AppColors.ReadyAccent, AppColors.ReadyGlowAccent,
                "glowBrush", false, autoreverse: true, repeat: true, durationMs: 1500);
            var sb = new Storyboard();
            sb.Children.Add(glowAnimation);

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

        private void HighlightSyntax(bool clearHistory = false)
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

        private void NewFileButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PromptSavingCurrentFile())
            {
                fileName = null;
                fullFileName = null;
                ClearSourceCode();
                ClearOutput();
                UpdateFilename();
                editorHasChanges = false;
            }
        }

        private void ClearSourceCode()
        {
            SourceCodeBox.Document.Blocks.Clear();
            SourceCodeBox.Document.Blocks.Add(new Paragraph());
        }

        private void UpdateFilename()
        {
            Title = fileName ?? "Новый файл" + " - Logikek";
        }

        private void OpenFileButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!PromptSavingCurrentFile()) return;

            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog(this).Value)
            {
                fileName = openFileDialog.SafeFileName;
                fullFileName = openFileDialog.FileName;
                SourceCodeBox.Document.Blocks.Clear();
                ClearOutput();
                UpdateFilename();

                using (var reader = new StreamReader(openFileDialog.FileName, Encoding.Default))
                {
                    var code = reader.ReadToEnd();
                    SourceCodeBox.Document.AppendText(code);
                }

                HighlightSyntax(true);
                editorHasChanges = false;
            }
        }

        private bool PromptSavingCurrentFile()
        {
            if (!editorHasChanges) return true;
            var result = MessageBox.Show($"Сохранить файл {fileName}?",
                "Сохранение",
                MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return SaveFile();
                case MessageBoxResult.Cancel:
                    return false;
                default:
                    return true;
            }
        }

        private void SaveFileButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveFile();
        }

        private bool SaveFile()
        {
            try
            {
                if (string.IsNullOrEmpty(fullFileName))
                {
                    saveFileDialog.FileName = "";
                    if (saveFileDialog.ShowDialog(this).Value)
                    {
                        fileName = saveFileDialog.SafeFileName;
                        fullFileName = saveFileDialog.FileName;
                    }
                    else
                    {
                        return false;
                    }
                }

                using (var writer = new StreamWriter(fullFileName, false, Encoding.Default))
                {
                    writer.Write(SourceCode);
                }

                editorHasChanges = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения файла");
                return false;
            }
        }

        private delegate void HotkeyDelegate();
    }
}