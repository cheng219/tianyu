using System.Collections;
using System.Collections.Generic;

public class pt_req_friend_relation_d704 : st.net.NetBase.Pt {
	public pt_req_friend_relation_d704()
	{
		Id = 0xD704;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_friend_relation_d704();
	}
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		return writer.data;
	}

}
