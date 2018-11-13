using System.Collections;
using System.Collections.Generic;

public class pt_team_destroy_d030 : st.net.NetBase.Pt {
	public pt_team_destroy_d030()
	{
		Id = 0xD030;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_destroy_d030();
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
