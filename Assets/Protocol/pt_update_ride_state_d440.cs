using System.Collections;
using System.Collections.Generic;

public class pt_update_ride_state_d440 : st.net.NetBase.Pt {
	public pt_update_ride_state_d440()
	{
		Id = 0xD440;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_ride_state_d440();
	}
	public int ride_state;
	public int ride_id;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ride_state = reader.Read_int();
		ride_id = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ride_state);
		writer.write_int(ride_id);
		writer.write_int(state);
		return writer.data;
	}

}
