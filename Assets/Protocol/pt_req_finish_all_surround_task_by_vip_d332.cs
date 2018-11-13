using System.Collections;
using System.Collections.Generic;

public class pt_req_finish_all_surround_task_by_vip_d332 : st.net.NetBase.Pt {
	public pt_req_finish_all_surround_task_by_vip_d332()
	{
		Id = 0xD332;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_finish_all_surround_task_by_vip_d332();
	}
	public uint surround_task_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		surround_task_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(surround_task_id);
		return writer.data;
	}

}
