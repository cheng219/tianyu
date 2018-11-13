using System.Collections;
using System.Collections.Generic;

public class pt_lost_item_d009 : st.net.NetBase.Pt {
	public pt_lost_item_d009()
	{
		Id = 0xD009;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lost_item_d009();
	}
	public uint lost_item_total_num;
	public uint active_id;
	public List<st.net.NetBase.lost_list> lost_item_list = new List<st.net.NetBase.lost_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		lost_item_total_num = reader.Read_uint();
		active_id = reader.Read_uint();
		ushort lenlost_item_list = reader.Read_ushort();
		lost_item_list = new List<st.net.NetBase.lost_list>();
		for(int i_lost_item_list = 0 ; i_lost_item_list < lenlost_item_list ; i_lost_item_list ++)
		{
			st.net.NetBase.lost_list listData = new st.net.NetBase.lost_list();
			listData.fromBinary(reader);
			lost_item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(lost_item_total_num);
		writer.write_int(active_id);
		ushort lenlost_item_list = (ushort)lost_item_list.Count;
		writer.write_short(lenlost_item_list);
		for(int i_lost_item_list = 0 ; i_lost_item_list < lenlost_item_list ; i_lost_item_list ++)
		{
			st.net.NetBase.lost_list listData = lost_item_list[i_lost_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
