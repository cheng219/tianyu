using System.Collections;
using System.Collections.Generic;

public class pt_cancel_camera_follow_d70b : st.net.NetBase.Pt {
	public pt_cancel_camera_follow_d70b()
	{
		Id = 0xD70B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_cancel_camera_follow_d70b();
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
