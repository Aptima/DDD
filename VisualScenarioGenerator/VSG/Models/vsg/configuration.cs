namespace Config.Parameters { 
using System.Drawing; 
using System.ComponentModel; 
using System.Collections; 
using System.Collections.Generic; 
using System.Drawing.Design; 
using System.Windows.Forms.Design; 
using System; 
using System.Text; 
using AME.Controllers.Base.TypeConversion; 

            internal class ArrayConverterToString : ArrayConverter
            {
                public override object ConvertTo(ITypeDescriptorContext context,
                                         System.Globalization.CultureInfo culture,
                                         object value, Type destType)
                {
                    if (destType == typeof(string) && value is Array)
                    {
                        Array arrayVal = (Array)value;

                        StringBuilder nameBuilder = new StringBuilder();
                        for (int i = 0; i < arrayVal.Length; i++)
                        {
                            if (i == 0)
                            {
                                nameBuilder.Append('[');
                            }
                            if (i != 0)
                            {
                                nameBuilder.Append(", ");
                            }
                            nameBuilder.Append(arrayVal.GetValue(i).ToString());
                            if (i == arrayVal.Length-1)
                            {
                                nameBuilder.Append(']');
                            }
                        }
                        return nameBuilder.ToString();
                    }
                    return base.ConvertTo(context, culture, value, destType);
                }
            }
            internal class Util
            {
                public static Boolean IsNull(Object obj)
                {
                    return obj == null;
                }
            }public class Scenario { 
public enum RangeRingDisplayEnum{ Full, [StringValue("Shared Sensor Network")]SharedSensorNetwork, Personal, Disabled } 
private System.String m_Scenarioscenarioname;
private System.String m_Scenariodescription;
private System.Int32 m_Scenariotimetoattack;
private System.Boolean m_Scenarioallowassettransfers;
private RangeRingDisplayEnum m_Scenariorangeringdisplaytype;
public Scenario() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Scenarioscenarioname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Scenarioscenarioname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Scenariodescription"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Scenariodescription = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Scenariotimetoattack"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Int32)); 
if (value.Length > 0) { 
m_Scenariotimetoattack = (System.Int32)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Scenarioallowassettransfers"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Scenarioallowassettransfers = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Scenariorangeringdisplaytype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(RangeRingDisplayEnum)); 
if (value.Length > 0) { 
m_Scenariorangeringdisplaytype = (RangeRingDisplayEnum)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Scenario Name"), DefaultValueAttribute(@""), CategoryAttribute("Scenario"), DescriptionAttribute("Name of Scenario")]
public System.String Scenarioscenarioname { 
 get { return m_Scenarioscenarioname; } 
set { m_Scenarioscenarioname = value; } } 

[DisplayName("Description"), DefaultValueAttribute(@""), CategoryAttribute("Scenario"), DescriptionAttribute("Description of scenario")]
public System.String Scenariodescription { 
 get { return m_Scenariodescription; } 
set { m_Scenariodescription = value; } } 

[DisplayName("Time To Attack"), DefaultValueAttribute(@"10"), CategoryAttribute("Scenario"), DescriptionAttribute("Time To Attack")]
public System.Int32 Scenariotimetoattack { 
 get { return m_Scenariotimetoattack; } 
set { m_Scenariotimetoattack = value; } } 

[DisplayName("Allow Asset Transfers"), DefaultValueAttribute(@"false"), CategoryAttribute("Scenario"), DescriptionAttribute("Enables or disables the ability for Decision Makers to be able to transfer assets between each other.")]
public System.Boolean Scenarioallowassettransfers { 
 get { return m_Scenarioallowassettransfers; } 
set { m_Scenarioallowassettransfers = value; } } 

[DisplayName("Range Ring Display Type"), DefaultValueAttribute(@"Full"), CategoryAttribute("Scenario"), DescriptionAttribute("Range Ring Client Display Value")]
public RangeRingDisplayEnum Scenariorangeringdisplaytype { 
 get { return m_Scenariorangeringdisplaytype; } 
set { m_Scenariorangeringdisplaytype = value; } } 
 } 
public class Playfield { 
private System.String m_Playfieldmapfilename;
private System.String m_Playfieldiconlibrary;
private System.String m_Playfieldutmzone;
private System.Decimal m_Playfieldverticalscale;
private System.Decimal m_Playfieldhorizontalscale;
private System.Decimal m_Playfieldnorthing;
private System.Decimal m_Playfieldeasting;
public Playfield() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Playfieldmapfilename"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Playfieldmapfilename = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Playfieldiconlibrary"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Playfieldiconlibrary = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Playfieldutmzone"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Playfieldutmzone = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Playfieldverticalscale"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Playfieldverticalscale = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Playfieldhorizontalscale"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Playfieldhorizontalscale = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Playfieldnorthing"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Playfieldnorthing = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Playfieldeasting"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Playfieldeasting = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Map Filename"), DefaultValueAttribute(@""), CategoryAttribute("Playfield"), DescriptionAttribute("Map Filename")]
public System.String Playfieldmapfilename { 
 get { return m_Playfieldmapfilename; } 
set { m_Playfieldmapfilename = value; } } 

[DisplayName("Icon Library"), DefaultValueAttribute(@""), CategoryAttribute("Playfield"), DescriptionAttribute("Icon Library")]
public System.String Playfieldiconlibrary { 
 get { return m_Playfieldiconlibrary; } 
set { m_Playfieldiconlibrary = value; } } 

[DisplayName("UTM Zone"), DefaultValueAttribute(@"None"), CategoryAttribute("Playfield"), DescriptionAttribute("UTM Zone")]
public System.String Playfieldutmzone { 
 get { return m_Playfieldutmzone; } 
set { m_Playfieldutmzone = value; } } 

[DisplayName("Vertical Scale"), DefaultValueAttribute(@"1.0"), CategoryAttribute("Playfield"), DescriptionAttribute("Vertical Scale")]
public System.Decimal Playfieldverticalscale { 
 get { return m_Playfieldverticalscale; } 
set { if (value < 1) {  throw new ArgumentException(" Could not satisfy constraint: min, value: 1 "); }if (value > 5000) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 5000 "); } else { m_Playfieldverticalscale = value; } } }

[DisplayName("Horizontal Scale"), DefaultValueAttribute(@"1.0"), CategoryAttribute("Playfield"), DescriptionAttribute("Horizontal Scale")]
public System.Decimal Playfieldhorizontalscale { 
 get { return m_Playfieldhorizontalscale; } 
set { if (value < 1) {  throw new ArgumentException(" Could not satisfy constraint: min, value: 1 "); }if (value > 5000) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 5000 "); } else { m_Playfieldhorizontalscale = value; } } }

[DisplayName("Northing"), DefaultValueAttribute(@"0.0"), CategoryAttribute("Playfield"), DescriptionAttribute("Northing")]
public System.Decimal Playfieldnorthing { 
 get { return m_Playfieldnorthing; } 
set { m_Playfieldnorthing = value; } } 

[DisplayName("Easting"), DefaultValueAttribute(@"0.0"), CategoryAttribute("Playfield"), DescriptionAttribute("Easting")]
public System.Decimal Playfieldeasting { 
 get { return m_Playfieldeasting; } 
set { m_Playfieldeasting = value; } } 
 } 
public class LandRegion { 
public enum Colors{ AliceBlue, Aqua, Blue, BlueViolet, Brown, CornflowerBlue, Crimson, Cyan, DarkBlue, DarkGray, DarkGreen, DarkKhaki, DarkOliveGreen, DarkRed, DarkSlateGray, DodgerBlue, Fuchsia, Gray, Green, GreenYellow, HotPink, Khaki, Lavender, LimeGreen, Magenta, Maroon, Navy, Olive, OliveDrab, Orange, OrangeRed, Plum, PowderBlue, Purple, Red, RosyBrown, RoyalBlue, SandyBrown, SeaGreen, Silver, SkyBlue, SlateBlue, SlateGray, Tan, Teal, Transparent, Turquoise, Violet, Yellow, YellowGreen } 
private System.String m_Locationpolygonpoints;
private System.Drawing.Color m_Colorcolor;
private System.String m_Coloralpha;
public LandRegion() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Locationpolygonpoints"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Locationpolygonpoints = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Colorcolor"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color)); 
if (value.Length > 0) { 
m_Colorcolor = (System.Drawing.Color)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Coloralpha"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Coloralpha = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Polygon Points"), DefaultValueAttribute(@""), CategoryAttribute("Location"), DescriptionAttribute("Location of the Land Region")]
public System.String Locationpolygonpoints { 
 get { return m_Locationpolygonpoints; } 
set { m_Locationpolygonpoints = value; } } 

[DisplayName("Color"), DefaultValueAttribute(@"Brown"), CategoryAttribute("Color"), DescriptionAttribute("Color")]
public System.Drawing.Color Colorcolor { 
 get { return m_Colorcolor; } 
set { m_Colorcolor = value; } } 

[DisplayName("Alpha"), DefaultValueAttribute(@""), CategoryAttribute("Color"), DescriptionAttribute("Alpha")]
public System.String Coloralpha { 
 get { return m_Coloralpha; } 
set { m_Coloralpha = value; } } 
 } 
public class ActiveRegion { 
public enum Colors{ AliceBlue, Aqua, Blue, BlueViolet, Brown, CornflowerBlue, Crimson, Cyan, DarkBlue, DarkGray, DarkGreen, DarkKhaki, DarkOliveGreen, DarkRed, DarkSlateGray, DodgerBlue, Fuchsia, Gray, Green, GreenYellow, HotPink, Khaki, Lavender, LimeGreen, Magenta, Maroon, Navy, Olive, OliveDrab, Orange, OrangeRed, Plum, PowderBlue, Purple, Red, RosyBrown, RoyalBlue, SandyBrown, SeaGreen, Silver, SkyBlue, SlateBlue, SlateGray, Tan, Teal, Transparent, Turquoise, Violet, Yellow, YellowGreen } 
private System.String m_Locationpolygonpoints;
private System.String m_Locationrelativepolygonpoints;
private System.String m_Locationreferencepoint;
private System.Boolean m_Activeregionisdynamicregion;
private System.Drawing.Color m_Colorcolor;
private System.String m_Coloralpha;
private System.Decimal m_Activeregionstart;
private System.Decimal m_Activeregionend;
private System.Decimal m_Activeregionspeedmultiplier;
private System.Boolean m_Activeregionblocksmovement;
private System.String m_Activeregionsensorsblocked;
private System.Boolean m_Activeregionisvisible;
public ActiveRegion() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Locationpolygonpoints"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Locationpolygonpoints = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Colorcolor"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color)); 
if (value.Length > 0) { 
m_Colorcolor = (System.Drawing.Color)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Coloralpha"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Coloralpha = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionstart"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Activeregionstart = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionend"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Activeregionend = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionspeedmultiplier"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Activeregionspeedmultiplier = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionblocksmovement"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Activeregionblocksmovement = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionsensorsblocked"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Activeregionsensorsblocked = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionisvisible"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Activeregionisvisible = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Locationreferencepoint"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Locationreferencepoint = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Activeregionisdynamicregion"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Activeregionisdynamicregion = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Locationrelativepolygonpoints"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Locationrelativepolygonpoints = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Polygon Points"), DefaultValueAttribute(@""), CategoryAttribute("Location"), DescriptionAttribute("Location of the Land Region")]
public System.String Locationpolygonpoints { 
 get { return m_Locationpolygonpoints; } 
set { m_Locationpolygonpoints = value; } } 

[DisplayName("Relative Polygon Points"), DefaultValueAttribute(@""), CategoryAttribute("Location"), DescriptionAttribute("Relative Location of the Land Region")]
public System.String Locationrelativepolygonpoints { 
 get { return m_Locationrelativepolygonpoints; } 
set { m_Locationrelativepolygonpoints = value; } } 

[DisplayName("Reference Point"), DefaultValueAttribute(@""), CategoryAttribute("Location"), DescriptionAttribute("Reference point for the Land Region")]
public System.String Locationreferencepoint { 
 get { return m_Locationreferencepoint; } 
set { m_Locationreferencepoint = value; } } 

[DisplayName("Is this a Dynamic Region"), DefaultValueAttribute(@"false"), CategoryAttribute("Active Region"), DescriptionAttribute("Is this a dynamic Region?")]
public System.Boolean Activeregionisdynamicregion { 
 get { return m_Activeregionisdynamicregion; } 
set { m_Activeregionisdynamicregion = value; } } 

[DisplayName("Color"), DefaultValueAttribute(@"DarkGreen"), CategoryAttribute("Color"), DescriptionAttribute("Color")]
public System.Drawing.Color Colorcolor { 
 get { return m_Colorcolor; } 
set { m_Colorcolor = value; } } 

[DisplayName("Alpha"), DefaultValueAttribute(@""), CategoryAttribute("Color"), DescriptionAttribute("Alpha")]
public System.String Coloralpha { 
 get { return m_Coloralpha; } 
set { m_Coloralpha = value; } } 

[DisplayName("Start"), DefaultValueAttribute(@"0.0"), CategoryAttribute("Active Region"), DescriptionAttribute("Start")]
public System.Decimal Activeregionstart { 
 get { return m_Activeregionstart; } 
set { m_Activeregionstart = value; } } 

[DisplayName("End"), DefaultValueAttribute(@"0.0"), CategoryAttribute("Active Region"), DescriptionAttribute("End")]
public System.Decimal Activeregionend { 
 get { return m_Activeregionend; } 
set { m_Activeregionend = value; } } 

[DisplayName("Speed Multiplier"), DefaultValueAttribute(@"1.0"), CategoryAttribute("Active Region"), DescriptionAttribute("Speed Multiplier")]
public System.Decimal Activeregionspeedmultiplier { 
 get { return m_Activeregionspeedmultiplier; } 
set { m_Activeregionspeedmultiplier = value; } } 

[DisplayName("Blocks Movement"), DefaultValueAttribute(@"false"), CategoryAttribute("Active Region"), DescriptionAttribute("Blocks Movement")]
public System.Boolean Activeregionblocksmovement { 
 get { return m_Activeregionblocksmovement; } 
set { m_Activeregionblocksmovement = value; } } 

[DisplayName("Sensors Blocked"), DefaultValueAttribute(@""), CategoryAttribute("Active Region"), DescriptionAttribute("Sensors Blocked")]
public System.String Activeregionsensorsblocked { 
 get { return m_Activeregionsensorsblocked; } 
set { m_Activeregionsensorsblocked = value; } } 

[DisplayName("Is Visible"), DefaultValueAttribute(@"true"), CategoryAttribute("Active Region"), DescriptionAttribute("Is Visible")]
public System.Boolean Activeregionisvisible { 
 get { return m_Activeregionisvisible; } 
set { m_Activeregionisvisible = value; } } 
 } 
public class DecisionMaker { 
public enum Colors{ AliceBlue, Aqua, Blue, BlueViolet, Brown, CornflowerBlue, Crimson, Cyan, DarkBlue, DarkGray, DarkGreen, DarkKhaki, DarkOliveGreen, DarkRed, DarkSlateGray, DodgerBlue, Fuchsia, Gray, Green, GreenYellow, HotPink, Khaki, Lavender, LimeGreen, Magenta, Maroon, Navy, Olive, OliveDrab, Orange, OrangeRed, Plum, PowderBlue, Purple, Red, RosyBrown, RoyalBlue, SandyBrown, SeaGreen, Silver, SkyBlue, SlateBlue, SlateGray, Tan, Teal, Transparent, Turquoise, Violet, Yellow, YellowGreen } 
private System.String m_Decisionmakerrole;
private System.Drawing.Color m_Decisionmakercolor;
private System.String m_Decisionmakerbriefing;
private System.Boolean m_Decisionmakercantransferassets;
private System.Boolean m_Decisionmakercanforceassettransfers;
public DecisionMaker() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Decisionmakerrole"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Decisionmakerrole = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Decisionmakercolor"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Color)); 
if (value.Length > 0) { 
m_Decisionmakercolor = (System.Drawing.Color)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Decisionmakerbriefing"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Decisionmakerbriefing = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Decisionmakercantransferassets"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Decisionmakercantransferassets = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Decisionmakercanforceassettransfers"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Decisionmakercanforceassettransfers = (System.Boolean)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Role"), DefaultValueAttribute(@""), CategoryAttribute("DecisionMaker"), DescriptionAttribute("Decision Maker role")]
public System.String Decisionmakerrole { 
 get { return m_Decisionmakerrole; } 
set { m_Decisionmakerrole = value; } } 

[DisplayName("Color"), DefaultValueAttribute(@"AliceBlue"), CategoryAttribute("DecisionMaker"), DescriptionAttribute("Decision Maker color")]
public System.Drawing.Color Decisionmakercolor { 
 get { return m_Decisionmakercolor; } 
set { m_Decisionmakercolor = value; } } 

[DisplayName("Briefing"), DefaultValueAttribute(@""), CategoryAttribute("DecisionMaker"), DescriptionAttribute("Decision Maker briefing")]
public System.String Decisionmakerbriefing { 
 get { return m_Decisionmakerbriefing; } 
set { m_Decisionmakerbriefing = value; } } 

[DisplayName("CanTransferAssets"), DefaultValueAttribute(@"false"), CategoryAttribute("DecisionMaker"), DescriptionAttribute("Defines whether or not a decision maker is allowed to transfer its assets.")]
public System.Boolean Decisionmakercantransferassets { 
 get { return m_Decisionmakercantransferassets; } 
set { m_Decisionmakercantransferassets = value; } } 

[DisplayName("CanForceAssetTransfers"), DefaultValueAttribute(@"false"), CategoryAttribute("DecisionMaker"), DescriptionAttribute("Defines whether or not a decision maker is allowed to force asset transfer for its subordinates.")]
public System.Boolean Decisionmakercanforceassettransfers { 
 get { return m_Decisionmakercanforceassettransfers; } 
set { m_Decisionmakercanforceassettransfers = value; } } 
 } 
public class Rule { 
public enum RuleTypes{ Object_1_Exists, Object_2_State_Transition } 
private System.Decimal m_Ruleincrement;
private System.String m_Ruleruletype;
private System.String m_Rulefromstate;
private System.String m_Rulenewstate;
public Rule() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Ruleincrement"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Ruleincrement = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Ruleruletype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Ruleruletype = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Rulefromstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Rulefromstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Rulenewstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Rulenewstate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Increment"), DefaultValueAttribute(@"0.0"), CategoryAttribute("Rule"), DescriptionAttribute("Increment")]
public System.Decimal Ruleincrement { 
 get { return m_Ruleincrement; } 
set { m_Ruleincrement = value; } } 

[DisplayName("RuleType"), DefaultValueAttribute(@"Object_1_Exists"), CategoryAttribute("Rule"), DescriptionAttribute("")]
public System.String Ruleruletype { 
 get { return m_Ruleruletype; } 
set { m_Ruleruletype = value; } } 

[DisplayName("FromState"), DefaultValueAttribute(@""), CategoryAttribute("Rule"), DescriptionAttribute("")]
public System.String Rulefromstate { 
 get { return m_Rulefromstate; } 
set { m_Rulefromstate = value; } } 

[DisplayName("NewState"), DefaultValueAttribute(@""), CategoryAttribute("Rule"), DescriptionAttribute("")]
public System.String Rulenewstate { 
 get { return m_Rulenewstate; } 
set { m_Rulenewstate = value; } } 
 } 
public class Actor { 
public enum WhoEnum{ Myself, Friendly, Hostile } 
public enum WhatEnum{ Any, Of_Species, Unit } 
public enum WhereEnum{ Anywhere, In_Region, Not_In_Region } 
private System.String m_Actorwho;
private System.String m_Actorwhat;
private System.String m_Actorwhere;
public Actor() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Actorwho"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Actorwho = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Actorwhat"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Actorwhat = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Actorwhere"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Actorwhere = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Who"), DefaultValueAttribute(@"Myself"), CategoryAttribute("Actor"), DescriptionAttribute("")]
public System.String Actorwho { 
 get { return m_Actorwho; } 
set { m_Actorwho = value; } } 

[DisplayName("What"), DefaultValueAttribute(@"Any"), CategoryAttribute("Actor"), DescriptionAttribute("")]
public System.String Actorwhat { 
 get { return m_Actorwhat; } 
set { m_Actorwhat = value; } } 

[DisplayName("Where"), DefaultValueAttribute(@"Anywhere"), CategoryAttribute("Actor"), DescriptionAttribute("")]
public System.String Actorwhere { 
 get { return m_Actorwhere; } 
set { m_Actorwhere = value; } } 
 } 
public class Region { 
private System.String m_Regionzone;
private System.String m_Regionrelationship;
public Region() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Regionzone"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Regionzone = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Regionrelationship"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Regionrelationship = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Zone"), DefaultValueAttribute(@""), CategoryAttribute("Region"), DescriptionAttribute("Zone")]
public System.String Regionzone { 
 get { return m_Regionzone; } 
set { m_Regionzone = value; } } 

[DisplayName("Relationship"), DefaultValueAttribute(@""), CategoryAttribute("Region"), DescriptionAttribute("Relationship")]
public System.String Regionrelationship { 
 get { return m_Regionrelationship; } 
set { m_Regionrelationship = value; } } 
 } 
public class Score { 
private System.Decimal m_Scoreinitial;
public Score() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Scoreinitial"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Scoreinitial = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Initial"), DefaultValueAttribute(@"0.0"), CategoryAttribute("Score"), DescriptionAttribute("Initial")]
public System.Decimal Scoreinitial { 
 get { return m_Scoreinitial; } 
set { m_Scoreinitial = value; } } 
 } 
public class Engram { 
public enum EngramTypeEnum{ String, Double, Logical } 
private System.String m_Engraminitialvalue;
private EngramTypeEnum m_Engramtype;
public Engram() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Engraminitialvalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engraminitialvalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(EngramTypeEnum)); 
if (value.Length > 0) { 
m_Engramtype = (EngramTypeEnum)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Initial Value"), DefaultValueAttribute(@""), CategoryAttribute("Engram"), DescriptionAttribute("Initial value for associated engram")]
public System.String Engraminitialvalue { 
 get { return m_Engraminitialvalue; } 
set { m_Engraminitialvalue = value; } } 

[DisplayName("Type"), DefaultValueAttribute(@"String"), CategoryAttribute("Engram"), DescriptionAttribute("Data type of the engram")]
public EngramTypeEnum Engramtype { 
 get { return m_Engramtype; } 
set { m_Engramtype = value; } } 
 } 
public class EngramRange { 
public enum DyadicCondition{ GT, LT, GE, LE, EQ } 
private System.String m_Engramrangename;
private System.String m_Engramrangeunit;
private System.String m_Engramrangeincluded;
private System.String m_Engramrangeexcluded;
private System.Boolean m_Engramrangeconditionalcomparison;
private System.Boolean m_Engramrangestringcomparison;
private System.String m_Engramrangecomparisonvalue;
public EngramRange() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Engramrangename"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangename = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeunit = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeincluded"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeincluded = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeexcluded"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeexcluded = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeconditionalcomparison"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeconditionalcomparison = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangestringcomparison"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangestringcomparison = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangecomparisonvalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangecomparisonvalue = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangename { 
 get { return m_Engramrangename; } 
set { m_Engramrangename = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeunit { 
 get { return m_Engramrangeunit; } 
set { m_Engramrangeunit = value; } } 

[DisplayName("Included"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeincluded { 
 get { return m_Engramrangeincluded; } 
set { m_Engramrangeincluded = value; } } 

[DisplayName("Excluded"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeexcluded { 
 get { return m_Engramrangeexcluded; } 
set { m_Engramrangeexcluded = value; } } 

[DisplayName("ConditionalComparison"), DefaultValueAttribute(@"True"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeconditionalcomparison { 
 get { return m_Engramrangeconditionalcomparison; } 
set { m_Engramrangeconditionalcomparison = value; } } 

[DisplayName("StringComparison"), DefaultValueAttribute(@"False"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangestringcomparison { 
 get { return m_Engramrangestringcomparison; } 
set { m_Engramrangestringcomparison = value; } } 

[DisplayName("ComparisonValue"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangecomparisonvalue { 
 get { return m_Engramrangecomparisonvalue; } 
set { m_Engramrangecomparisonvalue = value; } } 
 } 
public class Sensor { 
public enum Attributes{ ID, State, Location, Velocity, Throttle, ObjectName, OwnerID, ClassName, TeamName, Size, Vulnerability, IconName } 
private System.Double m_Sensorrange;
private System.Boolean m_Sensorglobal_sensor;
private System.Boolean m_Sensorattribute_sensor;
private System.Boolean m_Sensorcustom_attribute_sensor;
private System.String m_Sensorattribute;
private System.String m_Sensorcustom_attribute;
public Sensor() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Sensorrange"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Sensorrange = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorglobal_sensor"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Sensorglobal_sensor = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorattribute_sensor"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Sensorattribute_sensor = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorcustom_attribute_sensor"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Sensorcustom_attribute_sensor = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorattribute"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sensorattribute = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorcustom_attribute"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sensorcustom_attribute = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Range"), DefaultValueAttribute(@"0"), CategoryAttribute("Sensor"), DescriptionAttribute("Range of detection")]
public System.Double Sensorrange { 
 get { return m_Sensorrange; } 
set { m_Sensorrange = value; } } 

[DisplayName("Global_Sensor"), DefaultValueAttribute(@"true"), CategoryAttribute("Sensor"), DescriptionAttribute("Sense all attributes")]
public System.Boolean Sensorglobal_sensor { 
 get { return m_Sensorglobal_sensor; } 
set { m_Sensorglobal_sensor = value; } } 

[DisplayName("Attribute_Sensor"), DefaultValueAttribute(@"false"), CategoryAttribute("Sensor"), DescriptionAttribute("Sense a single attribute")]
public System.Boolean Sensorattribute_sensor { 
 get { return m_Sensorattribute_sensor; } 
set { m_Sensorattribute_sensor = value; } } 

[DisplayName("Custom_Attribute_Sensor"), DefaultValueAttribute(@"false"), CategoryAttribute("Sensor"), DescriptionAttribute("Sense a single custom attribute")]
public System.Boolean Sensorcustom_attribute_sensor { 
 get { return m_Sensorcustom_attribute_sensor; } 
set { m_Sensorcustom_attribute_sensor = value; } } 

[DisplayName("Attribute"), DefaultValueAttribute(@"Default"), CategoryAttribute("Sensor"), DescriptionAttribute("Attribute to sense")]
public System.String Sensorattribute { 
 get { return m_Sensorattribute; } 
set { m_Sensorattribute = value; } } 

[DisplayName("Custom_Attribute"), DefaultValueAttribute(@"Default"), CategoryAttribute("Sensor"), DescriptionAttribute("Custom Attribute to sense")]
public System.String Sensorcustom_attribute { 
 get { return m_Sensorcustom_attribute; } 
set { m_Sensorcustom_attribute = value; } } 
 } 
public class SensorRange { 
private System.Double m_Sensorrangerange;
private System.String m_Sensorrangelevel;
private System.Double m_Sensorrangespread;
private System.Double m_Sensorrangex;
private System.Double m_Sensorrangey;
private System.Double m_Sensorrangez;
public SensorRange() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Sensorrangerange"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Sensorrangerange = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorrangelevel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sensorrangelevel = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorrangespread"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Sensorrangespread = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorrangex"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Sensorrangex = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorrangey"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Sensorrangey = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sensorrangez"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Sensorrangez = (System.Double)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Range"), DefaultValueAttribute(@"0"), CategoryAttribute("SensorRange"), DescriptionAttribute("Range of this sensor")]
public System.Double Sensorrangerange { 
 get { return m_Sensorrangerange; } 
set { m_Sensorrangerange = value; } } 

[DisplayName("Level"), DefaultValueAttribute(@""), CategoryAttribute("SensorRange"), DescriptionAttribute("Level of this sensor")]
public System.String Sensorrangelevel { 
 get { return m_Sensorrangelevel; } 
set { m_Sensorrangelevel = value; } } 

[DisplayName("Spread"), DefaultValueAttribute(@"360"), CategoryAttribute("SensorRange"), DescriptionAttribute("Spread of this sensor")]
public System.Double Sensorrangespread { 
 get { return m_Sensorrangespread; } 
set { m_Sensorrangespread = value; } } 

[DisplayName("X"), DefaultValueAttribute(@"1"), CategoryAttribute("SensorRange"), DescriptionAttribute("X component of the direction vector")]
public System.Double Sensorrangex { 
 get { return m_Sensorrangex; } 
set { m_Sensorrangex = value; } } 

[DisplayName("Y"), DefaultValueAttribute(@"0"), CategoryAttribute("SensorRange"), DescriptionAttribute("Y component of the direction vector")]
public System.Double Sensorrangey { 
 get { return m_Sensorrangey; } 
set { m_Sensorrangey = value; } } 

[DisplayName("Z"), DefaultValueAttribute(@"0"), CategoryAttribute("SensorRange"), DescriptionAttribute("Z component of the direction vector")]
public System.Double Sensorrangez { 
 get { return m_Sensorrangez; } 
set { m_Sensorrangez = value; } } 
 } 
public class Emitter { 
public enum Attributes{ Default, Invisible, ID, State, Location, Velocity, Throttle, ObjectName, OwnerID, ClassName, TeamName, Size, Vulnerability, IconName } 
private System.Boolean m_Emitterattribute_emitter;
private System.Boolean m_Emittercustom_attribute_emitter;
private System.String m_Emitterattribute;
private System.String m_Emittercustom_attribute;
public Emitter() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Emitterattribute_emitter"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Emitterattribute_emitter = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Emittercustom_attribute_emitter"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Emittercustom_attribute_emitter = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Emitterattribute"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Emitterattribute = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Emittercustom_attribute"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Emittercustom_attribute = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Attribute_Emitter"), DefaultValueAttribute(@"true"), CategoryAttribute("Emitter"), DescriptionAttribute("Emits standard attributes")]
public System.Boolean Emitterattribute_emitter { 
 get { return m_Emitterattribute_emitter; } 
set { m_Emitterattribute_emitter = value; } } 

[DisplayName("Custom_Attribute_Emitter"), DefaultValueAttribute(@"false"), CategoryAttribute("Emitter"), DescriptionAttribute("Emits custom attribute")]
public System.Boolean Emittercustom_attribute_emitter { 
 get { return m_Emittercustom_attribute_emitter; } 
set { m_Emittercustom_attribute_emitter = value; } } 

[DisplayName("Attribute"), DefaultValueAttribute(@"Default"), CategoryAttribute("Emitter"), DescriptionAttribute("Attribute emitted")]
public System.String Emitterattribute { 
 get { return m_Emitterattribute; } 
set { m_Emitterattribute = value; } } 

[DisplayName("Custom_Attribute"), DefaultValueAttribute(@"Default"), CategoryAttribute("Emitter"), DescriptionAttribute("Custom Attribute emitted")]
public System.String Emittercustom_attribute { 
 get { return m_Emittercustom_attribute; } 
set { m_Emittercustom_attribute = value; } } 
 } 
public class Level { 
private System.Boolean m_Levelvariance;
private System.Boolean m_Levelprobability;
private System.Double m_Levelpercentage;
public Level() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Levelvariance"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Levelvariance = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Levelprobability"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Levelprobability = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Levelpercentage"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Levelpercentage = (System.Double)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Variance"), DefaultValueAttribute(@"true"), CategoryAttribute("Level"), DescriptionAttribute("Determines which of the following to use, probability or variance")]
public System.Boolean Levelvariance { 
 get { return m_Levelvariance; } 
set { m_Levelvariance = value; } } 

[DisplayName("Probability"), DefaultValueAttribute(@"false"), CategoryAttribute("Level"), DescriptionAttribute("Magnitude of emitter variance")]
public System.Boolean Levelprobability { 
 get { return m_Levelprobability; } 
set { m_Levelprobability = value; } } 

[DisplayName("Percentage"), DefaultValueAttribute(@"0"), CategoryAttribute("Level"), DescriptionAttribute("Probability of this emitter")]
public System.Double Levelpercentage { 
 get { return m_Levelpercentage; } 
set { m_Levelpercentage = value; } } 
 } 
public class Proximity { 
private System.Decimal m_Proximityrange;
public Proximity() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Proximityrange"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Proximityrange = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Range"), DefaultValueAttribute(@"0"), CategoryAttribute("Proximity"), DescriptionAttribute("Magnitude of effect")]
public System.Decimal Proximityrange { 
 get { return m_Proximityrange; } 
set { m_Proximityrange = value; } } 
 } 
public class Effect { 
private System.Int32 m_Effectintensity;
private System.Decimal m_Effectprobability;
public Effect() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Effectintensity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Int32)); 
if (value.Length > 0) { 
m_Effectintensity = (System.Int32)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Effectprobability"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Effectprobability = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Intensity"), DefaultValueAttribute(@"0"), CategoryAttribute("Effect"), DescriptionAttribute("Magnitude of effect")]
public System.Int32 Effectintensity { 
 get { return m_Effectintensity; } 
set { m_Effectintensity = value; } } 

[DisplayName("Probability"), DefaultValueAttribute(@"100.0"), CategoryAttribute("Effect"), DescriptionAttribute("Probability of this effect at this range")]
public System.Decimal Effectprobability { 
 get { return m_Effectprobability; } 
set { m_Effectprobability = value; } } 
 } 
public class Singleton { 
private System.String m_Singletoncapability;
public Singleton() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Singletoncapability"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Singletoncapability = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Capability"), DefaultValueAttribute(@""), CategoryAttribute("Singleton"), DescriptionAttribute("Capability of Singletont")]
public System.String Singletoncapability { 
 get { return m_Singletoncapability; } 
set { m_Singletoncapability = value; } } 
 } 
public class Combo { 
private System.String m_Combostate;
public Combo() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Combostate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Combostate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("State"), DefaultValueAttribute(@""), CategoryAttribute("Combo"), DescriptionAttribute("State of Combo")]
public System.String Combostate { 
 get { return m_Combostate; } 
set { m_Combostate = value; } } 
 } 
public class Transition { 
private System.Double m_Transitionrange;
private System.Int32 m_Transitionintensity;
private System.Decimal m_Transitionprobability;
private System.String m_Transitionstate;
public Transition() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Transitionrange"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Transitionrange = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Transitionintensity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Int32)); 
if (value.Length > 0) { 
m_Transitionintensity = (System.Int32)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Transitionprobability"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Transitionprobability = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Transitionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Transitionstate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Range"), DefaultValueAttribute(@"0"), CategoryAttribute("Transition"), DescriptionAttribute("Range of effect")]
public System.Double Transitionrange { 
 get { return m_Transitionrange; } 
set { m_Transitionrange = value; } } 

[DisplayName("Intensity"), DefaultValueAttribute(@"0"), CategoryAttribute("Transition"), DescriptionAttribute("Magnitude of effect")]
public System.Int32 Transitionintensity { 
 get { return m_Transitionintensity; } 
set { m_Transitionintensity = value; } } 

[DisplayName("Probability"), DefaultValueAttribute(@"100"), CategoryAttribute("Transition"), DescriptionAttribute("Probability of this effect at this range")]
public System.Decimal Transitionprobability { 
 get { return m_Transitionprobability; } 
set { m_Transitionprobability = value; } } 

[DisplayName("State"), DefaultValueAttribute(@""), CategoryAttribute("Transition"), DescriptionAttribute("State of transition")]
public System.String Transitionstate { 
 get { return m_Transitionstate; } 
set { m_Transitionstate = value; } } 
 } 
public class Contribution { 
private System.Double m_Contributionrange;
private System.Int32 m_Contributionintensity;
private System.Decimal m_Contributionprobability;
private System.String m_Contributioncapability;
public Contribution() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Contributionrange"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Contributionrange = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Contributionintensity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Int32)); 
if (value.Length > 0) { 
m_Contributionintensity = (System.Int32)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Contributionprobability"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Contributionprobability = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Contributioncapability"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Contributioncapability = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Range"), DefaultValueAttribute(@"0"), CategoryAttribute("Contribution"), DescriptionAttribute("Range of effect")]
public System.Double Contributionrange { 
 get { return m_Contributionrange; } 
set { m_Contributionrange = value; } } 

[DisplayName("Intensity"), DefaultValueAttribute(@"0"), CategoryAttribute("Contribution"), DescriptionAttribute("Magnitude of effect")]
public System.Int32 Contributionintensity { 
 get { return m_Contributionintensity; } 
set { m_Contributionintensity = value; } } 

[DisplayName("Probability"), DefaultValueAttribute(@"100"), CategoryAttribute("Contribution"), DescriptionAttribute("Probability of this effect at this range")]
public System.Decimal Contributionprobability { 
 get { return m_Contributionprobability; } 
set { m_Contributionprobability = value; } } 

[DisplayName("Capability"), DefaultValueAttribute(@""), CategoryAttribute("Contribution"), DescriptionAttribute("Capability of Singleton")]
public System.String Contributioncapability { 
 get { return m_Contributioncapability; } 
set { m_Contributioncapability = value; } } 
 } 
public class State { 
private System.Boolean m_Stateoverridestealable;
private System.Boolean m_Stateoverridelaunchduration;
private System.Boolean m_Stateoverridedockingduration;
private System.Boolean m_Stateoverridetimetoattack;
private System.Boolean m_Stateoverrideengagementduration;
private System.Boolean m_Stateoverridemaxspeed;
private System.Boolean m_Stateoverridefuelcapacity;
private System.Boolean m_Stateoverrideinitialfuel;
private System.Boolean m_Stateoverridefuelconsumption;
private System.Boolean m_Stateoverrideicon;
private System.Boolean m_Stateoverridefueldepletionstate;
private System.Boolean m_Statestealable;
private System.Double m_Statelaunchduration;
private System.Double m_Statedockingduration;
private System.Double m_Statetimetoattack;
private System.Double m_Stateengagementduration;
private System.Double m_Statemaxspeed;
private System.Double m_Statefuelcapacity;
private System.Double m_Stateinitialfuel;
private System.Double m_Statefuelconsumption;
private System.String m_Statefueldepletionstate;
private System.String m_Imageicon;
public State() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Stateoverridestealable"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridestealable = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridelaunchduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridelaunchduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridedockingduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridedockingduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridetimetoattack"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridetimetoattack = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverrideengagementduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverrideengagementduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridemaxspeed"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridemaxspeed = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridefuelcapacity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridefuelcapacity = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverrideinitialfuel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverrideinitialfuel = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridefuelconsumption"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridefuelconsumption = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverrideicon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverrideicon = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateoverridefueldepletionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Stateoverridefueldepletionstate = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statestealable"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Statestealable = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statelaunchduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Statelaunchduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statedockingduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Statedockingduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statetimetoattack"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Statetimetoattack = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateengagementduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Stateengagementduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statemaxspeed"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Statemaxspeed = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statefuelcapacity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Statefuelcapacity = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Stateinitialfuel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Stateinitialfuel = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statefuelconsumption"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Statefuelconsumption = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statefueldepletionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Statefueldepletionstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Imageicon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Imageicon = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("OverrideStealable"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Is this unit stealable")]
public System.Boolean Stateoverridestealable { 
 get { return m_Stateoverridestealable; } 
set { m_Stateoverridestealable = value; } } 

[DisplayName("OverrideLaunchDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverridelaunchduration { 
 get { return m_Stateoverridelaunchduration; } 
set { m_Stateoverridelaunchduration = value; } } 

[DisplayName("OverrideDockingDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverridedockingduration { 
 get { return m_Stateoverridedockingduration; } 
set { m_Stateoverridedockingduration = value; } } 

[DisplayName("OverrideTimeToAttack"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverridetimetoattack { 
 get { return m_Stateoverridetimetoattack; } 
set { m_Stateoverridetimetoattack = value; } } 

[DisplayName("OverrideEngagementDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Engagement Duration")]
public System.Boolean Stateoverrideengagementduration { 
 get { return m_Stateoverrideengagementduration; } 
set { m_Stateoverrideengagementduration = value; } } 

[DisplayName("OverrideMaxSpeed"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverridemaxspeed { 
 get { return m_Stateoverridemaxspeed; } 
set { m_Stateoverridemaxspeed = value; } } 

[DisplayName("OverrideFuelCapacity"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverridefuelcapacity { 
 get { return m_Stateoverridefuelcapacity; } 
set { m_Stateoverridefuelcapacity = value; } } 

[DisplayName("OverrideInitialFuel"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverrideinitialfuel { 
 get { return m_Stateoverrideinitialfuel; } 
set { m_Stateoverrideinitialfuel = value; } } 

[DisplayName("OverrideFuelConsumption"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Boolean Stateoverridefuelconsumption { 
 get { return m_Stateoverridefuelconsumption; } 
set { m_Stateoverridefuelconsumption = value; } } 

[DisplayName("OverrideIcon"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Image library key name of the icon.")]
public System.Boolean Stateoverrideicon { 
 get { return m_Stateoverrideicon; } 
set { m_Stateoverrideicon = value; } } 

[DisplayName("OverrideFuelDepletionState"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Image library key name of the icon.")]
public System.Boolean Stateoverridefueldepletionstate { 
 get { return m_Stateoverridefueldepletionstate; } 
set { m_Stateoverridefueldepletionstate = value; } } 

[DisplayName("Stealable"), DefaultValueAttribute(@"false"), CategoryAttribute("State"), DescriptionAttribute("Is this unit stealable")]
public System.Boolean Statestealable { 
 get { return m_Statestealable; } 
set { m_Statestealable = value; } } 

[DisplayName("LaunchDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Statelaunchduration { 
 get { return m_Statelaunchduration; } 
set { m_Statelaunchduration = value; } } 

[DisplayName("DockingDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Statedockingduration { 
 get { return m_Statedockingduration; } 
set { m_Statedockingduration = value; } } 

[DisplayName("TimeToAttack"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Statetimetoattack { 
 get { return m_Statetimetoattack; } 
set { m_Statetimetoattack = value; } } 

[DisplayName("EngagementDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Engagement Duration")]
public System.Double Stateengagementduration { 
 get { return m_Stateengagementduration; } 
set { m_Stateengagementduration = value; } } 

[DisplayName("MaxSpeed"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Statemaxspeed { 
 get { return m_Statemaxspeed; } 
set { m_Statemaxspeed = value; } } 

[DisplayName("FuelCapacity"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Statefuelcapacity { 
 get { return m_Statefuelcapacity; } 
set { m_Statefuelcapacity = value; } } 

[DisplayName("InitialFuel"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Stateinitialfuel { 
 get { return m_Stateinitialfuel; } 
set { m_Stateinitialfuel = value; } } 

[DisplayName("FuelConsumption"), DefaultValueAttribute(@"0"), CategoryAttribute("State"), DescriptionAttribute("Launch Duration")]
public System.Double Statefuelconsumption { 
 get { return m_Statefuelconsumption; } 
set { m_Statefuelconsumption = value; } } 

[DisplayName("FuelDepletionState"), DefaultValueAttribute(@"Dead"), CategoryAttribute("State"), DescriptionAttribute("Fuel Depletion State")]
public System.String Statefueldepletionstate { 
 get { return m_Statefueldepletionstate; } 
set { m_Statefueldepletionstate = value; } } 

[DisplayName("Icon"), DefaultValueAttribute(@"ImageLib.Unknown.png"), CategoryAttribute("Image"), DescriptionAttribute("Image library key name of the icon.")]
public System.String Imageicon { 
 get { return m_Imageicon; } 
set { m_Imageicon = value; } } 
 } 
public class Species { 
private System.Boolean m_Speciesisweapon;
private System.Boolean m_Speciesremoveondestruction;
private System.Boolean m_Speciesdefaultclassification;
private System.Double m_Speciescollisionradius;
private System.Boolean m_Specieslandobject;
private System.Boolean m_Speciesairobject;
private System.Boolean m_Speciesseaobject;
private System.Boolean m_Speciesexistingspecies;
private System.Boolean m_Specieslaunchedbyowner;
public Species() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Speciesisweapon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciesisweapon = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciesremoveondestruction"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciesremoveondestruction = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciesdefaultclassification"].Attributes;
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
if (myAttribute != null)
{
    value = myAttribute.Value.ToString();
    converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean));
    if (value.Length > 0)
    {
        m_Speciesdefaultclassification = (System.Boolean)converter.ConvertFromString(value);
    }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescollisionradius"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Speciescollisionradius = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Specieslandobject"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Specieslandobject = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciesairobject"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciesairobject = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciesseaobject"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciesseaobject = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciesexistingspecies"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciesexistingspecies = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Specieslaunchedbyowner"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Specieslaunchedbyowner = (System.Boolean)converter.ConvertFromString(value); }
}

 } 

[DisplayName("IsWeapon"), DefaultValueAttribute(@"false"), CategoryAttribute("Species"), DescriptionAttribute("Is this a weapon?")]
public System.Boolean Speciesisweapon { 
 get { return m_Speciesisweapon; } 
set { m_Speciesisweapon = value; } } 

[DisplayName("RemoveOnDestruction"), DefaultValueAttribute(@"true"), CategoryAttribute("Species"), DescriptionAttribute("Does this get removed from game after death?")]
public System.Boolean Speciesremoveondestruction { 
 get { return m_Speciesremoveondestruction; } 
set { m_Speciesremoveondestruction = value; } }

[DisplayName("DefaultClassification"), DefaultValueAttribute(@"true"), CategoryAttribute("Species"), DescriptionAttribute("Default Classification Tag")]
public System.Boolean Speciesdefaultclassification
{
    get { return m_Speciesdefaultclassification; }
    set { m_Speciesdefaultclassification = value; }
} 

[DisplayName("CollisionRadius"), DefaultValueAttribute(@"0"), CategoryAttribute("Species"), DescriptionAttribute("IWhen an object gets this close there's a collision")]
public System.Double Speciescollisionradius { 
 get { return m_Speciescollisionradius; } 
set { m_Speciescollisionradius = value; } } 

[DisplayName("LandObject"), DefaultValueAttribute(@"true"), CategoryAttribute("Species"), DescriptionAttribute("Base upon generic land object")]
public System.Boolean Specieslandobject { 
 get { return m_Specieslandobject; } 
set { m_Specieslandobject = value; } } 

[DisplayName("AirObject"), DefaultValueAttribute(@"false"), CategoryAttribute("Species"), DescriptionAttribute("Base upon generic air object")]
public System.Boolean Speciesairobject { 
 get { return m_Speciesairobject; } 
set { m_Speciesairobject = value; } } 

[DisplayName("SeaObject"), DefaultValueAttribute(@"false"), CategoryAttribute("Species"), DescriptionAttribute("Base upon generic sea object")]
public System.Boolean Speciesseaobject { 
 get { return m_Speciesseaobject; } 
set { m_Speciesseaobject = value; } } 

[DisplayName("ExistingSpecies"), DefaultValueAttribute(@"false"), CategoryAttribute("Species"), DescriptionAttribute("Base upon existing species")]
public System.Boolean Speciesexistingspecies { 
 get { return m_Speciesexistingspecies; } 
set { m_Speciesexistingspecies = value; } } 

[DisplayName("LaunchedByOwner"), DefaultValueAttribute(@"false"), CategoryAttribute("Species"), DescriptionAttribute("If true, this species can be launched (as a subplatform or weapon) ONLY by the subplatform's owner.  If false, this species can ONLY be launched by it's platform's owner.")]
public System.Boolean Specieslaunchedbyowner { 
 get { return m_Specieslaunchedbyowner; } 
set { m_Specieslaunchedbyowner = value; } } 
 } 
public class RevealEvent { 
private System.Decimal m_Revealeventtime;
private System.Double m_Initiallocationx;
private System.Double m_Initiallocationy;
private System.Double m_Initiallocationz;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
private System.String m_Revealeventinitialstate;
private System.String m_Startupparametersinitialtag;
private System.Boolean m_Speciescompletionunit;
private System.Boolean m_Startupparametersoverridestealable;
private System.Boolean m_Startupparametersoverridelaunchduration;
private System.Boolean m_Startupparametersoverridedockingduration;
private System.Boolean m_Startupparametersoverridetimetoattack;
private System.Boolean m_Startupparametersoverrideengagementduration;
private System.Boolean m_Startupparametersoverridemaxspeed;
private System.Boolean m_Startupparametersoverridefuelcapacity;
private System.Boolean m_Startupparametersoverrideinitialfuel;
private System.Boolean m_Startupparametersoverridefuelconsumption;
private System.Boolean m_Startupparametersoverrideicon;
private System.Boolean m_Startupparametersoverridefueldepletionstate;
private System.Boolean m_Startupparametersstealable;
private System.Double m_Startupparameterslaunchduration;
private System.Double m_Startupparametersdockingduration;
private System.Double m_Startupparameterstimetoattack;
private System.Double m_Startupparametersengagementduration;
private System.Double m_Startupparametersmaxspeed;
private System.Double m_Startupparametersfuelcapacity;
private System.Double m_Startupparametersinitialfuel;
private System.Double m_Startupparametersfuelconsumption;
private System.String m_Startupparametersicon;
private System.String m_Revealeventfueldepletionstate;
public RevealEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Revealeventtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Revealeventtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Initiallocationx"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Initiallocationx = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Initiallocationy"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Initiallocationy = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Initiallocationz"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Initiallocationz = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Revealeventinitialstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Revealeventinitialstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersinitialtag"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Startupparametersinitialtag = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridestealable"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridestealable = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridelaunchduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridelaunchduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridedockingduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridedockingduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridetimetoattack"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridetimetoattack = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverrideengagementduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverrideengagementduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridemaxspeed"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridemaxspeed = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridefuelcapacity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridefuelcapacity = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverrideinitialfuel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverrideinitialfuel = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridefuelconsumption"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridefuelconsumption = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverrideicon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverrideicon = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridefueldepletionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridefueldepletionstate = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersstealable"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersstealable = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparameterslaunchduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparameterslaunchduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersdockingduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersdockingduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparameterstimetoattack"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparameterstimetoattack = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersengagementduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersengagementduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersmaxspeed"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersmaxspeed = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersfuelcapacity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersfuelcapacity = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersinitialfuel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersinitialfuel = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersfuelconsumption"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersfuelconsumption = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersicon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Startupparametersicon = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Revealeventfueldepletionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Revealeventfueldepletionstate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("RevealEvent"), DescriptionAttribute("Time of reveal")]
public System.Decimal Revealeventtime { 
 get { return m_Revealeventtime; } 
set { m_Revealeventtime = value; } } 

[DisplayName("X"), DefaultValueAttribute(@"0"), CategoryAttribute("InitialLocation"), DescriptionAttribute("")]
public System.Double Initiallocationx { 
 get { return m_Initiallocationx; } 
set { if (value > 9999999) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 9999999 "); } else { m_Initiallocationx = value; } } }

[DisplayName("Y"), DefaultValueAttribute(@"0"), CategoryAttribute("InitialLocation"), DescriptionAttribute("")]
public System.Double Initiallocationy { 
 get { return m_Initiallocationy; } 
set { if (value > 9999999) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 9999999 "); } else { m_Initiallocationy = value; } } }

[DisplayName("Z"), DefaultValueAttribute(@"0"), CategoryAttribute("InitialLocation"), DescriptionAttribute("")]
public System.Double Initiallocationz { 
 get { return m_Initiallocationz; } 
set { m_Initiallocationz = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 

[DisplayName("InitialState"), DefaultValueAttribute(@"FullyFunctional"), CategoryAttribute("RevealEvent"), DescriptionAttribute("")]
public System.String Revealeventinitialstate { 
 get { return m_Revealeventinitialstate; } 
set { m_Revealeventinitialstate = value; } } 

[DisplayName("InitialTag"), DefaultValueAttribute(@""), CategoryAttribute("StartupParameters"), DescriptionAttribute("The initial tag for the object")]
public System.String Startupparametersinitialtag { 
 get { return m_Startupparametersinitialtag; } 
set { m_Startupparametersinitialtag = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 

[DisplayName("OverrideStealable"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Is this unit stealable")]
public System.Boolean Startupparametersoverridestealable { 
 get { return m_Startupparametersoverridestealable; } 
set { m_Startupparametersoverridestealable = value; } } 

[DisplayName("OverrideLaunchDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridelaunchduration { 
 get { return m_Startupparametersoverridelaunchduration; } 
set { m_Startupparametersoverridelaunchduration = value; } } 

[DisplayName("OverrideDockingDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridedockingduration { 
 get { return m_Startupparametersoverridedockingduration; } 
set { m_Startupparametersoverridedockingduration = value; } } 

[DisplayName("OverrideTimeToAttack"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Attack Duration")]
public System.Boolean Startupparametersoverridetimetoattack { 
 get { return m_Startupparametersoverridetimetoattack; } 
set { m_Startupparametersoverridetimetoattack = value; } } 

[DisplayName("OverrideEngagementDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Engagement Duration")]
public System.Boolean Startupparametersoverrideengagementduration { 
 get { return m_Startupparametersoverrideengagementduration; } 
set { m_Startupparametersoverrideengagementduration = value; } } 

[DisplayName("OverrideMaxSpeed"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Maximum Speed")]
public System.Boolean Startupparametersoverridemaxspeed { 
 get { return m_Startupparametersoverridemaxspeed; } 
set { m_Startupparametersoverridemaxspeed = value; } } 

[DisplayName("OverrideFuelCapacity"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Fuel Capacity")]
public System.Boolean Startupparametersoverridefuelcapacity { 
 get { return m_Startupparametersoverridefuelcapacity; } 
set { m_Startupparametersoverridefuelcapacity = value; } } 

[DisplayName("OverrideInitialFuel"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Initial Fuel")]
public System.Boolean Startupparametersoverrideinitialfuel { 
 get { return m_Startupparametersoverrideinitialfuel; } 
set { m_Startupparametersoverrideinitialfuel = value; } } 

[DisplayName("OverrideFuelConsumption"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridefuelconsumption { 
 get { return m_Startupparametersoverridefuelconsumption; } 
set { m_Startupparametersoverridefuelconsumption = value; } } 

[DisplayName("OverrideIcon"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Image library key name of the icon.")]
public System.Boolean Startupparametersoverrideicon { 
 get { return m_Startupparametersoverrideicon; } 
set { m_Startupparametersoverrideicon = value; } } 

[DisplayName("OverrideFuelDepletionState"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Image library key name of the icon.")]
public System.Boolean Startupparametersoverridefueldepletionstate { 
 get { return m_Startupparametersoverridefueldepletionstate; } 
set { m_Startupparametersoverridefueldepletionstate = value; } } 

[DisplayName("Stealable"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Is this unit stealable")]
public System.Boolean Startupparametersstealable { 
 get { return m_Startupparametersstealable; } 
set { m_Startupparametersstealable = value; } } 

[DisplayName("LaunchDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparameterslaunchduration { 
 get { return m_Startupparameterslaunchduration; } 
set { m_Startupparameterslaunchduration = value; } } 

[DisplayName("DockingDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersdockingduration { 
 get { return m_Startupparametersdockingduration; } 
set { m_Startupparametersdockingduration = value; } } 

[DisplayName("TimeToAttack"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Attack Duration")]
public System.Double Startupparameterstimetoattack { 
 get { return m_Startupparameterstimetoattack; } 
set { m_Startupparameterstimetoattack = value; } } 

[DisplayName("EngagementDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Engagement Duration")]
public System.Double Startupparametersengagementduration { 
 get { return m_Startupparametersengagementduration; } 
set { m_Startupparametersengagementduration = value; } } 

[DisplayName("MaxSpeed"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersmaxspeed { 
 get { return m_Startupparametersmaxspeed; } 
set { m_Startupparametersmaxspeed = value; } } 

[DisplayName("FuelCapacity"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersfuelcapacity { 
 get { return m_Startupparametersfuelcapacity; } 
set { m_Startupparametersfuelcapacity = value; } } 

[DisplayName("InitialFuel"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersinitialfuel { 
 get { return m_Startupparametersinitialfuel; } 
set { m_Startupparametersinitialfuel = value; } } 

[DisplayName("FuelConsumption"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersfuelconsumption { 
 get { return m_Startupparametersfuelconsumption; } 
set { m_Startupparametersfuelconsumption = value; } } 

[DisplayName("Icon"), DefaultValueAttribute(@""), CategoryAttribute("StartupParameters"), DescriptionAttribute("Image library key name of the icon.")]
public System.String Startupparametersicon { 
 get { return m_Startupparametersicon; } 
set { m_Startupparametersicon = value; } } 

[DisplayName("FuelDepletionState"), DefaultValueAttribute(@"Dead"), CategoryAttribute("RevealEvent"), DescriptionAttribute("")]
public System.String Revealeventfueldepletionstate { 
 get { return m_Revealeventfueldepletionstate; } 
set { m_Revealeventfueldepletionstate = value; } } 
 } 
public class MoveEvent { 
private System.Decimal m_Moveeventtime;
private System.Decimal m_Moveeventthrottle;
private System.Double m_Destinationlocationx;
private System.Double m_Destinationlocationy;
private System.Double m_Destinationlocationz;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
private System.Boolean m_Speciescompletionunit;
public MoveEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Moveeventtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Moveeventtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Moveeventthrottle"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Moveeventthrottle = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Destinationlocationx"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Destinationlocationx = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Destinationlocationy"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Destinationlocationy = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Destinationlocationz"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Destinationlocationz = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("MoveEvent"), DescriptionAttribute("Time of move")]
public System.Decimal Moveeventtime { 
 get { return m_Moveeventtime; } 
set { m_Moveeventtime = value; } } 

[DisplayName("Throttle"), DefaultValueAttribute(@"100"), CategoryAttribute("MoveEvent"), DescriptionAttribute("Time of move")]
public System.Decimal Moveeventthrottle { 
 get { return m_Moveeventthrottle; } 
set { m_Moveeventthrottle = value; } } 

[DisplayName("X"), DefaultValueAttribute(@"0"), CategoryAttribute("DestinationLocation"), DescriptionAttribute("")]
public System.Double Destinationlocationx { 
 get { return m_Destinationlocationx; } 
set { if (value > 9999999) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 9999999 "); } else { m_Destinationlocationx = value; } } }

[DisplayName("Y"), DefaultValueAttribute(@"0"), CategoryAttribute("DestinationLocation"), DescriptionAttribute("")]
public System.Double Destinationlocationy { 
 get { return m_Destinationlocationy; } 
set { if (value > 9999999) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 9999999 "); } else { m_Destinationlocationy = value; } } }

[DisplayName("Z"), DefaultValueAttribute(@"0"), CategoryAttribute("DestinationLocation"), DescriptionAttribute("")]
public System.Double Destinationlocationz { 
 get { return m_Destinationlocationz; } 
set { m_Destinationlocationz = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 
 } 
public class CompletionEvent { 
private System.String m_Completioneventaction;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
private System.Boolean m_Speciescompletionunit;
private System.String m_Completioneventstate;
public CompletionEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Completioneventaction"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Completioneventaction = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Completioneventstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Completioneventstate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Action"), DefaultValueAttribute(@""), CategoryAttribute("CompletionEvent"), DescriptionAttribute("Type of action that triggers the completion.")]
public System.String Completioneventaction { 
 get { return m_Completioneventaction; } 
set { m_Completioneventaction = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 

[DisplayName("State"), DefaultValueAttribute(@""), CategoryAttribute("CompletionEvent"), DescriptionAttribute("")]
public System.String Completioneventstate { 
 get { return m_Completioneventstate; } 
set { m_Completioneventstate = value; } } 
 } 
public class LaunchEvent { 
private System.Decimal m_Launcheventtime;
private System.Double m_Relativelocationx;
private System.Double m_Relativelocationy;
private System.Double m_Relativelocationz;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
private System.Boolean m_Speciescompletionunit;
private System.String m_Launcheventinitialstate;
private System.Boolean m_Startupparametersoverridestealable;
private System.Boolean m_Startupparametersoverridelaunchduration;
private System.Boolean m_Startupparametersoverridedockingduration;
private System.Boolean m_Startupparametersoverridetimetoattack;
private System.Boolean m_Startupparametersoverrideengagementduration;
private System.Boolean m_Startupparametersoverridemaxspeed;
private System.Boolean m_Startupparametersoverridefuelcapacity;
private System.Boolean m_Startupparametersoverrideinitialfuel;
private System.Boolean m_Startupparametersoverridefuelconsumption;
private System.Boolean m_Startupparametersoverrideicon;
private System.Boolean m_Startupparametersoverridefueldepletionstate;
private System.Boolean m_Startupparametersstealable;
private System.Double m_Startupparameterslaunchduration;
private System.Double m_Startupparametersdockingduration;
private System.Double m_Startupparametersengagementduration;
private System.Double m_Startupparametersmaxspeed;
private System.Double m_Startupparametersfuelcapacity;
private System.Double m_Startupparametersinitialfuel;
private System.Double m_Startupparametersfuelconsumption;
private System.String m_Startupparametersicon;
private System.String m_Launcheventfueldepletionstate;
public LaunchEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Launcheventtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Launcheventtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Relativelocationx"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Relativelocationx = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Relativelocationy"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Relativelocationy = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Relativelocationz"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Relativelocationz = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Launcheventinitialstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Launcheventinitialstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridestealable"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridestealable = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridelaunchduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridelaunchduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridedockingduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridedockingduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridetimetoattack"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridetimetoattack = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverrideengagementduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverrideengagementduration = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridemaxspeed"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridemaxspeed = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridefuelcapacity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridefuelcapacity = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverrideinitialfuel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverrideinitialfuel = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridefuelconsumption"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridefuelconsumption = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverrideicon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverrideicon = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersoverridefueldepletionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersoverridefueldepletionstate = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersstealable"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Startupparametersstealable = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparameterslaunchduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparameterslaunchduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersdockingduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersdockingduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersengagementduration"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersengagementduration = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersmaxspeed"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersmaxspeed = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersfuelcapacity"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersfuelcapacity = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersinitialfuel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersinitialfuel = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersfuelconsumption"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Double)); 
if (value.Length > 0) { 
m_Startupparametersfuelconsumption = (System.Double)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Startupparametersicon"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Startupparametersicon = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Launcheventfueldepletionstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Launcheventfueldepletionstate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("LaunchEvent"), DescriptionAttribute("Time of move")]
public System.Decimal Launcheventtime { 
 get { return m_Launcheventtime; } 
set { m_Launcheventtime = value; } } 

[DisplayName("X"), DefaultValueAttribute(@"0"), CategoryAttribute("RelativeLocation"), DescriptionAttribute("")]
public System.Double Relativelocationx { 
 get { return m_Relativelocationx; } 
set { if (value > 9999999) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 9999999 "); } else { m_Relativelocationx = value; } } }

[DisplayName("Y"), DefaultValueAttribute(@"0"), CategoryAttribute("RelativeLocation"), DescriptionAttribute("")]
public System.Double Relativelocationy { 
 get { return m_Relativelocationy; } 
set { if (value > 9999999) {  throw new ArgumentException(" Could not satisfy constraint: max, value: 9999999 "); } else { m_Relativelocationy = value; } } }

[DisplayName("Z"), DefaultValueAttribute(@"0"), CategoryAttribute("RelativeLocation"), DescriptionAttribute("")]
public System.Double Relativelocationz { 
 get { return m_Relativelocationz; } 
set { m_Relativelocationz = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 

[DisplayName("InitialState"), DefaultValueAttribute(@"FullyFunctional"), CategoryAttribute("LaunchEvent"), DescriptionAttribute("")]
public System.String Launcheventinitialstate { 
 get { return m_Launcheventinitialstate; } 
set { m_Launcheventinitialstate = value; } } 

[DisplayName("OverrideStealable"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Is this unit stealable")]
public System.Boolean Startupparametersoverridestealable { 
 get { return m_Startupparametersoverridestealable; } 
set { m_Startupparametersoverridestealable = value; } } 

[DisplayName("OverrideLaunchDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridelaunchduration { 
 get { return m_Startupparametersoverridelaunchduration; } 
set { m_Startupparametersoverridelaunchduration = value; } } 

[DisplayName("OverrideDockingDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridedockingduration { 
 get { return m_Startupparametersoverridedockingduration; } 
set { m_Startupparametersoverridedockingduration = value; } } 

[DisplayName("OverrideTimeToAttack"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridetimetoattack { 
 get { return m_Startupparametersoverridetimetoattack; } 
set { m_Startupparametersoverridetimetoattack = value; } } 

[DisplayName("OverrideEngagementDuration"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Engagement Duration")]
public System.Boolean Startupparametersoverrideengagementduration { 
 get { return m_Startupparametersoverrideengagementduration; } 
set { m_Startupparametersoverrideengagementduration = value; } } 

[DisplayName("OverrideMaxSpeed"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridemaxspeed { 
 get { return m_Startupparametersoverridemaxspeed; } 
set { m_Startupparametersoverridemaxspeed = value; } } 

[DisplayName("OverrideFuelCapacity"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridefuelcapacity { 
 get { return m_Startupparametersoverridefuelcapacity; } 
set { m_Startupparametersoverridefuelcapacity = value; } } 

[DisplayName("OverrideInitialFuel"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverrideinitialfuel { 
 get { return m_Startupparametersoverrideinitialfuel; } 
set { m_Startupparametersoverrideinitialfuel = value; } } 

[DisplayName("OverrideFuelConsumption"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Boolean Startupparametersoverridefuelconsumption { 
 get { return m_Startupparametersoverridefuelconsumption; } 
set { m_Startupparametersoverridefuelconsumption = value; } } 

[DisplayName("OverrideIcon"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Image library key name of the icon.")]
public System.Boolean Startupparametersoverrideicon { 
 get { return m_Startupparametersoverrideicon; } 
set { m_Startupparametersoverrideicon = value; } } 

[DisplayName("OverrideFuelDepletionState"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Image library key name of the icon.")]
public System.Boolean Startupparametersoverridefueldepletionstate { 
 get { return m_Startupparametersoverridefueldepletionstate; } 
set { m_Startupparametersoverridefueldepletionstate = value; } } 

[DisplayName("Stealable"), DefaultValueAttribute(@"false"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Is this unit stealable")]
public System.Boolean Startupparametersstealable { 
 get { return m_Startupparametersstealable; } 
set { m_Startupparametersstealable = value; } } 

[DisplayName("LaunchDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparameterslaunchduration { 
 get { return m_Startupparameterslaunchduration; } 
set { m_Startupparameterslaunchduration = value; } } 

[DisplayName("DockingDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersdockingduration { 
 get { return m_Startupparametersdockingduration; } 
set { m_Startupparametersdockingduration = value; } } 

[DisplayName("EngagementDuration"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Engagement Duration")]
public System.Double Startupparametersengagementduration { 
 get { return m_Startupparametersengagementduration; } 
set { m_Startupparametersengagementduration = value; } } 

[DisplayName("MaxSpeed"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersmaxspeed { 
 get { return m_Startupparametersmaxspeed; } 
set { m_Startupparametersmaxspeed = value; } } 

[DisplayName("FuelCapacity"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersfuelcapacity { 
 get { return m_Startupparametersfuelcapacity; } 
set { m_Startupparametersfuelcapacity = value; } } 

[DisplayName("InitialFuel"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersinitialfuel { 
 get { return m_Startupparametersinitialfuel; } 
set { m_Startupparametersinitialfuel = value; } } 

[DisplayName("FuelConsumption"), DefaultValueAttribute(@"0"), CategoryAttribute("StartupParameters"), DescriptionAttribute("Launch Duration")]
public System.Double Startupparametersfuelconsumption { 
 get { return m_Startupparametersfuelconsumption; } 
set { m_Startupparametersfuelconsumption = value; } } 

[DisplayName("Icon"), DefaultValueAttribute(@""), CategoryAttribute("StartupParameters"), DescriptionAttribute("Image library key name of the icon.")]
public System.String Startupparametersicon { 
 get { return m_Startupparametersicon; } 
set { m_Startupparametersicon = value; } } 

[DisplayName("FuelDepletionState"), DefaultValueAttribute(@"Dead"), CategoryAttribute("LaunchEvent"), DescriptionAttribute("")]
public System.String Launcheventfueldepletionstate { 
 get { return m_Launcheventfueldepletionstate; } 
set { m_Launcheventfueldepletionstate = value; } } 
 } 
public class TransferEvent { 
private System.Decimal m_Transfereventtime;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
private System.Boolean m_Speciescompletionunit;
public TransferEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Transfereventtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Transfereventtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("TransferEvent"), DescriptionAttribute("Time of move")]
public System.Decimal Transfereventtime { 
 get { return m_Transfereventtime; } 
set { m_Transfereventtime = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 
 } 
public class StateChangeEvent { 
private System.Decimal m_Statechangeeventtime;
private System.Boolean m_Speciescompletionunit;
private System.String m_Statechangeeventstate;
private System.String m_Statechangeeventfromstate;
private System.String m_Statechangeeventexceptstate;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
public StateChangeEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Statechangeeventtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Statechangeeventtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statechangeeventstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Statechangeeventstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statechangeeventfromstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Statechangeeventfromstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Statechangeeventexceptstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Statechangeeventexceptstate = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("StateChangeEvent"), DescriptionAttribute("")]
public System.Decimal Statechangeeventtime { 
 get { return m_Statechangeeventtime; } 
set { m_Statechangeeventtime = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 

[DisplayName("State"), DefaultValueAttribute(@""), CategoryAttribute("StateChangeEvent"), DescriptionAttribute("")]
public System.String Statechangeeventstate { 
 get { return m_Statechangeeventstate; } 
set { m_Statechangeeventstate = value; } } 

[DisplayName("FromState"), DefaultValueAttribute(@""), CategoryAttribute("StateChangeEvent"), DescriptionAttribute("")]
public System.String Statechangeeventfromstate { 
 get { return m_Statechangeeventfromstate; } 
set { m_Statechangeeventfromstate = value; } } 

[DisplayName("ExceptState"), DefaultValueAttribute(@""), CategoryAttribute("StateChangeEvent"), DescriptionAttribute("")]
public System.String Statechangeeventexceptstate { 
 get { return m_Statechangeeventexceptstate; } 
set { m_Statechangeeventexceptstate = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 
 } 
public class ReiterateEvent { 
private System.Decimal m_Reiterateeventtime;
private System.String m_Engramrangeengramname;
private System.Boolean m_Engramrangeengramunitselected;
private System.Boolean m_Engramrangeengramperformingunit;
private System.String m_Engramrangeengramrangeinclude;
private System.String m_Engramrangeengramrangeexclude;
private System.String m_Engramrangeengramcompareinequality;
private System.String m_Engramrangeengramcomparevalue;
private System.String m_Engramrangeselectedengramtype;
public ReiterateEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Reiterateeventtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Reiterateeventtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramunitselected"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramunitselected = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramperformingunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Engramrangeengramperformingunit = (System.Boolean)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeinclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeinclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramrangeexclude"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramrangeexclude = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcompareinequality"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcompareinequality = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeengramcomparevalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeengramcomparevalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Engramrangeselectedengramtype"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Engramrangeselectedengramtype = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("ReiterateEvent"), DescriptionAttribute("Time of reiterate")]
public System.Decimal Reiterateeventtime { 
 get { return m_Reiterateeventtime; } 
set { m_Reiterateeventtime = value; } } 

[DisplayName("Engram Name"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramname { 
 get { return m_Engramrangeengramname; } 
set { m_Engramrangeengramname = value; } } 

[DisplayName("Engram Unit Selected"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramunitselected { 
 get { return m_Engramrangeengramunitselected; } 
set { m_Engramrangeengramunitselected = value; } } 

[DisplayName("Engram Performing Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.Boolean Engramrangeengramperformingunit { 
 get { return m_Engramrangeengramperformingunit; } 
set { m_Engramrangeengramperformingunit = value; } } 

[DisplayName("Engram Range Include"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeinclude { 
 get { return m_Engramrangeengramrangeinclude; } 
set { m_Engramrangeengramrangeinclude = value; } } 

[DisplayName("Engram Range Exclude"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramrangeexclude { 
 get { return m_Engramrangeengramrangeexclude; } 
set { m_Engramrangeengramrangeexclude = value; } } 

[DisplayName("Engram Compare Inequality"), DefaultValueAttribute(@"EQ"), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcompareinequality { 
 get { return m_Engramrangeengramcompareinequality; } 
set { m_Engramrangeengramcompareinequality = value; } } 

[DisplayName("Engram Compare Value"), DefaultValueAttribute(@""), CategoryAttribute("EngramRange"), DescriptionAttribute("")]
public System.String Engramrangeengramcomparevalue { 
 get { return m_Engramrangeengramcomparevalue; } 
set { m_Engramrangeengramcomparevalue = value; } } 

[DisplayName("Selected Engram Type"), DefaultValueAttribute(@"Include"), CategoryAttribute("EngramRange"), DescriptionAttribute("Compare, Include, Exclude")]
public System.String Engramrangeselectedengramtype { 
 get { return m_Engramrangeselectedengramtype; } 
set { m_Engramrangeselectedengramtype = value; } } 
 } 
public class OpenChatRoomEvent { 
private System.Decimal m_Openchatroomtime;
private System.String m_Openchatroomname;
public OpenChatRoomEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Openchatroomtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Openchatroomtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Openchatroomname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Openchatroomname = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("OpenChatRoom"), DescriptionAttribute("")]
public System.Decimal Openchatroomtime { 
 get { return m_Openchatroomtime; } 
set { m_Openchatroomtime = value; } } 

[DisplayName("Name"), DefaultValueAttribute(@""), CategoryAttribute("OpenChatRoom"), DescriptionAttribute("")]
public System.String Openchatroomname { 
 get { return m_Openchatroomname; } 
set { m_Openchatroomname = value; } } 
 } 
public class CloseChatRoomEvent { 
private System.Decimal m_Closechatroomtime;
public CloseChatRoomEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Closechatroomtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Closechatroomtime = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("CloseChatRoom"), DescriptionAttribute("")]
public System.Decimal Closechatroomtime { 
 get { return m_Closechatroomtime; } 
set { m_Closechatroomtime = value; } } 
 } 
public class SendChatMessageEvent { 
private System.Decimal m_Sendchatmessagetime;
private System.String m_Sendchatmessagemessage;
public SendChatMessageEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Sendchatmessagetime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Sendchatmessagetime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sendchatmessagemessage"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sendchatmessagemessage = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("SendChatMessage"), DescriptionAttribute("")]
public System.Decimal Sendchatmessagetime { 
 get { return m_Sendchatmessagetime; } 
set { m_Sendchatmessagetime = value; } } 

[DisplayName("Message"), DefaultValueAttribute(@""), CategoryAttribute("SendChatMessage"), DescriptionAttribute("")]
public System.String Sendchatmessagemessage { 
 get { return m_Sendchatmessagemessage; } 
set { m_Sendchatmessagemessage = value; } } 
 } 
public class OpenVoiceChannelEvent { 
private System.String m_Openvoicechannelname;
private System.Decimal m_Openvoicechanneltime;
public OpenVoiceChannelEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Openvoicechannelname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Openvoicechannelname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Openvoicechanneltime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Openvoicechanneltime = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Name"), DefaultValueAttribute(@""), CategoryAttribute("OpenVoiceChannel"), DescriptionAttribute("The name of the voice channel")]
public System.String Openvoicechannelname { 
 get { return m_Openvoicechannelname; } 
set { m_Openvoicechannelname = value; } } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("OpenVoiceChannel"), DescriptionAttribute("The time at which the voice channel is opened.")]
public System.Decimal Openvoicechanneltime { 
 get { return m_Openvoicechanneltime; } 
set { m_Openvoicechanneltime = value; } } 
 } 
public class CloseVoiceChannelEvent { 
private System.String m_Closevoicechannelname;
private System.Decimal m_Closevoicechanneltime;
public CloseVoiceChannelEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Closevoicechannelname"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Closevoicechannelname = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Closevoicechanneltime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Closevoicechanneltime = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Name"), DefaultValueAttribute(@""), CategoryAttribute("CloseVoiceChannel"), DescriptionAttribute("The name of the voice channel")]
public System.String Closevoicechannelname { 
 get { return m_Closevoicechannelname; } 
set { m_Closevoicechannelname = value; } } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("CloseVoiceChannel"), DescriptionAttribute("The time at which the voice channel is closed.")]
public System.Decimal Closevoicechanneltime { 
 get { return m_Closevoicechanneltime; } 
set { m_Closevoicechanneltime = value; } } 
 } 
public class SendVoiceMessageEvent { 
private System.Decimal m_Sendvoicemessagetime;
private System.String m_Sendvoicemessagefilepath;
private System.String m_Sendvoicemessagechannel;
public SendVoiceMessageEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Sendvoicemessagetime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Sendvoicemessagetime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sendvoicemessagefilepath"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sendvoicemessagefilepath = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sendvoicemessagechannel"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sendvoicemessagechannel = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("SendVoiceMessage"), DescriptionAttribute("")]
public System.Decimal Sendvoicemessagetime { 
 get { return m_Sendvoicemessagetime; } 
set { m_Sendvoicemessagetime = value; } } 

[DisplayName("FilePath"), DefaultValueAttribute(@""), CategoryAttribute("SendVoiceMessage"), DescriptionAttribute("")]
public System.String Sendvoicemessagefilepath { 
 get { return m_Sendvoicemessagefilepath; } 
set { m_Sendvoicemessagefilepath = value; } } 

[DisplayName("Channel"), DefaultValueAttribute(@""), CategoryAttribute("SendVoiceMessage"), DescriptionAttribute("")]
public System.String Sendvoicemessagechannel { 
 get { return m_Sendvoicemessagechannel; } 
set { m_Sendvoicemessagechannel = value; } } 
 } 
public class SendVoiceMessageToUserEvent { 
private System.Decimal m_Sendvoicemessagetousertime;
private System.String m_Sendvoicemessagetouserfilepath;
private System.String m_Sendvoicemessagetouserdecisionmakerid;
public SendVoiceMessageToUserEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Sendvoicemessagetousertime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Sendvoicemessagetousertime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sendvoicemessagetouserfilepath"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sendvoicemessagetouserfilepath = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Sendvoicemessagetouserdecisionmakerid"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Sendvoicemessagetouserdecisionmakerid = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("SendVoiceMessageToUser"), DescriptionAttribute("")]
public System.Decimal Sendvoicemessagetousertime { 
 get { return m_Sendvoicemessagetousertime; } 
set { m_Sendvoicemessagetousertime = value; } } 

[DisplayName("FilePath"), DefaultValueAttribute(@""), CategoryAttribute("SendVoiceMessageToUser"), DescriptionAttribute("")]
public System.String Sendvoicemessagetouserfilepath { 
 get { return m_Sendvoicemessagetouserfilepath; } 
set { m_Sendvoicemessagetouserfilepath = value; } } 

[DisplayName("DecisionMakerID"), DefaultValueAttribute(@""), CategoryAttribute("SendVoiceMessageToUser"), DescriptionAttribute("")]
public System.String Sendvoicemessagetouserdecisionmakerid { 
 get { return m_Sendvoicemessagetouserdecisionmakerid; } 
set { m_Sendvoicemessagetouserdecisionmakerid = value; } } 
 } 
public class ChangeEngramEvent { 
private System.Decimal m_Changeengramtime;
private System.String m_Changeengramvalue;
private System.Boolean m_Changeengramunitspecified;
public ChangeEngramEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Changeengramtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Changeengramtime = (System.Decimal)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Changeengramvalue"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Changeengramvalue = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Changeengramunitspecified"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Changeengramunitspecified = (System.Boolean)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("ChangeEngram"), DescriptionAttribute("")]
public System.Decimal Changeengramtime { 
 get { return m_Changeengramtime; } 
set { m_Changeengramtime = value; } } 

[DisplayName("Value"), DefaultValueAttribute(@""), CategoryAttribute("ChangeEngram"), DescriptionAttribute("")]
public System.String Changeengramvalue { 
 get { return m_Changeengramvalue; } 
set { m_Changeengramvalue = value; } } 

[DisplayName("Unit Specified"), DefaultValueAttribute(@"false"), CategoryAttribute("ChangeEngram"), DescriptionAttribute("")]
public System.Boolean Changeengramunitspecified { 
 get { return m_Changeengramunitspecified; } 
set { m_Changeengramunitspecified = value; } } 
 } 
public class RemoveEngramEvent { 
private System.Decimal m_Removeengramtime;
public RemoveEngramEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Removeengramtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Decimal)); 
if (value.Length > 0) { 
m_Removeengramtime = (System.Decimal)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("RemoveEngram"), DescriptionAttribute("")]
public System.Decimal Removeengramtime { 
 get { return m_Removeengramtime; } 
set { m_Removeengramtime = value; } } 
 } 
public class SpeciesCompletionEvent { 
private System.String m_Speciescompletioneventaction;
private System.String m_Speciescompletioneventstate;
public SpeciesCompletionEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Speciescompletioneventaction"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Speciescompletioneventaction = (System.String)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletioneventstate"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.String)); 
if (value.Length > 0) { 
m_Speciescompletioneventstate = (System.String)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Action"), DefaultValueAttribute(@""), CategoryAttribute("SpeciesCompletionEvent"), DescriptionAttribute("Type of action that triggers the completion.")]
public System.String Speciescompletioneventaction { 
 get { return m_Speciescompletioneventaction; } 
set { m_Speciescompletioneventaction = value; } } 

[DisplayName("State"), DefaultValueAttribute(@""), CategoryAttribute("SpeciesCompletionEvent"), DescriptionAttribute("")]
public System.String Speciescompletioneventstate { 
 get { return m_Speciescompletioneventstate; } 
set { m_Speciescompletioneventstate = value; } } 
 } 
public class FlushEvent { 
private System.UInt32 m_Flushtime;
private System.Boolean m_Speciescompletionunit;
public FlushEvent() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Flushtime"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.UInt32)); 
if (value.Length > 0) { 
m_Flushtime = (System.UInt32)converter.ConvertFromString(value); }
}

attributes = TypeDescriptor.GetProperties(this)["Speciescompletionunit"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Boolean)); 
if (value.Length > 0) { 
m_Speciescompletionunit = (System.Boolean)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Time"), DefaultValueAttribute(@"1"), CategoryAttribute("Flush"), DescriptionAttribute("")]
public System.UInt32 Flushtime { 
 get { return m_Flushtime; } 
set { m_Flushtime = value; } } 

[DisplayName("Unit"), DefaultValueAttribute(@"false"), CategoryAttribute("SpeciesCompletion"), DescriptionAttribute("")]
public System.Boolean Speciescompletionunit { 
 get { return m_Speciescompletionunit; } 
set { m_Speciescompletionunit = value; } } 
 } 
public class SpeciesSubplatformCapacitySpeciesSpecies { 
private System.Int32 m_Capacitycount;
public SpeciesSubplatformCapacitySpeciesSpecies() { 
AttributeCollection attributes; 
DefaultValueAttribute myAttribute; 
TypeConverter converter; 
String value; 
attributes = TypeDescriptor.GetProperties(this)["Capacitycount"].Attributes; 
myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)]; 
if (myAttribute != null) {value = myAttribute.Value.ToString(); 
converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Int32)); 
if (value.Length > 0) { 
m_Capacitycount = (System.Int32)converter.ConvertFromString(value); }
}

 } 

[DisplayName("Count"), DefaultValueAttribute(@"0"), CategoryAttribute("Capacity")]
public System.Int32 Capacitycount { 
 get { return m_Capacitycount; } 
set { m_Capacitycount = value; } } 
 } 
 }