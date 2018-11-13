using System.Collections;
using System.Collections.Generic;

public class pt_entourage_list_d010 : st.net.NetBase.Pt {
	public pt_entourage_list_d010()
	{
		Id = 0xD010;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_entourage_list_d010();
	}
	public List<st.net.NetBase.entourage_list> entourage_list = new List<st.net.NetBase.entourage_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenentourage_list = reader.Read_ushort();
		entourage_list = new List<st.net.NetBase.entourage_list>();
		for(int i_entourage_list = 0 ; i_entourage_list < lenentourage_list ; i_entourage_list ++)
		{
			st.net.NetBase.entourage_list listData = new st.net.NetBase.entourage_list();
			listData.fromBinary(reader);
			entourage_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenentourage_list = (ushort)entourage_list.Count;
		writer.write_short(lenentourage_list);
		for(int i_entourage_list = 0 ; i_entourage_list < lenentourage_list ; i_entourage_list ++)
		{
			st.net.NetBase.entourage_list listData = entourage_list[i_entourage_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
