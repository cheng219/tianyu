using System.Collections;
using System.Collections.Generic;

public class pt_scene_hide_c020 : st.net.NetBase.Pt {
	public pt_scene_hide_c020()
	{
		Id = 0xC020;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_hide_c020();
	}
	public List<st.net.NetBase.scene_objs> hide_list = new List<st.net.NetBase.scene_objs>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenhide_list = reader.Read_ushort();
		hide_list = new List<st.net.NetBase.scene_objs>();
		for(int i_hide_list = 0 ; i_hide_list < lenhide_list ; i_hide_list ++)
		{
			st.net.NetBase.scene_objs listData = new st.net.NetBase.scene_objs();
			listData.fromBinary(reader);
			hide_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenhide_list = (ushort)hide_list.Count;
		writer.write_short(lenhide_list);
		for(int i_hide_list = 0 ; i_hide_list < lenhide_list ; i_hide_list ++)
		{
			st.net.NetBase.scene_objs listData = hide_list[i_hide_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
