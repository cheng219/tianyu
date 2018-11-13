using System.Collections;
using System.Collections.Generic;

public class pt_req_look_usr_info_d711 : st.net.NetBase.Pt {
	public pt_req_look_usr_info_d711()
	{
		Id = 0xD711;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_look_usr_info_d711();
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
