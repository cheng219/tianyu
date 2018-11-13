using System.Collections;
using System.Collections.Generic;

public class pt_req_team_d427 : st.net.NetBase.Pt {
	public pt_req_team_d427()
	{
		Id = 0xD427;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_team_d427();
	}
	public int state;
	public int uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(uid);
		return writer.data;
	}

}
