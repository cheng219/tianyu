using System.Collections;
using System.Collections.Generic;

public class pt_recruit_robot_info_d744 : st.net.NetBase.Pt {
	public pt_recruit_robot_info_d744()
	{
		Id = 0xD744;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_recruit_robot_info_d744();
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
