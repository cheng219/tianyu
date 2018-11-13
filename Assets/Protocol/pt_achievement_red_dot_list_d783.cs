using System.Collections;
using System.Collections.Generic;

public class pt_achievement_red_dot_list_d783 : st.net.NetBase.Pt {
	public pt_achievement_red_dot_list_d783()
	{
		Id = 0xD783;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_achievement_red_dot_list_d783();
	}
	public List<st.net.NetBase.red_dot_list> red_dot_list = new List<st.net.NetBase.red_dot_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenred_dot_list = reader.Read_ushort();
		red_dot_list = new List<st.net.NetBase.red_dot_list>();
		for(int i_red_dot_list = 0 ; i_red_dot_list < lenred_dot_list ; i_red_dot_list ++)
		{
			st.net.NetBase.red_dot_list listData = new st.net.NetBase.red_dot_list();
			listData.fromBinary(reader);
			red_dot_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenred_dot_list = (ushort)red_dot_list.Count;
		writer.write_short(lenred_dot_list);
		for(int i_red_dot_list = 0 ; i_red_dot_list < lenred_dot_list ; i_red_dot_list ++)
		{
			st.net.NetBase.red_dot_list listData = red_dot_list[i_red_dot_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
