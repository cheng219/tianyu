using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using System;

public class UnpackZip : MonoBehaviour{
	private float progress = 0;
	public float Progress
	{
		get
		{
			return progress;
		}
		set
		{
			progress = value;
		}
	}


    protected long hasUnPackCount = 0;
    public long HasUnPackCount
    {
        get
        {
            return hasUnPackCount;
        }
        set
        {
            hasUnPackCount = value;
        }
    }

    protected long totalUnPackCount = 0;
    public long TotalUnPackCount
    {
        get
        {
            return totalUnPackCount;
        }
        set
        {
            totalUnPackCount = value;
        }
    }

    private string curFileName = string.Empty;
    /// <summary>
    /// 当前正在解压的文件名
    /// </summary>
    public string CurFileName
    {
        get
        {
            return curFileName;
        }
        protected set
        {
            curFileName = value;
        }
    }

	public static UnpackZip GetInstance(GameObject go)
	{
		UnpackZip unpack = go.GetComponent<UnpackZip>();
		if(unpack == null)
		{
			unpack = go.AddComponent<UnpackZip>();
		}
		return unpack;
	}
	/// <summary>
	/// Starts the unpackasync.
	/// </summary>
	/// 
	 public void StartUnpack1(string file,string dir)
	{
		StartCoroutine(UnpackAsync1(file,dir));
	}
	public void StartUnpack2(byte[] mbyte,string dir)
	{
		StartCoroutine(UnpackAsync2(mbyte,dir));
	}
	IEnumerator UnpackAsync1(string file, string dir)
	{
		int index = 0;
		long hasUnpack=0;
	    if (!Directory.Exists(dir))
	     	Directory.CreateDirectory(dir);	
		using(FileStream streamRead =File.OpenRead(file))
		{
			byte[] data = new byte[2048];
     		ZipInputStream s = new ZipInputStream (streamRead);
			ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry= s.GetNextEntry();
			while(theEntry!=null)
			{
				index++;
				if(index%100==0)
					yield return 0;
			    string directoryName = Path.GetDirectoryName(theEntry.Name);
			    string fileName = Path.GetFileName(theEntry.Name);	
			    if (directoryName != String.Empty)
			      	Directory.CreateDirectory(dir + directoryName);
				if (fileName != String.Empty)
		     	{
                    CurFileName = theEntry.Name;
					using(FileStream streamWriter = File.Create(dir + theEntry.Name))
						{
							int size = s.Read(data, 0, data.Length);
					        while (size > 0)
					      	{
								streamWriter.Write(data, 0, size);
								size = s.Read(data, 0, data.Length);
					      	}
							streamWriter.Close();
						}
				}
				hasUnpack +=theEntry.CompressedSize;
				Progress   = (float)hasUnpack/( float)streamRead.Length;
                //HasUnPackCount = hasUnpack;
                //TotalUnPackCount = streamRead.Length;
				//Debug.Log(index.ToString()+" "+ hasUnpack +"/" +streamRead.Length);
				theEntry= s.GetNextEntry();
			}
			Progress = 1;
			s.Close();
            GC.Collect();
		}
	}
	
	IEnumerator UnpackAsync2(byte[] mbyte, string dir)
	{
		int index = 0;
		long hasUnpack=0;
	    if (!Directory.Exists(dir))
	     	Directory.CreateDirectory(dir);	
		using(MemoryStream streamRead =new MemoryStream(mbyte))
		{
			byte[] data = new byte[2048];
     		ZipInputStream s = new ZipInputStream (streamRead);
			ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry= s.GetNextEntry();
			while(theEntry!=null)
			{
				index++;
//				NGUIDebug.Log(index.ToString()+" "+ hasUnpack +"/" +streamRead.Length);
				if(index%100==0)
					yield return 0;
				Progress   = (float)hasUnpack/( float)streamRead.Length;
			    string directoryName = Path.GetDirectoryName(theEntry.Name);
			    string fileName = Path.GetFileName(theEntry.Name);	
			    if (directoryName != String.Empty)
			      	Directory.CreateDirectory(dir + directoryName);
				if (fileName != String.Empty)
		     	{
                    CurFileName = theEntry.Name;
					using(FileStream streamWriter = File.Create(dir + theEntry.Name))
						{
							int size = s.Read(data, 0, data.Length);
					        while (size > 0)
					      	{
								hasUnpack +=size;
								streamWriter.Write(data, 0, size);
								size = s.Read(data, 0, data.Length);
					      	}
							streamWriter.Close();
						}
				}
				theEntry= s.GetNextEntry();
			}
			Progress = 1;
			s.Close();
            System.GC.Collect();
		}
	}
	
	IEnumerator UnpackAsync(string file, string dir,int count)
	{
		int index = 0;
	    if (!Directory.Exists(dir))
	     	Directory.CreateDirectory(dir);	
    	ICSharpCode.SharpZipLib.Zip.ZipInputStream s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(file));	
    	ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;
		
		
		byte[] data = new byte[1024];
	   	while ((theEntry = s.GetNextEntry()) != null)
	   	{					
			index++;
			if(index%(count/50) == 0)
				yield return 0;
			Progress = (float)index/(float)count;
		    string directoryName = Path.GetDirectoryName(theEntry.Name);
		    string fileName = Path.GetFileName(theEntry.Name);	
		    if (directoryName != String.Empty)
		      	Directory.CreateDirectory(dir + directoryName);
	
	     	if (fileName != String.Empty)
	     	{
//		      	FileStream streamWriter = File.Create(dir + theEntry.Name);	
//				int size = 2048;
//		        while (true)
//		      	{
//					size = s.Read(data, 0, data.Length);
//					if (size > 0)
//					{
//						streamWriter.Write(data, 0, size);
//					}
//					else
//					{
//						break;
//					}
//		      	}
//	      		streamWriter.Close();
                CurFileName = theEntry.Name;
						using(FileStream streamWriter = File.Create(dir + theEntry.Name))
						{
							int size = s.Read(data, 0, data.Length);
					        while (size > 0)
					      	{
								streamWriter.Write(data, 0, size);
								size = s.Read(data, 0, data.Length);
					      	}
							streamWriter.Close();
						}
	    	}
		}
		Progress = 1;
		s.Close();
        GC.Collect();
	}

	/// <summary>
	/// Unpack the file no progress.
	/// </summary>
	public void Unpack(string file, string dir)
	{
		try
	   	{
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
	
	    	ICSharpCode.SharpZipLib.Zip.ZipInputStream s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(file));	
	    	ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry;
			
			byte[] data = new byte[2048];
		   	while ((theEntry = s.GetNextEntry()) != null)
		   	{
			    string directoryName = Path.GetDirectoryName(theEntry.Name);
			    string fileName = Path.GetFileName(theEntry.Name);
                if (directoryName != String.Empty)
                {
                    Directory.CreateDirectory(dir + directoryName);
                }
		
		     	if (fileName != String.Empty)
		     	{
                    CurFileName = theEntry.Name;
			      	FileStream streamWriter = File.Create(dir + theEntry.Name);	
					int size = 2048;
			        while (true)
			      	{
						size = s.Read(data, 0, data.Length);
						if (size > 0)
						{
							streamWriter.Write(data, 0, size);
						}
						else
						{
							break;
						}
			      	}
		      		streamWriter.Close();
		    	}
			}
			s.Close();
            GC.Collect();
	   	}catch (Exception)
	   	{
	    	throw;
	   	}
	}
}