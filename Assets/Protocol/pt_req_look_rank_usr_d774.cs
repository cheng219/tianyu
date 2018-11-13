using System.Collections;
using System.Collections.Generic;

public class pt_req_look_rank_usr_d774 : st.net.NetBase.Pt {
	public pt_req_look_rank_usr_d774()
	{
		Id = 0xD774;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_look_rank_usr_d774();
	}
	public int type;
	public int uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(uid);
		return writer.data;
	}

}
