using System.Collections;
using System.Collections.Generic;

public class pt_guild_item_chg_d506 : st.net.NetBase.Pt {
	public pt_guild_item_chg_d506()
	{
		Id = 0xD506;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_item_chg_d506();
	}
	public List<st.net.NetBase.item_des> guild_item = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_item = reader.Read_ushort();
		guild_item = new List<st.net.NetBase.item_des>();
		for(int i_guild_item = 0 ; i_guild_item < lenguild_item ; i_guild_item ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			guild_item.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_item = (ushort)guild_item.Count;
		writer.write_short(lenguild_item);
		for(int i_guild_item = 0 ; i_guild_item < lenguild_item ; i_guild_item ++)
		{
			st.net.NetBase.item_des listData = guild_item[i_guild_item];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
