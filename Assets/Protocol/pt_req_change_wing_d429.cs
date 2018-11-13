using System.Collections;
using System.Collections.Generic;

public class pt_req_change_wing_d429 : st.net.NetBase.Pt {
	public pt_req_change_wing_d429()
	{
		Id = 0xD429;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_change_wing_d429();
	}
	public uint wing_id;
	public uint state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		wing_id = reader.Read_uint();
		state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(wing_id);
		writer.write_int(state);
		return writer.data;
	}

}
