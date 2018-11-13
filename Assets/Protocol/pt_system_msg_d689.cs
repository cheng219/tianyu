using System.Collections;
using System.Collections.Generic;

public class pt_system_msg_d689 : st.net.NetBase.Pt {
	public pt_system_msg_d689()
	{
		Id = 0xD689;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_system_msg_d689();
	}
	public int type;
	public List<st.net.NetBase.normal_info> nomal_list = new List<st.net.NetBase.normal_info>();
	public List<st.net.NetBase.item_des> item = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		ushort lennomal_list = reader.Read_ushort();
		nomal_list = new List<st.net.NetBase.normal_info>();
		for(int i_nomal_list = 0 ; i_nomal_list < lennomal_list ; i_nomal_list ++)
		{
			st.net.NetBase.normal_info listData = new st.net.NetBase.normal_info();
			listData.fromBinary(reader);
			nomal_list.Add(listData);
		}
		ushort lenitem = reader.Read_ushort();
		item = new List<st.net.NetBase.item_des>();
		for(int i_item = 0 ; i_item < lenitem ; i_item ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			item.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		ushort lennomal_list = (ushort)nomal_list.Count;
		writer.write_short(lennomal_list);
		for(int i_nomal_list = 0 ; i_nomal_list < lennomal_list ; i_nomal_list ++)
		{
			st.net.NetBase.normal_info listData = nomal_list[i_nomal_list];
			listData.toBinary(writer);
		}
		ushort lenitem = (ushort)item.Count;
		writer.write_short(lenitem);
		for(int i_item = 0 ; i_item < lenitem ; i_item ++)
		{
			st.net.NetBase.item_des listData = item[i_item];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
