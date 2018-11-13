using System.Collections;
using System.Collections.Generic;

public class pt_guild_liveness_info_d50e : st.net.NetBase.Pt {
	public pt_guild_liveness_info_d50e()
	{
		Id = 0xD50E;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_liveness_info_d50e();
	}
	public uint liveness_guild;
	public uint liveness_self;
	public List<uint> reward_list = new List<uint>();
	public List<st.net.NetBase.guild_liveness_member_info> member_info_list = new List<st.net.NetBase.guild_liveness_member_info>();
	public List<st.net.NetBase.guild_liveness_task_info> task_list = new List<st.net.NetBase.guild_liveness_task_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		liveness_guild = reader.Read_uint();
		liveness_self = reader.Read_uint();
		ushort lenreward_list = reader.Read_ushort();
		reward_list = new List<uint>();
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			uint listData = reader.Read_uint();
			reward_list.Add(listData);
		}
		ushort lenmember_info_list = reader.Read_ushort();
		member_info_list = new List<st.net.NetBase.guild_liveness_member_info>();
		for(int i_member_info_list = 0 ; i_member_info_list < lenmember_info_list ; i_member_info_list ++)
		{
			st.net.NetBase.guild_liveness_member_info listData = new st.net.NetBase.guild_liveness_member_info();
			listData.fromBinary(reader);
			member_info_list.Add(listData);
		}
		ushort lentask_list = reader.Read_ushort();
		task_list = new List<st.net.NetBase.guild_liveness_task_info>();
		for(int i_task_list = 0 ; i_task_list < lentask_list ; i_task_list ++)
		{
			st.net.NetBase.guild_liveness_task_info listData = new st.net.NetBase.guild_liveness_task_info();
			listData.fromBinary(reader);
			task_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(liveness_guild);
		writer.write_int(liveness_self);
		ushort lenreward_list = (ushort)reward_list.Count;
		writer.write_short(lenreward_list);
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			uint listData = reward_list[i_reward_list];
			writer.write_int(listData);
		}
		ushort lenmember_info_list = (ushort)member_info_list.Count;
		writer.write_short(lenmember_info_list);
		for(int i_member_info_list = 0 ; i_member_info_list < lenmember_info_list ; i_member_info_list ++)
		{
			st.net.NetBase.guild_liveness_member_info listData = member_info_list[i_member_info_list];
			listData.toBinary(writer);
		}
		ushort lentask_list = (ushort)task_list.Count;
		writer.write_short(lentask_list);
		for(int i_task_list = 0 ; i_task_list < lentask_list ; i_task_list ++)
		{
			st.net.NetBase.guild_liveness_task_info listData = task_list[i_task_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
