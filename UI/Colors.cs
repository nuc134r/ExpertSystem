using System.Windows;
using System.Windows.Media;

namespace UI
{
    public static class AppColors
    {
        public static Color RunningAccent
        {
            get { return ColorUtils.HexColor("#CA5100"); }
        }

        public static Color ReadyAccent
        {
            get { return ColorUtils.HexColor("#008BE8"); }
        }

        public static Color ReadyGlowAccent
        {
            get { return ColorUtils.HexColor("#006BB3"); }
        }

        public static Color ActiveBoxBg
        {
            get { return ColorUtils.HexColor("#2B2B2B"); }
        }

        public static Color InactiveBoxBg
        {
            get { return ColorUtils.HexColor("#1D1D1D"); }
        }

        public static Color SaveAccent
        {
            get { return ColorUtils.HexColor("#7D8844"); }
        }

        public static Color SaveGlowAccent
        {
            get { return ColorUtils.HexColor("#657127"); }
        }
    }
    
    public static class SyntaxColors
    {
        public static Color Rule
        {
            get { return ColorUtils.HexColor("#E3E3BC"); }
        }

        public static Color Bracket
        {
            get { return ColorUtils.HexColor("#9C9C9C"); }
        }

        public static Color Atom
        {
            get { return ColorUtils.HexColor("#BC93CF"); }
        }

        public static Color Fact
        {
            get { return ColorUtils.HexColor("#BFC3F1"); }
        }

        public static Color Operator
        {
            get { return ColorUtils.HexColor("#CDFFFF"); }
        }

        public static Color Query
        {
            get { return ColorUtils.HexColor("#B56ED4"); }
        }

        public static Color Comment
        {
            get { return ColorUtils.HexColor("#57A64A"); }
        }

        public static Color Arguments
        {
            get { return Colors.White; }
        }

        public static Color Semicolon
        {
            get { return Colors.White; }
        }
    }
}