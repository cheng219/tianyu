using System.Collections;
using System.Collections.Generic;

public class pt_brother_reward_info_d543 : st.net.NetBase.Pt {
	public pt_brother_reward_info_d543()
	{
		Id = 0xD543;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_brother_reward_info_d543();
	}
	public List<st.net.NetBase.brother_reward_info> reward_list = new List<st.net.NetBase.brother_reward_info>();
	public List<st.net.NetBase.sworn_task_finish_info> task_finish_list = new List<st.net.NetBase.sworn_task_finish_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenreward_list = reader.Read_ushort();
		reward_list = new List<st.net.NetBase.brother_reward_info>();
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			st.net.NetBase.brother_reward_info listData = new st.net.NetBase.brother_reward_info();
			listData.fromBinary(reader);
			reward_list.Add(listData);
		}
		ushort lentask_finish_list = reader.Read_ushort();
		task_finish_list = new List<st.net.NetBase.sworn_task_finish_info>();
		for(int i_task_finish_list = 0 ; i_task_finish_list < lentask_finish_list ; i_task_finish_list ++)
		{
			st.net.NetBase.sworn_task_finish_info listData = new st.net.NetBase.sworn_task_finish_info();
			listData.fromBinary(reader);
			task_finish_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenreward_list = (ushort)reward_list.Count;
		writer.write_short(lenreward_list);
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			st.net.NetBase.brother_reward_info listData = reward_list[i_reward_list];
			listData.toBinary(writer);
		}
		ushort lentask_finish_list = (ushort)task_finish_list.Count;
		writer.write_short(lentask_finish_list);
		for(int i_task_finish_list = 0 ; i_task_finish_list < lentask_finish_list ; i_task_finish_list ++)
		{
			st.net.NetBase.sworn_task_finish_info listData = task_finish_list[i_task_finish_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
