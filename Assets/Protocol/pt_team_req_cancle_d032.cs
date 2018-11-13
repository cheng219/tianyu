using System.Collections;
using System.Collections.Generic;

public class pt_team_req_cancle_d032 : st.net.NetBase.Pt {
	public pt_team_req_cancle_d032()
	{
		Id = 0xD032;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_req_cancle_d032();
	}
	public uint req_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_uid);
		return writer.data;
	}

}
