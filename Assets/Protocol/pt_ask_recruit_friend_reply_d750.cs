using System.Collections;
using System.Collections.Generic;

public class pt_ask_recruit_friend_reply_d750 : st.net.NetBase.Pt {
	public pt_ask_recruit_friend_reply_d750()
	{
		Id = 0xD750;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ask_recruit_friend_reply_d750();
	}
	public int ans_uid;
	public int ans_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ans_uid = reader.Read_int();
		ans_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ans_uid);
		writer.write_int(ans_state);
		return writer.data;
	}

}
