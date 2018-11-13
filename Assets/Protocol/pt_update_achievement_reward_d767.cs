using System.Collections;
using System.Collections.Generic;

public class pt_update_achievement_reward_d767 : st.net.NetBase.Pt {
	public pt_update_achievement_reward_d767()
	{
		Id = 0xD767;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_achievement_reward_d767();
	}
	public List<st.net.NetBase.achievement_reward> achievement_reward = new List<st.net.NetBase.achievement_reward>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenachievement_reward = reader.Read_ushort();
		achievement_reward = new List<st.net.NetBase.achievement_reward>();
		for(int i_achievement_reward = 0 ; i_achievement_reward < lenachievement_reward ; i_achievement_reward ++)
		{
			st.net.NetBase.achievement_reward listData = new st.net.NetBase.achievement_reward();
			listData.fromBinary(reader);
			achievement_reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenachievement_reward = (ushort)achievement_reward.Count;
		writer.write_short(lenachievement_reward);
		for(int i_achievement_reward = 0 ; i_achievement_reward < lenachievement_reward ; i_achievement_reward ++)
		{
			st.net.NetBase.achievement_reward listData = achievement_reward[i_achievement_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
