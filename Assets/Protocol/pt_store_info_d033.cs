using System.Collections;
using System.Collections.Generic;

public class pt_store_info_d033 : st.net.NetBase.Pt {
	public pt_store_info_d033()
	{
		Id = 0xD033;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_store_info_d033();
	}
	public uint store_id;
	public uint auto_fresh_time;
	public uint fresh_times;
	public List<st.net.NetBase.store_cell_info> cell_list = new List<st.net.NetBase.store_cell_info>();
	public List<st.net.NetBase.store_cell_info> show_cell_list = new List<st.net.NetBase.store_cell_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		store_id = reader.Read_uint();
		auto_fresh_time = reader.Read_uint();
		fresh_times = reader.Read_uint();
		ushort lencell_list = reader.Read_ushort();
		cell_list = new List<st.net.NetBase.store_cell_info>();
		for(int i_cell_list = 0 ; i_cell_list < lencell_list ; i_cell_list ++)
		{
			st.net.NetBase.store_cell_info listData = new st.net.NetBase.store_cell_info();
			listData.fromBinary(reader);
			cell_list.Add(listData);
		}
		ushort lenshow_cell_list = reader.Read_ushort();
		show_cell_list = new List<st.net.NetBase.store_cell_info>();
		for(int i_show_cell_list = 0 ; i_show_cell_list < lenshow_cell_list ; i_show_cell_list ++)
		{
			st.net.NetBase.store_cell_info listData = new st.net.NetBase.store_cell_info();
			listData.fromBinary(reader);
			show_cell_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(store_id);
		writer.write_int(auto_fresh_time);
		writer.write_int(fresh_times);
		ushort lencell_list = (ushort)cell_list.Count;
		writer.write_short(lencell_list);
		for(int i_cell_list = 0 ; i_cell_list < lencell_list ; i_cell_list ++)
		{
			st.net.NetBase.store_cell_info listData = cell_list[i_cell_list];
			listData.toBinary(writer);
		}
		ushort lenshow_cell_list = (ushort)show_cell_list.Count;
		writer.write_short(lenshow_cell_list);
		for(int i_show_cell_list = 0 ; i_show_cell_list < lenshow_cell_list ; i_show_cell_list ++)
		{
			st.net.NetBase.store_cell_info listData = show_cell_list[i_show_cell_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
