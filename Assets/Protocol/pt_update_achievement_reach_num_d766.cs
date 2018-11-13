using System.Collections;
using System.Collections.Generic;

public class pt_update_achievement_reach_num_d766 : st.net.NetBase.Pt {
	public pt_update_achievement_reach_num_d766()
	{
		Id = 0xD766;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_achievement_reach_num_d766();
	}
	public List<st.net.NetBase.achievement_reach> achievement_reach = new List<st.net.NetBase.achievement_reach>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenachievement_reach = reader.Read_ushort();
		achievement_reach = new List<st.net.NetBase.achievement_reach>();
		for(int i_achievement_reach = 0 ; i_achievement_reach < lenachievement_reach ; i_achievement_reach ++)
		{
			st.net.NetBase.achievement_reach listData = new st.net.NetBase.achievement_reach();
			listData.fromBinary(reader);
			achievement_reach.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenachievement_reach = (ushort)achievement_reach.Count;
		writer.write_short(lenachievement_reach);
		for(int i_achievement_reach = 0 ; i_achievement_reach < lenachievement_reach ; i_achievement_reach ++)
		{
			st.net.NetBase.achievement_reach listData = achievement_reach[i_achievement_reach];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
