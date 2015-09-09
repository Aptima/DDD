using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;



/// <summary>
/// NamedColorDialog extends the standard color dialog bu restricting all color choices to the list of named colors.
/// System colors (i.e.'Button,' 'Window,') are not included in the list but could be.
/// </summary>
public class NamedColorDialog
{


    private System.Windows.Forms.ColorDialog innerDialog=new System.Windows.Forms.ColorDialog();
    private List<int> customColors = new List<int>();
    public Color ChosenColor = Color.Black;


    //Find the closest color to the one chosen
    private Color ClosestColor(Color custom)
    {
        double minDistance = double.MaxValue;
        Color minColor=Color.Black;
        double blue,green,red,alpha;
        double customBlue = (double)(custom.B);
        double customGreen = (double)(custom.G);
        double customRed = (double)(custom.R);
        double customAlpha = (double)(custom.A);
        foreach (Color c in namedColors)
        {   
             blue = (double)(c.B);
         green = (double)(c.G);
             red = (double)(c.R);
             alpha = (double)c.A;
            double distance = Math.Pow (blue - customBlue,2.0)+ Math.Pow(green-customGreen,2.0)
            + Math.Pow(red-customRed,2.0) + Math.Pow(alpha-customAlpha,2.0);
        if (distance < minDistance)
        {
            minDistance = distance;
            minColor = c;
        }
 
   }
       return minColor;
    }

	public NamedColorDialog()
	{
        innerDialog.AllowFullOpen = true;
        innerDialog.SolidColorOnly = false;
        innerDialog.AnyColor = true;
	}
    public DialogResult ShowDialog(IWin32Window owner)
    {
        customColors.CopyTo(innerDialog.CustomColors);
        DialogResult returnValue = innerDialog.ShowDialog();
        if (DialogResult.OK == returnValue)
        {
            ChosenColor =  ClosestColor( innerDialog.Color);
            customColors.Clear();
            for (int i = 0; i < innerDialog.CustomColors.Length; i++)
                customColors.Add(innerDialog.CustomColors[i]);
        }
        return returnValue;
    }

    private static Color[] namedColors = {
Color.AliceBlue, Color.AntiqueWhite, Color.Aqua, Color.Aquamarine,
Color.Azure, Color.Beige, Color.Bisque, Color.Black, Color.BlanchedAlmond,
Color.Blue, Color.BlueViolet, Color.Brown, Color.BurlyWood,
Color.CadetBlue, Color.Chartreuse, Color.Chocolate, Color.Coral, Color.CornflowerBlue,
Color.Cornsilk, Color.Crimson, Color.Cyan, Color.DarkBlue,
Color.DarkCyan, Color.DarkGoldenrod, Color.DarkGray, Color.DarkGreen, Color.DarkKhaki,
Color.DarkMagenta, Color.DarkOliveGreen, Color.DarkOrange, Color.DarkOrchid,
Color.DarkRed, Color.DarkSalmon, Color.DarkSeaGreen, Color.DarkSlateBlue, Color.DarkSlateGray,
Color.DarkTurquoise, Color.DarkViolet, Color.DeepPink, Color.DeepSkyBlue,
Color.DimGray, Color.DodgerBlue, Color.Firebrick, Color.FloralWhite, Color.ForestGreen,
Color.Fuchsia, Color.Gainsboro, Color.GhostWhite, Color.Gold,
Color.Goldenrod, Color.Gray, Color.Green, Color.GreenYellow, Color.Honeydew,
Color.HotPink, Color.IndianRed, Color.Indigo, Color.Ivory,
Color.Khaki, Color.Lavender, Color.LavenderBlush, Color.LawnGreen, Color.LemonChiffon,
Color.LightBlue, Color.LightCoral, Color.LightCyan, Color.LightGoldenrodYellow,
Color.LightGray, Color.LightGreen, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen,
Color.LightSkyBlue, Color.LightSlateGray, Color.LightSteelBlue, Color.LightYellow,
Color.Lime, Color.LimeGreen, Color.Linen, Color.Magenta, Color.Maroon,
Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid, Color.MediumPurple,
Color.MediumSeaGreen, Color.MediumSlateBlue, Color.MediumSpringGreen, Color.MediumTurquoise, Color.MediumVioletRed,
Color.MidnightBlue, Color.MintCream, Color.MistyRose, Color.Moccasin,
Color.NavajoWhite, Color.Navy, Color.OldLace, Color.Olive, Color.OliveDrab,
Color.Orange, Color.OrangeRed, Color.Orchid, Color.PaleGoldenrod,
Color.PaleGreen, Color.PaleTurquoise, Color.PaleVioletRed, Color.PapayaWhip, Color.PeachPuff,
Color.Peru, Color.Pink, Color.Plum, Color.PowderBlue,
Color.Purple, Color.Red, Color.RosyBrown, Color.RoyalBlue, Color.SaddleBrown,
Color.Salmon, Color.SandyBrown, Color.SeaGreen, Color.SeaShell,
Color.Sienna, Color.Silver, Color.SkyBlue, Color.SlateBlue, Color.SlateGray,
Color.Snow, Color.SpringGreen, Color.SteelBlue, Color.Tan,
Color.Teal, Color.Thistle, Color.Tomato, Color.Transparent, Color.Turquoise,
Color.Violet, Color.Wheat, Color.White, Color.WhiteSmoke,
Color.Yellow, Color.YellowGreen 
};
    public static Color[] NamedColors
    {
        get { return namedColors; }
    }

}
