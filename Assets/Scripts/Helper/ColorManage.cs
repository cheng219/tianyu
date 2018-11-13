using UnityEngine;
using System.Collections;
//using System;

public static class ColorManage
{
	public static Color GetColor(string hexVal)
	{
		//int c = Convert.ToInt32(hexVal,16);
		uint c = uint.Parse(hexVal,System.Globalization.NumberStyles.HexNumber);
//		float tmp = c&0xff;
//		float b = tmp/0xff;
//		c >>= 8;
//		tmp = c&0xff;
//		float g = tmp/0xff;
//		c >>= 8;
//		tmp = c&0xff;
//		float r = tmp/0xff;
//		return new Color(r,g,b);
		return GetColor(c);
	}
	
	public static Color GetColor(uint hexVal)
	{
		float tmp = hexVal&0xff;
		float b = tmp/0xff;
		hexVal >>= 8;
		tmp = hexVal&0xff;
		float g = tmp/0xff;
		hexVal >>= 8;
		tmp = hexVal&0xff;
		float r = tmp/0xff;
		return new Color(r,g,b);
	}
	
	public static string GetColorHexVal(Color c)
	{
		uint tmp = (uint)(c.r*0xff) & 0xff;
		tmp <<= 8;
		tmp += (uint)(c.g*0xff) & 0xff;
		tmp <<= 8;
		tmp += (uint)(c.b*0xff) & 0xff;
		return string.Format("{0:X6}",tmp);
	}
}
