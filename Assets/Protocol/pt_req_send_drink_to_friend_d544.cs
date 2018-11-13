using System.Collections;
using System.Collections.Generic;

public class pt_req_send_drink_to_friend_d544 : st.net.NetBase.Pt {
	public pt_req_send_drink_to_friend_d544()
	{
		Id = 0xD544;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_send_drink_to_friend_d544();
	}
	public int target_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		target_uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(target_uid);
		return writer.data;
	}

}
