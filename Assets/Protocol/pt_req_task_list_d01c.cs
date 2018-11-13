using System.Collections;
using System.Collections.Generic;

public class pt_req_task_list_d01c : st.net.NetBase.Pt {
	public pt_req_task_list_d01c()
	{
		Id = 0xD01C;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_task_list_d01c();
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
