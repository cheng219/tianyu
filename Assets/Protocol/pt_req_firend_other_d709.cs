using System.Collections;
using System.Collections.Generic;

public class pt_req_firend_other_d709 : st.net.NetBase.Pt {
	public pt_req_firend_other_d709()
	{
		Id = 0xD709;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_firend_other_d709();
	}
	public int state;
	public int oth_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		oth_uid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(oth_uid);
		return writer.data;
	}

}
