using System.Collections;
using System.Collections.Generic;

public class pt_req_challenge_seal_boss_d702 : st.net.NetBase.Pt {
	public pt_req_challenge_seal_boss_d702()
	{
		Id = 0xD702;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_challenge_seal_boss_d702();
	}
	public int type;
	public int boss_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		boss_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(boss_id);
		return writer.data;
	}

}
