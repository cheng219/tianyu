using System.Collections;
using System.Collections.Generic;

public class pt_hidden_task_accept_d951 : st.net.NetBase.Pt {
	public pt_hidden_task_accept_d951()
	{
		Id = 0xD951;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_hidden_task_accept_d951();
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
