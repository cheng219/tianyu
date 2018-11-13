using System.Collections;
using System.Collections.Generic;

public class pt_usr_info_lost_item_d126 : st.net.NetBase.Pt {
	public pt_usr_info_lost_item_d126()
	{
		Id = 0xD126;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_info_lost_item_d126();
	}
	public List<st.net.NetBase.lost_item_info> lost_item_info = new List<st.net.NetBase.lost_item_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenlost_item_info = reader.Read_ushort();
		lost_item_info = new List<st.net.NetBase.lost_item_info>();
		for(int i_lost_item_info = 0 ; i_lost_item_info < lenlost_item_info ; i_lost_item_info ++)
		{
			st.net.NetBase.lost_item_info listData = new st.net.NetBase.lost_item_info();
			listData.fromBinary(reader);
			lost_item_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenlost_item_info = (ushort)lost_item_info.Count;
		writer.write_short(lenlost_item_info);
		for(int i_lost_item_info = 0 ; i_lost_item_info < lenlost_item_info ; i_lost_item_info ++)
		{
			st.net.NetBase.lost_item_info listData = lost_item_info[i_lost_item_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
