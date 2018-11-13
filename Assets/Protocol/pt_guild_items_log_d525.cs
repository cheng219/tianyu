using System.Collections;
using System.Collections.Generic;

public class pt_guild_items_log_d525 : st.net.NetBase.Pt {
	public pt_guild_items_log_d525()
	{
		Id = 0xD525;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_items_log_d525();
	}
	public List<st.net.NetBase.guild_items_log_list> guild_item_logs = new List<st.net.NetBase.guild_items_log_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_item_logs = reader.Read_ushort();
		guild_item_logs = new List<st.net.NetBase.guild_items_log_list>();
		for(int i_guild_item_logs = 0 ; i_guild_item_logs < lenguild_item_logs ; i_guild_item_logs ++)
		{
			st.net.NetBase.guild_items_log_list listData = new st.net.NetBase.guild_items_log_list();
			listData.fromBinary(reader);
			guild_item_logs.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_item_logs = (ushort)guild_item_logs.Count;
		writer.write_short(lenguild_item_logs);
		for(int i_guild_item_logs = 0 ; i_guild_item_logs < lenguild_item_logs ; i_guild_item_logs ++)
		{
			st.net.NetBase.guild_items_log_list listData = guild_item_logs[i_guild_item_logs];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
