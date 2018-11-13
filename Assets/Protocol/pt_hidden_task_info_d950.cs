using System.Collections;
using System.Collections.Generic;

public class pt_hidden_task_info_d950 : st.net.NetBase.Pt {
	public pt_hidden_task_info_d950()
	{
		Id = 0xD950;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_hidden_task_info_d950();
	}
	public byte status;
	public uint rest_time;
	public uint uid;
	public string name;
	public byte prof;
	public uint score_target;
	public uint score_self;
	public List<st.net.NetBase.item_list> reward = new List<st.net.NetBase.item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		status = reader.Read_byte();
		rest_time = reader.Read_uint();
		uid = reader.Read_uint();
		name = reader.Read_str();
		prof = reader.Read_byte();
		score_target = reader.Read_uint();
		score_self = reader.Read_uint();
		ushort lenreward = reader.Read_ushort();
		reward = new List<st.net.NetBase.item_list>();
		for(int i_reward = 0 ; i_reward < lenreward ; i_reward ++)
		{
			st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
			listData.fromBinary(reader);
			reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(status);
		writer.write_int(rest_time);
		writer.write_int(uid);
		writer.write_str(name);
		writer.write_byte(prof);
		writer.write_int(score_target);
		writer.write_int(score_self);
		ushort lenreward = (ushort)reward.Count;
		writer.write_short(lenreward);
		for(int i_reward = 0 ; i_reward < lenreward ; i_reward ++)
		{
			st.net.NetBase.item_list listData = reward[i_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
