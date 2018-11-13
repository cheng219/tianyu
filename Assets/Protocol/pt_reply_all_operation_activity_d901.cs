using System.Collections;
using System.Collections.Generic;

public class pt_reply_all_operation_activity_d901 : st.net.NetBase.Pt {
	public pt_reply_all_operation_activity_d901()
	{
		Id = 0xD901;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_all_operation_activity_d901();
	}
	public List<st.net.NetBase.operation_activity_title_info> title_info_list = new List<st.net.NetBase.operation_activity_title_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentitle_info_list = reader.Read_ushort();
		title_info_list = new List<st.net.NetBase.operation_activity_title_info>();
		for(int i_title_info_list = 0 ; i_title_info_list < lentitle_info_list ; i_title_info_list ++)
		{
			st.net.NetBase.operation_activity_title_info listData = new st.net.NetBase.operation_activity_title_info();
			listData.fromBinary(reader);
			title_info_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentitle_info_list = (ushort)title_info_list.Count;
		writer.write_short(lentitle_info_list);
		for(int i_title_info_list = 0 ; i_title_info_list < lentitle_info_list ; i_title_info_list ++)
		{
			st.net.NetBase.operation_activity_title_info listData = title_info_list[i_title_info_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
