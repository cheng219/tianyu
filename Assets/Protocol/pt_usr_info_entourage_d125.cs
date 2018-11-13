using System.Collections;
using System.Collections.Generic;

public class pt_usr_info_entourage_d125 : st.net.NetBase.Pt {
	public pt_usr_info_entourage_d125()
	{
		Id = 0xD125;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_info_entourage_d125();
	}
	public List<st.net.NetBase.entourage_info_list> entourage_info_list = new List<st.net.NetBase.entourage_info_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenentourage_info_list = reader.Read_ushort();
		entourage_info_list = new List<st.net.NetBase.entourage_info_list>();
		for(int i_entourage_info_list = 0 ; i_entourage_info_list < lenentourage_info_list ; i_entourage_info_list ++)
		{
			st.net.NetBase.entourage_info_list listData = new st.net.NetBase.entourage_info_list();
			listData.fromBinary(reader);
			entourage_info_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenentourage_info_list = (ushort)entourage_info_list.Count;
		writer.write_short(lenentourage_info_list);
		for(int i_entourage_info_list = 0 ; i_entourage_info_list < lenentourage_info_list ; i_entourage_info_list ++)
		{
			st.net.NetBase.entourage_info_list listData = entourage_info_list[i_entourage_info_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
