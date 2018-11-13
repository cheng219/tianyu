using System.Collections;
using System.Collections.Generic;

public class pt_req_pk_challenge_d490 : st.net.NetBase.Pt {
	public pt_req_pk_challenge_d490()
	{
		Id = 0xD490;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_pk_challenge_d490();
	}
	public int challenge_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		challenge_uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(challenge_uid);
		return writer.data;
	}

}
