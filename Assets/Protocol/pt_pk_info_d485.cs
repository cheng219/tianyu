using System.Collections;
using System.Collections.Generic;

public class pt_pk_info_d485 : st.net.NetBase.Pt {
	public pt_pk_info_d485()
	{
		Id = 0xD485;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pk_info_d485();
	}
	public int rank;
	public int surplus_time;
	public int challenge_num;
	public int reward_countdown;
	public int state;
	public List<st.net.NetBase.robot_list> robot_list = new List<st.net.NetBase.robot_list>();
	public List<st.net.NetBase.log_list> log_list = new List<st.net.NetBase.log_list>();
	public int buy_challenge_times;
	public int reward_rank;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rank = reader.Read_int();
		surplus_time = reader.Read_int();
		challenge_num = reader.Read_int();
		reward_countdown = reader.Read_int();
		state = reader.Read_int();
		ushort lenrobot_list = reader.Read_ushort();
		robot_list = new List<st.net.NetBase.robot_list>();
		for(int i_robot_list = 0 ; i_robot_list < lenrobot_list ; i_robot_list ++)
		{
			st.net.NetBase.robot_list listData = new st.net.NetBase.robot_list();
			listData.fromBinary(reader);
			robot_list.Add(listData);
		}
		ushort lenlog_list = reader.Read_ushort();
		log_list = new List<st.net.NetBase.log_list>();
		for(int i_log_list = 0 ; i_log_list < lenlog_list ; i_log_list ++)
		{
			st.net.NetBase.log_list listData = new st.net.NetBase.log_list();
			listData.fromBinary(reader);
			log_list.Add(listData);
		}
		buy_challenge_times = reader.Read_int();
		reward_rank = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rank);
		writer.write_int(surplus_time);
		writer.write_int(challenge_num);
		writer.write_int(reward_countdown);
		writer.write_int(state);
		ushort lenrobot_list = (ushort)robot_list.Count;
		writer.write_short(lenrobot_list);
		for(int i_robot_list = 0 ; i_robot_list < lenrobot_list ; i_robot_list ++)
		{
			st.net.NetBase.robot_list listData = robot_list[i_robot_list];
			listData.toBinary(writer);
		}
		ushort lenlog_list = (ushort)log_list.Count;
		writer.write_short(lenlog_list);
		for(int i_log_list = 0 ; i_log_list < lenlog_list ; i_log_list ++)
		{
			st.net.NetBase.log_list listData = log_list[i_log_list];
			listData.toBinary(writer);
		}
		writer.write_int(buy_challenge_times);
		writer.write_int(reward_rank);
		return writer.data;
	}

}
