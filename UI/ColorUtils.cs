using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UI
{
    public static class ColorUtils
    {
        /// <summary>
        /// Получить цвет по шестнадцатеричному представлению
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Color HexColor(string str)
        {
            return (Color) ColorConverter.ConvertFromString(str);
        }

        /// <summary>
        /// Создание анимации цвета
        /// </summary>
        /// <param name="from">Исходный цвет</param>
        /// <param name="to">Целевой цвет</param>
        /// <param name="targetName">Имя Brush</param>
        /// <param name="reversed">Наоборот</param>
        /// <param name="durationMs">Продолжительность</param>
        /// <param name="autoreverse">Авторазворот</param>
        /// <param name="repeat">Повтор</param>
        /// <returns></returns>
        public static ColorAnimation CreateColorAnimation(Color from, Color to, string targetName, bool reversed,
            int durationMs = 175, bool autoreverse = false, bool repeat = false)
        {
            var duration = TimeSpan.FromMilliseconds(durationMs);
            var animation = new ColorAnimation(reversed ? @from : to, reversed ? to : @from, duration)
            {
                AutoReverse = autoreverse
            };

            if (repeat) animation.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard.SetTargetName(animation, targetName);
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));

            return animation;
        }
    }
}