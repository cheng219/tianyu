using System.Collections;
using System.Collections.Generic;

public class pt_surround_task_fly_state_c120 : st.net.NetBase.Pt {
	public pt_surround_task_fly_state_c120()
	{
		Id = 0xC120;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_surround_task_fly_state_c120();
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
