using System.Collections;
using System.Collections.Generic;

public class pt_reply_royal_box_list_d941 : st.net.NetBase.Pt {
	public pt_reply_royal_box_list_d941()
	{
		Id = 0xD941;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_royal_box_list_d941();
	}
	public uint rest_acc_times;
	public List<st.net.NetBase.royal_box_info> box_list = new List<st.net.NetBase.royal_box_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rest_acc_times = reader.Read_uint();
		ushort lenbox_list = reader.Read_ushort();
		box_list = new List<st.net.NetBase.royal_box_info>();
		for(int i_box_list = 0 ; i_box_list < lenbox_list ; i_box_list ++)
		{
			st.net.NetBase.royal_box_info listData = new st.net.NetBase.royal_box_info();
			listData.fromBinary(reader);
			box_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rest_acc_times);
		ushort lenbox_list = (ushort)box_list.Count;
		writer.write_short(lenbox_list);
		for(int i_box_list = 0 ; i_box_list < lenbox_list ; i_box_list ++)
		{
			st.net.NetBase.royal_box_info listData = box_list[i_box_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
