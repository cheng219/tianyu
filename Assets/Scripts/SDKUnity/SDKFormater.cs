using System;
//using System.Collections;
using System.Reflection;
//using System.IO;
//using System.Text;

public struct DGConstructorInfo
{
	public ConstructorInfo ci;
	public string version;

	public DGConstructorInfo(ConstructorInfo _constructorInfo,  string _version)
	{
		ci = _constructorInfo;
		version = _version;
	}
}


public class SDKFormater {


	static SDKFormater() { }
	
	/*
		5.0 接口修改   增加version 字段 
		 
	*/
	public static DGConstructorInfo getSDKConstructor(string platform)
	{
		Assembly ass = Assembly.GetExecutingAssembly();
		Type[] types = ass.GetTypes();
		foreach (Type type in types)
		{
			SDKAttribute attr = Attribute.GetCustomAttribute(type, typeof(SDKAttribute), true) as SDKAttribute;
			if (attr == null) continue;

			if (attr.Platform == platform)
			{
				ConstructorInfo ci = type.GetConstructor(new Type[] { typeof(string) });
				return new DGConstructorInfo(ci,  attr.Version);
			}
		}
		return new DGConstructorInfo(null,  "0.0");
	}




}
