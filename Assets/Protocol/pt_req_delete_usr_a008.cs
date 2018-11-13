using System.Collections;
using System.Collections.Generic;

public class pt_req_delete_usr_a008 : st.net.NetBase.Pt {
	public pt_req_delete_usr_a008()
	{
		Id = 0xA008;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_delete_usr_a008();
	}
	public uint uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		return writer.data;
	}

}
