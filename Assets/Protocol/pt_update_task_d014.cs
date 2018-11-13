using System.Collections;
using System.Collections.Generic;

public class pt_update_task_d014 : st.net.NetBase.Pt {
	public pt_update_task_d014()
	{
		Id = 0xD014;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_task_d014();
	}
	public List<st.net.NetBase.task_list_info> tasks = new List<st.net.NetBase.task_list_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentasks = reader.Read_ushort();
		tasks = new List<st.net.NetBase.task_list_info>();
		for(int i_tasks = 0 ; i_tasks < lentasks ; i_tasks ++)
		{
			st.net.NetBase.task_list_info listData = new st.net.NetBase.task_list_info();
			listData.fromBinary(reader);
			tasks.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentasks = (ushort)tasks.Count;
		writer.write_short(lentasks);
		for(int i_tasks = 0 ; i_tasks < lentasks ; i_tasks ++)
		{
			st.net.NetBase.task_list_info listData = tasks[i_tasks];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
