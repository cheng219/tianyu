using System.Collections;
using System.Collections.Generic;

public class pt_scene_drop_c011 : st.net.NetBase.Pt {
	public pt_scene_drop_c011()
	{
		Id = 0xC011;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_drop_c011();
	}
	public uint oid;
	public List<st.net.NetBase.drop_des> drop_list = new List<st.net.NetBase.drop_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		ushort lendrop_list = reader.Read_ushort();
		drop_list = new List<st.net.NetBase.drop_des>();
		for(int i_drop_list = 0 ; i_drop_list < lendrop_list ; i_drop_list ++)
		{
			st.net.NetBase.drop_des listData = new st.net.NetBase.drop_des();
			listData.fromBinary(reader);
			drop_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		ushort lendrop_list = (ushort)drop_list.Count;
		writer.write_short(lendrop_list);
		for(int i_drop_list = 0 ; i_drop_list < lendrop_list ; i_drop_list ++)
		{
			st.net.NetBase.drop_des listData = drop_list[i_drop_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
