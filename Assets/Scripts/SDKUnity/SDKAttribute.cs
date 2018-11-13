using System;
//using System.Reflection;


[AttributeUsage(AttributeTargets.Class)]
public class SDKAttribute : System.Attribute {

	private string platform;
	private string version;

	public SDKAttribute(string _platform, string _version )
	{
		this.platform=_platform;
		this.version=_version;
	}

	public string Platform
	{
		get 
		{
			return this.platform;
		}
	}

	public string Version
	{
		get
		{
			return this.version;
		}
	}
}
