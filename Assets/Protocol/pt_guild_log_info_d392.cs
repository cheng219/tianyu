using System.Collections;
using System.Collections.Generic;

public class pt_guild_log_info_d392 : st.net.NetBase.Pt {
	public pt_guild_log_info_d392()
	{
		Id = 0xD392;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_log_info_d392();
	}
	public List<st.net.NetBase.guild_log> logs = new List<st.net.NetBase.guild_log>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenlogs = reader.Read_ushort();
		logs = new List<st.net.NetBase.guild_log>();
		for(int i_logs = 0 ; i_logs < lenlogs ; i_logs ++)
		{
			st.net.NetBase.guild_log listData = new st.net.NetBase.guild_log();
			listData.fromBinary(reader);
			logs.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenlogs = (ushort)logs.Count;
		writer.write_short(lenlogs);
		for(int i_logs = 0 ; i_logs < lenlogs ; i_logs ++)
		{
			st.net.NetBase.guild_log listData = logs[i_logs];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
