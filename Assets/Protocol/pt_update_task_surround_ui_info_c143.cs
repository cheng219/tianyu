using System.Collections;
using System.Collections.Generic;

public class pt_update_task_surround_ui_info_c143 : st.net.NetBase.Pt {
	public pt_update_task_surround_ui_info_c143()
	{
		Id = 0xC143;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_task_surround_ui_info_c143();
	}
	public List<st.net.NetBase.task_surround_info> task_surround = new List<st.net.NetBase.task_surround_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentask_surround = reader.Read_ushort();
		task_surround = new List<st.net.NetBase.task_surround_info>();
		for(int i_task_surround = 0 ; i_task_surround < lentask_surround ; i_task_surround ++)
		{
			st.net.NetBase.task_surround_info listData = new st.net.NetBase.task_surround_info();
			listData.fromBinary(reader);
			task_surround.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentask_surround = (ushort)task_surround.Count;
		writer.write_short(lentask_surround);
		for(int i_task_surround = 0 ; i_task_surround < lentask_surround ; i_task_surround ++)
		{
			st.net.NetBase.task_surround_info listData = task_surround[i_task_surround];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
