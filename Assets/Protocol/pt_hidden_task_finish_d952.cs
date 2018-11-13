using System.Collections;
using System.Collections.Generic;

public class pt_hidden_task_finish_d952 : st.net.NetBase.Pt {
	public pt_hidden_task_finish_d952()
	{
		Id = 0xD952;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_hidden_task_finish_d952();
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
