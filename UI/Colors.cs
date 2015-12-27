using System.Windows;
using System.Windows.Media;

namespace UI
{
    public static class AppColors
    {
        public static Color RunningAccent => ColorUtils.HexColor("#CA5100");
        public static Color ReadyAccent => ColorUtils.HexColor("#008BE8");
        public static Color ReadyGlowAccent => ColorUtils.HexColor("#006BB3");
        public static Color ActiveBoxBg => ColorUtils.HexColor("#2B2B2B");
        public static Color InactiveBoxBg => ColorUtils.HexColor("#1D1D1D");
    }
    
    public static class SyntaxColors
    {
        public static Color Rule => ColorUtils.HexColor("#E3E3BC");
        public static Color Bracket => ColorUtils.HexColor("#9C9C9C");
        public static Color Atom => ColorUtils.HexColor("#BC93CF");
        public static Color Fact => ColorUtils.HexColor("#BFC3F1");
        public static Color Operator => ColorUtils.HexColor("#CDFFFF");
        public static Color Query => ColorUtils.HexColor("#B56ED4");
        public static Color Comment => ColorUtils.HexColor("#57A64A");
        public static Color Arguments => Colors.White;
        public static Color Semicolon => Colors.White;
    }
}