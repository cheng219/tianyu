using System.Collections;
using System.Collections.Generic;

public class pt_cast_soul_info_d446 : st.net.NetBase.Pt {
	public pt_cast_soul_info_d446()
	{
		Id = 0xD446;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_cast_soul_info_d446();
	}
	public List<st.net.NetBase.base_list> base_soul = new List<st.net.NetBase.base_list>();
	public List<st.net.NetBase.advanced_list> advanced_soul = new List<st.net.NetBase.advanced_list>();
	public int cur_cast_soul;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenbase_soul = reader.Read_ushort();
		base_soul = new List<st.net.NetBase.base_list>();
		for(int i_base_soul = 0 ; i_base_soul < lenbase_soul ; i_base_soul ++)
		{
			st.net.NetBase.base_list listData = new st.net.NetBase.base_list();
			listData.fromBinary(reader);
			base_soul.Add(listData);
		}
		ushort lenadvanced_soul = reader.Read_ushort();
		advanced_soul = new List<st.net.NetBase.advanced_list>();
		for(int i_advanced_soul = 0 ; i_advanced_soul < lenadvanced_soul ; i_advanced_soul ++)
		{
			st.net.NetBase.advanced_list listData = new st.net.NetBase.advanced_list();
			listData.fromBinary(reader);
			advanced_soul.Add(listData);
		}
		cur_cast_soul = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenbase_soul = (ushort)base_soul.Count;
		writer.write_short(lenbase_soul);
		for(int i_base_soul = 0 ; i_base_soul < lenbase_soul ; i_base_soul ++)
		{
			st.net.NetBase.base_list listData = base_soul[i_base_soul];
			listData.toBinary(writer);
		}
		ushort lenadvanced_soul = (ushort)advanced_soul.Count;
		writer.write_short(lenadvanced_soul);
		for(int i_advanced_soul = 0 ; i_advanced_soul < lenadvanced_soul ; i_advanced_soul ++)
		{
			st.net.NetBase.advanced_list listData = advanced_soul[i_advanced_soul];
			listData.toBinary(writer);
		}
		writer.write_int(cur_cast_soul);
		return writer.data;
	}

}
