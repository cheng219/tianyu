using System.Collections;
using System.Collections.Generic;

public class pt_req_strengthen_keepsake_d535 : st.net.NetBase.Pt {
	public pt_req_strengthen_keepsake_d535()
	{
		Id = 0xD535;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_strengthen_keepsake_d535();
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
