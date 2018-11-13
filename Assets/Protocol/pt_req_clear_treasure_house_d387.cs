using System.Collections;
using System.Collections.Generic;

public class pt_req_clear_treasure_house_d387 : st.net.NetBase.Pt {
	public pt_req_clear_treasure_house_d387()
	{
		Id = 0xD387;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_clear_treasure_house_d387();
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
