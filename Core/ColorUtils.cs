using System.Windows.Media;

namespace Core
{
    public static class ColorUtils
    {
        public static Color HexColor(string str) => (Color)ColorConverter.ConvertFromString(str);
    }
}