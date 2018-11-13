using System.Collections;
using System.Collections.Generic;

public class pt_other_ply_info_d810 : st.net.NetBase.Pt {
	public pt_other_ply_info_d810()
	{
		Id = 0xD810;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_other_ply_info_d810();
	}
	public List<st.net.NetBase.scene_ply> ply_list = new List<st.net.NetBase.scene_ply>();
	public List<st.net.NetBase.scene_entourage> entourage_list = new List<st.net.NetBase.scene_entourage>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenply_list = reader.Read_ushort();
		ply_list = new List<st.net.NetBase.scene_ply>();
		for(int i_ply_list = 0 ; i_ply_list < lenply_list ; i_ply_list ++)
		{
			st.net.NetBase.scene_ply listData = new st.net.NetBase.scene_ply();
			listData.fromBinary(reader);
			ply_list.Add(listData);
		}
		ushort lenentourage_list = reader.Read_ushort();
		entourage_list = new List<st.net.NetBase.scene_entourage>();
		for(int i_entourage_list = 0 ; i_entourage_list < lenentourage_list ; i_entourage_list ++)
		{
			st.net.NetBase.scene_entourage listData = new st.net.NetBase.scene_entourage();
			listData.fromBinary(reader);
			entourage_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenply_list = (ushort)ply_list.Count;
		writer.write_short(lenply_list);
		for(int i_ply_list = 0 ; i_ply_list < lenply_list ; i_ply_list ++)
		{
			st.net.NetBase.scene_ply listData = ply_list[i_ply_list];
			listData.toBinary(writer);
		}
		ushort lenentourage_list = (ushort)entourage_list.Count;
		writer.write_short(lenentourage_list);
		for(int i_entourage_list = 0 ; i_entourage_list < lenentourage_list ; i_entourage_list ++)
		{
			st.net.NetBase.scene_entourage listData = entourage_list[i_entourage_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
