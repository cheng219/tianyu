using System.Collections;
using System.Collections.Generic;

public class pt_wing_list_info_d417 : st.net.NetBase.Pt {
	public pt_wing_list_info_d417()
	{
		Id = 0xD417;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_wing_list_info_d417();
	}
	public List<st.net.NetBase.wing_base_info> wing_list = new List<st.net.NetBase.wing_base_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenwing_list = reader.Read_ushort();
		wing_list = new List<st.net.NetBase.wing_base_info>();
		for(int i_wing_list = 0 ; i_wing_list < lenwing_list ; i_wing_list ++)
		{
			st.net.NetBase.wing_base_info listData = new st.net.NetBase.wing_base_info();
			listData.fromBinary(reader);
			wing_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenwing_list = (ushort)wing_list.Count;
		writer.write_short(lenwing_list);
		for(int i_wing_list = 0 ; i_wing_list < lenwing_list ; i_wing_list ++)
		{
			st.net.NetBase.wing_base_info listData = wing_list[i_wing_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
