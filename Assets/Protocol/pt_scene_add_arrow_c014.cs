using System.Collections;
using System.Collections.Generic;

public class pt_scene_add_arrow_c014 : st.net.NetBase.Pt {
	public pt_scene_add_arrow_c014()
	{
		Id = 0xC014;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_add_arrow_c014();
	}
	public List<st.net.NetBase.scene_arrow> list = new List<st.net.NetBase.scene_arrow>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenlist = reader.Read_ushort();
		list = new List<st.net.NetBase.scene_arrow>();
		for(int i_list = 0 ; i_list < lenlist ; i_list ++)
		{
			st.net.NetBase.scene_arrow listData = new st.net.NetBase.scene_arrow();
			listData.fromBinary(reader);
			list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenlist = (ushort)list.Count;
		writer.write_short(lenlist);
		for(int i_list = 0 ; i_list < lenlist ; i_list ++)
		{
			st.net.NetBase.scene_arrow listData = list[i_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
