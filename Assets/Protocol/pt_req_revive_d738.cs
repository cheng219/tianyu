using System.Collections;
using System.Collections.Generic;

public class pt_req_revive_d738 : st.net.NetBase.Pt {
	public pt_req_revive_d738()
	{
		Id = 0xD738;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_revive_d738();
	}
	public int action;
	public int quick_buy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_int();
		quick_buy = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(action);
		writer.write_int(quick_buy);
		return writer.data;
	}

}
