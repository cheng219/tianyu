using System.Collections;
using System.Collections.Generic;

public class pt_req_call_boss_d494 : st.net.NetBase.Pt {
	public pt_req_call_boss_d494()
	{
		Id = 0xD494;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_call_boss_d494();
	}
	public int boss_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		boss_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(boss_id);
		return writer.data;
	}

}
