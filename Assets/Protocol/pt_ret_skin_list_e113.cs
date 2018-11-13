using System.Collections;
using System.Collections.Generic;

public class pt_ret_skin_list_e113 : st.net.NetBase.Pt {
	public pt_ret_skin_list_e113()
	{
		Id = 0xE113;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_skin_list_e113();
	}
	public List<st.net.NetBase.skin_base_info> skin_list = new List<st.net.NetBase.skin_base_info>();
	public int skin_lev;
	public int skin_exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenskin_list = reader.Read_ushort();
		skin_list = new List<st.net.NetBase.skin_base_info>();
		for(int i_skin_list = 0 ; i_skin_list < lenskin_list ; i_skin_list ++)
		{
			st.net.NetBase.skin_base_info listData = new st.net.NetBase.skin_base_info();
			listData.fromBinary(reader);
			skin_list.Add(listData);
		}
		skin_lev = reader.Read_int();
		skin_exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenskin_list = (ushort)skin_list.Count;
		writer.write_short(lenskin_list);
		for(int i_skin_list = 0 ; i_skin_list < lenskin_list ; i_skin_list ++)
		{
			st.net.NetBase.skin_base_info listData = skin_list[i_skin_list];
			listData.toBinary(writer);
		}
		writer.write_int(skin_lev);
		writer.write_int(skin_exp);
		return writer.data;
	}

}
