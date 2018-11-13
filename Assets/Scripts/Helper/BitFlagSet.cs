using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Bit flag set.
/// </summary>

public class BitFlagSet
{
	
	private byte[] m_aBitFlags;
	private uint m_nBitSize;
	
	//构造
	public BitFlagSet(uint bitSize)
	{
		m_nBitSize=bitSize;
		uint len = ((m_nBitSize - 1) >> 3) + 1;
        m_aBitFlags = new byte[len];
	}

	//设置所有标记位
	public void MarkAllFlags()
	{
        for (int i = 0; i < m_aBitFlags.Length; i++)
        {
			m_aBitFlags[i]=0xff;			
		}				
	}
	//清除所有标记位
	public void  ClearAllFlags()
	{
        Array.Clear(m_aBitFlags, 0x00, m_aBitFlags.Length);				
	}	
	//取指定的标记位
	public bool GetFlagByIndex(int nIdx)
	{
		if(0 > nIdx || m_nBitSize <= nIdx)
			return false;

		return 0 != (m_aBitFlags[nIdx>>3] & (byte)(1 << (nIdx % 8)));
	}		
	//清除指定的标记位
	public void ClearFlagByIndex(int nIdx)
	{
		if(0 > nIdx || m_nBitSize <= nIdx)
			return ;
		
	     m_aBitFlags[nIdx>>3] &= (byte)~(0x01 << (nIdx % 8));
	}

	//设置指定标记位
	public void MarkFlagByIndex(int nIdx)
	{
		if(0 > nIdx || m_nBitSize <= nIdx)
			return ;
        //Debug.Log("nIdx = " + nIdx + ",nIdx >> 3 = " + (nIdx >> 3));
		m_aBitFlags[nIdx >> 3] |= (byte)(0x01 << (nIdx % 8));
	}
	//所占用的标记数
	public uint Size(){ return m_nBitSize;}
			
}