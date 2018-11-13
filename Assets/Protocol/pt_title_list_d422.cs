using System.Collections;
using System.Collections.Generic;

public class pt_title_list_d422 : st.net.NetBase.Pt {
	public pt_title_list_d422()
	{
		Id = 0xD422;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_title_list_d422();
	}
	public List<st.net.NetBase.title_base_info_list> title_list = new List<st.net.NetBase.title_base_info_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentitle_list = reader.Read_ushort();
		title_list = new List<st.net.NetBase.title_base_info_list>();
		for(int i_title_list = 0 ; i_title_list < lentitle_list ; i_title_list ++)
		{
			st.net.NetBase.title_base_info_list listData = new st.net.NetBase.title_base_info_list();
			listData.fromBinary(reader);
			title_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentitle_list = (ushort)title_list.Count;
		writer.write_short(lentitle_list);
		for(int i_title_list = 0 ; i_title_list < lentitle_list ; i_title_list ++)
		{
			st.net.NetBase.title_base_info_list listData = title_list[i_title_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
