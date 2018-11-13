using System.Collections;
using System.Collections.Generic;

public class pt_lucky_wheel_reward_d965 : st.net.NetBase.Pt {
	public pt_lucky_wheel_reward_d965()
	{
		Id = 0xD965;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lucky_wheel_reward_d965();
	}
	public List<st.net.NetBase.lucky_wheel_reward_info> reward_list = new List<st.net.NetBase.lucky_wheel_reward_info>();
	public uint jackpot;
	public byte flag;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenreward_list = reader.Read_ushort();
		reward_list = new List<st.net.NetBase.lucky_wheel_reward_info>();
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			st.net.NetBase.lucky_wheel_reward_info listData = new st.net.NetBase.lucky_wheel_reward_info();
			listData.fromBinary(reader);
			reward_list.Add(listData);
		}
		jackpot = reader.Read_uint();
		flag = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenreward_list = (ushort)reward_list.Count;
		writer.write_short(lenreward_list);
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			st.net.NetBase.lucky_wheel_reward_info listData = reward_list[i_reward_list];
			listData.toBinary(writer);
		}
		writer.write_int(jackpot);
		writer.write_byte(flag);
		return writer.data;
	}

}
