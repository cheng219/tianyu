using System;
using System.Text;
using System.Collections;

public class ByteAr_b
{
	public static byte[] PickBytes(byte[] data, int len, byte[] get_bytes, int off = 0)
	{
		if (data.Length < off + len)
			return null;
		Array.Copy(data, off, get_bytes, 0, len);
		return data;
	}
	
	public static byte[] GetBytes(byte[] data, int len, byte[] get_bytes, int off = 0)
	{
		if (data.Length < off + len)
			return null;
		Array.Copy(data, off, get_bytes, 0, len);
		
		byte[] ret = null;
		if (off > 0)
		{
			ret = new byte[data.Length - len];
			Array.Copy(data, 0, ret, 0, off);
			Array.Copy(data, off + len, ret, off, ret.Length - off);
		}
		else
		{
			ret = new byte[data.Length - len];
			Array.Copy(data, len, ret, 0, ret.Length);
		}
		
		return ret;
	}
	
	public static byte[] WriteBytes(byte[] data, byte[] write_bytes)
	{
		byte[] ret = new byte[data.Length + write_bytes.Length];
		Array.Copy(data, 0, ret, 0, data.Length);
		Array.Copy(write_bytes, 0, ret, data.Length, write_bytes.Length);
		return ret;
	}

	public static byte[] f2b(float i)
	{
		byte[] ret = BitConverter.GetBytes(i);
		Array.Reverse(ret);
		return ret;
	}

	public static float b2f(byte[] bytes)
	{
		if (bytes.Length < 4)
			return 0;
		Array.Reverse(bytes);
        float ret = BitConverter.ToSingle(bytes, 0);
		return ret;
	}
	
	public static byte[] i2b(uint i)
	{
		byte[] ret = new byte[4];
		ret[0] = (byte)((i & 0xFF000000) >> 24);
		ret[1] = (byte)((i & 0xFF0000) >> 16);
		ret[2] = (byte)((i & 0xFF00) >> 8);
		ret[3] = (byte)((i & 0xFF));
		return ret;
	}
	public static uint b2ui(byte[] bytes)
	{
		if (bytes.Length < 4)
			return 0;
		uint ret = 0;
		ret |= (uint)bytes[0] << 24;
		ret |= (uint)bytes[1] << 16;
		ret |= (uint)bytes[2] << 8;
		ret |= (uint)bytes[3];
		return ret;
	}
	public static byte[] i2b(int i)
	{
		byte[] ret = new byte[4];
		ret[0] = (byte)((i & 0xFF000000) >> 24);
		ret[1] = (byte)((i & 0xFF0000) >> 16);
		ret[2] = (byte)((i & 0xFF00) >> 8);
		ret[3] = (byte)((i & 0xFF));
		return ret;
	}
	public static int b2i(byte[] bytes)
	{
		if (bytes.Length < 4)
			return 0;
		int ret = 0;
		ret |= (int)bytes[0] << 24;
		ret |= (int)bytes[1] << 16;
		ret |= (int)bytes[2] << 8;
		ret |= (int)bytes[3];
		return ret;
	}
	
	public static byte[] l2b(ulong i)
	{
		byte[] ret = new byte[8];
		ret[0] = (byte)((i & 0xFF00000000000000) >> 56);
		ret[1] = (byte)((i & 0xFF000000000000) >> 48);
		ret[2] = (byte)((i & 0xFF0000000000) >> 40);
		ret[3] = (byte)((i & 0xFF00000000) >> 32);
		ret[4] = (byte)((i & 0xFF000000) >> 24);
		ret[5] = (byte)((i & 0xFF0000) >> 16);
		ret[6] = (byte)((i & 0xFF00) >> 8);
		ret[7] = (byte)((i & 0xFF));
		return ret;
	}
	public static ulong b2ul(byte[] bytes)
	{
		if (bytes.Length < 8)
			return 0;
		ulong ret = 0;
		ret |= (ulong)bytes[0] << 56;
		ret |= (ulong)bytes[1] << 48;
		ret |= (ulong)bytes[2] << 40;
		ret |= (ulong)bytes[3] << 32;
		ret |= (ulong)bytes[4] << 24;
		ret |= (ulong)bytes[5] << 16;
		ret |= (ulong)bytes[6] << 8;
		ret |= (ulong)bytes[7];
		return ret;
	}
	public static byte[] l2b(long i)
	{
		byte[] ret = new byte[8];
        ret[0] = (byte)((i >> 56) & 0xFF);
		ret[1] = (byte)((i & 0xFF000000000000) >> 48);
		ret[2] = (byte)((i & 0xFF0000000000) >> 40);
		ret[3] = (byte)((i & 0xFF00000000) >> 32);
		ret[4] = (byte)((i & 0xFF000000) >> 24);
		ret[5] = (byte)((i & 0xFF0000) >> 16);
		ret[6] = (byte)((i & 0xFF00) >> 8);
		ret[7] = (byte)((i & 0xFF));
		return ret;
	}
	public static long b2l(byte[] bytes)
	{
		if (bytes.Length < 8)
			return 0;
		long ret = 0;
		ret |= (long)bytes[0] << 56;
		ret |= (long)bytes[1] << 48;
		ret |= (long)bytes[2] << 40;
		ret |= (long)bytes[3] << 32;
		ret |= (long)bytes[4] << 24;
		ret |= (long)bytes[5] << 16;
		ret |= (long)bytes[6] << 8;
		ret |= (long)bytes[7];
		return ret;
	}
	public static byte[] s2b(ushort i)
	{
		byte[] ret = new byte[2];
		ret[0] = (byte)((i & 0xFF00) >> 8);
		ret[1] = (byte)((i & 0xFF));
		return ret;
	}
	public static ushort b2us(byte[] bytes)
	{
		if (bytes.Length < 2)
			return 0;
		ushort ret = 0;
		ret |= (ushort)(bytes[0] << 8);
		ret |= (ushort)bytes[1] ;
		return ret;
	}
	public static byte[] s2b(short i)
	{
		byte[] ret = new byte[2];
		ret[0] = (byte)((i & 0xFF00) >> 8);
		ret[1] = (byte)((i & 0xFF));
		return ret;
	}
	public static short b2s(byte[] bytes)
	{
		if (bytes.Length < 2)
			return 0;
		short ret = 0;
		ret |= (short)(bytes[0] << 8);
		ret |= (short)bytes[1];
		return ret;
	}
	public static byte[] b2b(byte i)
	{
		byte[] ret = new byte[1];
		ret[0] = i;
		return ret;
	}
	public static byte b2b(byte[] bytes)
	{
		if (bytes.Length < 1)
			return 0;
		return bytes[0];
	}
	public static byte[] str2b(string str)
	{
		byte[] bytes_buf = new byte[str.Length * 4];
		ushort len = (ushort)Encoding.UTF8.GetBytes(str, 0, str.Length, bytes_buf, 0);
		byte[] str_buf = new byte[len];
		Array.Copy(bytes_buf, 0, str_buf, 0, len);
		byte[] len_buf = s2b(len);
		byte[] ret = new byte[len_buf.Length + str_buf.Length];
		Array.Copy(len_buf, 0, ret, 0, len_buf.Length);
		Array.Copy(str_buf, 0, ret, len_buf.Length, str_buf.Length);
		return ret;
	}
	public static string b2str(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
	}
}
