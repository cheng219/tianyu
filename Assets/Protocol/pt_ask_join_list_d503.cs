using System.Collections;
using System.Collections.Generic;

public class pt_ask_join_list_d503 : st.net.NetBase.Pt {
	public pt_ask_join_list_d503()
	{
		Id = 0xD503;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ask_join_list_d503();
	}
	public int open_state;
	public int fight_score;
	public List<st.net.NetBase.ask_join_guild_list> ask_join_info = new List<st.net.NetBase.ask_join_guild_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		open_state = reader.Read_int();
		fight_score = reader.Read_int();
		ushort lenask_join_info = reader.Read_ushort();
		ask_join_info = new List<st.net.NetBase.ask_join_guild_list>();
		for(int i_ask_join_info = 0 ; i_ask_join_info < lenask_join_info ; i_ask_join_info ++)
		{
			st.net.NetBase.ask_join_guild_list listData = new st.net.NetBase.ask_join_guild_list();
			listData.fromBinary(reader);
			ask_join_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(open_state);
		writer.write_int(fight_score);
		ushort lenask_join_info = (ushort)ask_join_info.Count;
		writer.write_short(lenask_join_info);
		for(int i_ask_join_info = 0 ; i_ask_join_info < lenask_join_info ; i_ask_join_info ++)
		{
			st.net.NetBase.ask_join_guild_list listData = ask_join_info[i_ask_join_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
