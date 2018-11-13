using System.Collections;
using System.Collections.Generic;

public class pt_req_recruit_friend_d747 : st.net.NetBase.Pt {
	public pt_req_recruit_friend_d747()
	{
		Id = 0xD747;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_recruit_friend_d747();
	}
	public int oth_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oth_uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oth_uid);
		return writer.data;
	}

}
