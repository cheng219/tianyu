using System.Collections;
using System.Collections.Generic;

public class pt_scene_add_trap_c015 : st.net.NetBase.Pt {
	public pt_scene_add_trap_c015()
	{
		Id = 0xC015;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_add_trap_c015();
	}
	public List<st.net.NetBase.scene_trap> list = new List<st.net.NetBase.scene_trap>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenlist = reader.Read_ushort();
		list = new List<st.net.NetBase.scene_trap>();
		for(int i_list = 0 ; i_list < lenlist ; i_list ++)
		{
			st.net.NetBase.scene_trap listData = new st.net.NetBase.scene_trap();
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
			st.net.NetBase.scene_trap listData = list[i_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
