using System.Collections;
using System.Collections.Generic;

public class pt_req_finish_all_shilian_task_by_vip_d333 : st.net.NetBase.Pt {
	public pt_req_finish_all_shilian_task_by_vip_d333()
	{
		Id = 0xD333;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_finish_all_shilian_task_by_vip_d333();
	}
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		return writer.data;
	}

}
