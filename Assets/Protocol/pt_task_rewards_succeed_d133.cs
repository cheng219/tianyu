using System.Collections;
using System.Collections.Generic;

public class pt_task_rewards_succeed_d133 : st.net.NetBase.Pt {
	public pt_task_rewards_succeed_d133()
	{
		Id = 0xD133;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_task_rewards_succeed_d133();
	}
	public List<st.net.NetBase.get_success_list> get_success_list = new List<st.net.NetBase.get_success_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenget_success_list = reader.Read_ushort();
		get_success_list = new List<st.net.NetBase.get_success_list>();
		for(int i_get_success_list = 0 ; i_get_success_list < lenget_success_list ; i_get_success_list ++)
		{
			st.net.NetBase.get_success_list listData = new st.net.NetBase.get_success_list();
			listData.fromBinary(reader);
			get_success_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenget_success_list = (ushort)get_success_list.Count;
		writer.write_short(lenget_success_list);
		for(int i_get_success_list = 0 ; i_get_success_list < lenget_success_list ; i_get_success_list ++)
		{
			st.net.NetBase.get_success_list listData = get_success_list[i_get_success_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
