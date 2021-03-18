using UnityEngine;
    public static class ColorUtil {
        public static Color FromRGB( byte r, byte g, byte b )
        {
            return new Color( r / 255f, g / 255f, b / 255f );
        }
        public static Color FromRGB( string htmlString )
        {
            ColorUtility.TryParseHtmlString( htmlString, out var color );
            return color;
        }
    }
