using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UI
{
    public static class ColorUtils
    {
        public static Color HexColor(string str)
        {
            return (Color) ColorConverter.ConvertFromString(str);
        }

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