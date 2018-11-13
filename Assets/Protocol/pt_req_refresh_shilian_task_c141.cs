using System.Collections;
using System.Collections.Generic;

public class pt_req_refresh_shilian_task_c141 : st.net.NetBase.Pt {
	public pt_req_refresh_shilian_task_c141()
	{
		Id = 0xC141;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_refresh_shilian_task_c141();
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
