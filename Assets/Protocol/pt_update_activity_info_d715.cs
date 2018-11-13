using System.Collections;
using System.Collections.Generic;

public class pt_update_activity_info_d715 : st.net.NetBase.Pt {
	public pt_update_activity_info_d715()
	{
		Id = 0xD715;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_activity_info_d715();
	}
	public List<st.net.NetBase.activity_list> activity_list = new List<st.net.NetBase.activity_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenactivity_list = reader.Read_ushort();
		activity_list = new List<st.net.NetBase.activity_list>();
		for(int i_activity_list = 0 ; i_activity_list < lenactivity_list ; i_activity_list ++)
		{
			st.net.NetBase.activity_list listData = new st.net.NetBase.activity_list();
			listData.fromBinary(reader);
			activity_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenactivity_list = (ushort)activity_list.Count;
		writer.write_short(lenactivity_list);
		for(int i_activity_list = 0 ; i_activity_list < lenactivity_list ; i_activity_list ++)
		{
			st.net.NetBase.activity_list listData = activity_list[i_activity_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
