using System.Collections;
using System.Collections.Generic;

public class pt_update_friend_intimacy_d759 : st.net.NetBase.Pt {
	public pt_update_friend_intimacy_d759()
	{
		Id = 0xD759;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_friend_intimacy_d759();
	}
	public int oth_uid;
	public int intimacy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oth_uid = reader.Read_int();
		intimacy = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oth_uid);
		writer.write_int(intimacy);
		return writer.data;
	}

}
