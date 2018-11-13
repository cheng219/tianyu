using System.Collections;
using System.Collections.Generic;

public class pt_req_finish_shilian_task_by_diamo_d334 : st.net.NetBase.Pt {
	public pt_req_finish_shilian_task_by_diamo_d334()
	{
		Id = 0xD334;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_finish_shilian_task_by_diamo_d334();
	}
	public uint taskid;
	public uint taskstep;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		taskid = reader.Read_uint();
		taskstep = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(taskid);
		writer.write_int(taskstep);
		return writer.data;
	}

}
