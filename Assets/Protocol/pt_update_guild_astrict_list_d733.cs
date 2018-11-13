using System.Collections;
using System.Collections.Generic;

public class pt_update_guild_astrict_list_d733 : st.net.NetBase.Pt {
	public pt_update_guild_astrict_list_d733()
	{
		Id = 0xD733;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_guild_astrict_list_d733();
	}
	public List<st.net.NetBase.astrict_item_list> astrict_item_list = new List<st.net.NetBase.astrict_item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenastrict_item_list = reader.Read_ushort();
		astrict_item_list = new List<st.net.NetBase.astrict_item_list>();
		for(int i_astrict_item_list = 0 ; i_astrict_item_list < lenastrict_item_list ; i_astrict_item_list ++)
		{
			st.net.NetBase.astrict_item_list listData = new st.net.NetBase.astrict_item_list();
			listData.fromBinary(reader);
			astrict_item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenastrict_item_list = (ushort)astrict_item_list.Count;
		writer.write_short(lenastrict_item_list);
		for(int i_astrict_item_list = 0 ; i_astrict_item_list < lenastrict_item_list ; i_astrict_item_list ++)
		{
			st.net.NetBase.astrict_item_list listData = astrict_item_list[i_astrict_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
