using System.Collections;
using System.Collections.Generic;

public class pt_activity_info_d106 : st.net.NetBase.Pt {
	public pt_activity_info_d106()
	{
		Id = 0xD106;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_activity_info_d106();
	}
	public uint activity_val;
	public List<st.net.NetBase.activity_info> activity_info = new List<st.net.NetBase.activity_info>();
	public List<st.net.NetBase.activity_rewards> activity_rewards = new List<st.net.NetBase.activity_rewards>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		activity_val = reader.Read_uint();
		ushort lenactivity_info = reader.Read_ushort();
		activity_info = new List<st.net.NetBase.activity_info>();
		for(int i_activity_info = 0 ; i_activity_info < lenactivity_info ; i_activity_info ++)
		{
			st.net.NetBase.activity_info listData = new st.net.NetBase.activity_info();
			listData.fromBinary(reader);
			activity_info.Add(listData);
		}
		ushort lenactivity_rewards = reader.Read_ushort();
		activity_rewards = new List<st.net.NetBase.activity_rewards>();
		for(int i_activity_rewards = 0 ; i_activity_rewards < lenactivity_rewards ; i_activity_rewards ++)
		{
			st.net.NetBase.activity_rewards listData = new st.net.NetBase.activity_rewards();
			listData.fromBinary(reader);
			activity_rewards.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(activity_val);
		ushort lenactivity_info = (ushort)activity_info.Count;
		writer.write_short(lenactivity_info);
		for(int i_activity_info = 0 ; i_activity_info < lenactivity_info ; i_activity_info ++)
		{
			st.net.NetBase.activity_info listData = activity_info[i_activity_info];
			listData.toBinary(writer);
		}
		ushort lenactivity_rewards = (ushort)activity_rewards.Count;
		writer.write_short(lenactivity_rewards);
		for(int i_activity_rewards = 0 ; i_activity_rewards < lenactivity_rewards ; i_activity_rewards ++)
		{
			st.net.NetBase.activity_rewards listData = activity_rewards[i_activity_rewards];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
