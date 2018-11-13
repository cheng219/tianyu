//=======================================
//作者：吴江
//日期：2015/5/20
//用途：网络消息记录
//========================================




using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System.IO;
using System;

public class NetRecorder : MonoBehaviour {

    private static FileWrite fileWriter;
	void Start () {
        if (fileWriter != null)
            return;

        if (!Application.isEditor)
        {
            #if UNITY_ANDROID
            return;
            #endif
        }

          DontDestroyOnLoad(this);
        string path = Application.dataPath;
        path = Path.GetDirectoryName(path);
        path = path + "/.record";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        fileWriter = new TxtFileWriter(path, System.DateTime.Now.ToFileTime() + ".txt");
 
      
    }
	
	// Update is called once per frame
    void OnApplicationQuit()
    {
        if (fileWriter != null)
        {
            fileWriter.close();
        }
	}

    public static void recordNetCmd(Pt _pt)
    {
        if (fileWriter != null)
        {
            float time = Time.realtimeSinceStartup;

            CmdStep step = new CmdStep(time, _pt.reader.data);
            fileWriter.writeCmdStep(step);
        }
    }


    public class TxtFileWriter : FileWrite
    {

         private StreamWriter file;
         public TxtFileWriter(string path, string name)
             : base(path, name)
         {
             file = new FileInfo(path + "/" + name).CreateText();
         }


         public override void writeCmdStep(CmdStep step)
         {
             float time =step.time;
             string str = time + " : ";
             if (step.datas != null)
                 for (int i = 0; i < step.datas.Length; i++)
                 {
                     str += step.datas[i];
                     if (i != step.datas.Length - 1)
                         str += " ";
                 }
             file.WriteLine(str);
         }

         public override void close()
         {
             file.Close();
             file.Dispose();
         }
         
     }

     public class BinFileWriter : FileWrite
     {

         private FileStream file;/////
         public BinFileWriter(string path, string name)
             : base(path, name)
         {

             file = new FileStream(path + "/" + name, FileMode.CreateNew);

         }

         public override void writeCmdStep(CmdStep step)
         {
            

             byte[] buffer = floatToByteArry(step.time);
             for(int i=0;i<buffer.Length;i++)
                 file.WriteByte(buffer[i]);

             byte cout = (byte)(step.datas.Length);
             file.WriteByte(cout);
             for (int i = 0; i < cout; i++)
              file.WriteByte(step.datas[i]);

            

         }


         public override void close()
         {
             file.Close();
             file.Dispose();
         }

         public static float byteArryToFloat(byte[] buffer)
         {

             int num4 = buffer[0] & 0xff;
             num4 += (buffer[1] & 0xff) << 8;
             num4 += (buffer[2] & 0xff) << 0x10;
             num4 += (buffer[3] & 0xff) << 0x18;
             int num5 = ((num4 & 0x80000000L) == 0L) ? 1 : -1;
             int num6 = (num4 & 0x7f800000) >> 0x17;
             int num7 = num4 & 0x7fffff;
             num7 |= 0x800000;
             return (float)((num5 * num7) * Math.Pow(2.0, (double)(num6 - 150)));
         }


         public static byte[] floatToByteArry(float a)
         {
             byte[] bytes = BitConverter.GetBytes(a);
             if (bytes.Length != 4)
             {
                 throw new IOException("float converter error!");
             }
             byte[] buffer = new byte[] { bytes[0], bytes[1], bytes[2], bytes[3] };
             return buffer;
         }
        
     }


     public abstract class FileWrite
     {
         
         public FileWrite(string path, string name)
         {
            
         }

         public abstract void writeCmdStep(CmdStep step);
         public abstract void close();

     }

     public class CmdStep
     {
         public float time;
         public byte[] datas;
         public CmdStep(float time, byte[] datas)
         {
             this.time = time;
             this.datas = datas;
         }
     }
}
