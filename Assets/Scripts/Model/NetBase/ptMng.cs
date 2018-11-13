using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace st.net.NetBase
{

    public class ByteReader
    {
        public byte[] data = null;
        public int read_indx = 0;

        public ByteReader(byte[] data)
        {
            this.data = data;
            read_indx = 0;
        }

        public byte[] Read_bytes(int len)
        {
            if (this.data.Length < read_indx + len)
                return null;

            byte[] getbytes = new byte[len];
            ByteAr_b.PickBytes(this.data, len, getbytes, read_indx);
            read_indx += len;
            return getbytes;
        }
        public float Read_float()
        {
            byte[] bytes = Read_bytes(4);
            return ByteAr_b.b2f(bytes);
        }
        public long Read_long()
		{
			byte[] bytes = Read_bytes(8);
			return ByteAr_b.b2l(bytes);
		}
		public ulong Read_ulong()
		{
			byte[] bytes = Read_bytes(8);
			return ByteAr_b.b2ul(bytes);
		}
        public int Read_int()
        {
            byte[] bytes = Read_bytes(4);
            return ByteAr_b.b2i(bytes);
        }
        public uint Read_uint()
        {
            byte[] bytes = Read_bytes(4);
            return ByteAr_b.b2ui(bytes);
        }
        public short Read_short()
        {
            byte[] bytes = Read_bytes(2);
            return ByteAr_b.b2s(bytes);
        }
        public ushort Read_ushort()
        {
            byte[] bytes = Read_bytes(2);
            return ByteAr_b.b2us(bytes);
        }
        public byte Read_byte()
        {
            byte[] bytes = Read_bytes(1);
            return ByteAr_b.b2b(bytes);
        }
        public string Read_str()
        {
            ushort len = Read_ushort();
            byte[] bytes = Read_bytes(len);
            return ByteAr_b.b2str(bytes); ;
        }
    }

    public class ByteWriter
    {
        public byte[] data = new byte[0];
        public bool write_byte(byte data)
        {
            return write_bytes(ByteAr_b.b2b(data));
        }
        public bool write_short(short data)
        {
            return write_bytes(ByteAr_b.s2b(data));
        }
        public bool write_short(ushort data)
        {
            return write_bytes(ByteAr_b.s2b(data));
        }
        public bool write_int(int data)
        {
            return write_bytes(ByteAr_b.i2b(data));
        }
        public bool write_int(uint data)
        {
            return write_bytes(ByteAr_b.i2b(data));
        }
        public bool write_long(long data)
		{
			return write_bytes(ByteAr_b.l2b(data));
		}
		public bool write_long(ulong data)
		{
			return write_bytes(ByteAr_b.l2b(data));
		}
        public bool write_float(float data)
        {
            return write_bytes(ByteAr_b.f2b(data));
        }
        public bool write_str(string str)
        {
            return write_bytes(ByteAr_b.str2b(str));
        }
        public bool write_bytes(byte[] bytes)
        {
            if (this.data == null)
                this.data = bytes;
            else
            {
                byte[] all = new byte[this.data.Length + bytes.Length];
                Array.Copy(this.data, 0, all, 0, this.data.Length);
                Array.Copy(bytes, 0, all, this.data.Length, bytes.Length);
                this.data = all;
            }
            return true;
        }
    }

    public abstract class Pt
    {
        public static ushort Id;
        public uint seq = 0;
        public ByteReader reader;
        public ByteWriter writer;

        public ushort GetID() { return Id; }
        public abstract Pt createNew();
        public abstract void fromBinary(byte[] binary);
        public abstract byte[] toBinary();
    }

    public class PtMsg : EventArgs
    {
        public PtMsg(byte[] pt)
        {
            this.bytes = pt;
        }
        public byte[] bytes = null;
    }

/*    public class PtMsg : EventArgs
    {
        public PtMsg(Pt pt)
        {
            this.pt = pt;
        }
        public Pt pt = null;
    }*/

    public class NetErr : EventArgs
    {
        public NetErr(string err)
        {
            this.err = err;
        }
        public string err = "";
    }

    public class ptMng
    {
		public class PtRigstInfo
		{
			public CreateNewPt CreateNewFun;
			public ProcessPt ProcessPtFun;
			public PtRigstInfo(CreateNewPt _CreateNewFun,ProcessPt _ProcessPtFun)
			{
				CreateNewFun = _CreateNewFun;
				ProcessPtFun = _ProcessPtFun;
			}
		}
        static ptMng _mng = null;

		public delegate Pt CreateNewPt();
		public delegate void ProcessPt(Pt pt);
		Dictionary<ushort, PtRigstInfo> dicPts = new Dictionary<ushort, PtRigstInfo>();
        public static ptMng GetInstance()
        {
            if (_mng == null)
            {
                _mng = new ptMng();
            }

            return _mng;
        }

		public void RegistPt(ushort pt_id,CreateNewPt CreateNewFun,ProcessPt ProcessPtFun)
        {
			if (!dicPts.ContainsKey(pt_id))
            {
				dicPts.Add(pt_id, new PtRigstInfo(CreateNewFun,ProcessPtFun));
            }
        }

		public void UnregistPt(ushort pt_id)
        {
			if (dicPts.ContainsKey(pt_id))
            {
				dicPts.Remove(pt_id);
            }
        }

        public void ClearRegistPt()
        {
            dicPts.Clear();
        }

		public Pt MakePt(ushort pt_id,uint pt_seq, byte[] data)
        {
            if (dicPts.ContainsKey(pt_id))
            {
				CreateNewPt CreateNewFun = dicPts[pt_id].CreateNewFun;
				Pt pt = CreateNewFun();
                pt.fromBinary(data);
				pt.seq = pt_seq;
                return pt;
            }

            return null;
        }

		public void PrcPt(st.net.NetBase.Pt pt)
		{
			if (dicPts.ContainsKey(pt.GetID()))
			{
				ProcessPt ProcessPtFun = dicPts[pt.GetID()].ProcessPtFun;
				ProcessPtFun(pt);
			}
		}
    }
}