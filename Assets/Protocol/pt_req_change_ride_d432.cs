using System.Collections;
using System.Collections.Generic;

public class pt_req_change_ride_d432 : st.net.NetBase.Pt {
	public pt_req_change_ride_d432()
	{
		Id = 0xD432;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_change_ride_d432();
	}
	public int state;
	public int id;
	public int req_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		id = reader.Read_int();
		req_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(id);
		writer.write_int(req_state);
		return writer.data;
	}

}
