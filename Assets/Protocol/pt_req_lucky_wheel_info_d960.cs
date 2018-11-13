using System.Collections;
using System.Collections.Generic;

public class pt_req_lucky_wheel_info_d960 : st.net.NetBase.Pt {
	public pt_req_lucky_wheel_info_d960()
	{
		Id = 0xD960;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_lucky_wheel_info_d960();
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
