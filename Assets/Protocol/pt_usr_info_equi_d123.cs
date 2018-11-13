using System.Collections;
using System.Collections.Generic;

public class pt_usr_info_equi_d123 : st.net.NetBase.Pt {
	public pt_usr_info_equi_d123()
	{
		Id = 0xD123;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_info_equi_d123();
	}
	public uint pid;
	public string name;
	public string guild_name;
	public uint prof;
	public uint lev;
	public uint title;
	public List<st.net.NetBase.item_des> item_list = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		pid = reader.Read_uint();
		name = reader.Read_str();
		guild_name = reader.Read_str();
		prof = reader.Read_uint();
		lev = reader.Read_uint();
		title = reader.Read_uint();
		ushort lenitem_list = reader.Read_ushort();
		item_list = new List<st.net.NetBase.item_des>();
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(pid);
		writer.write_str(name);
		writer.write_str(guild_name);
		writer.write_int(prof);
		writer.write_int(lev);
		writer.write_int(title);
		ushort lenitem_list = (ushort)item_list.Count;
		writer.write_short(lenitem_list);
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_des listData = item_list[i_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
