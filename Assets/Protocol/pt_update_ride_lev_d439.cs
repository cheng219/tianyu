using System.Collections;
using System.Collections.Generic;

public class pt_update_ride_lev_d439 : st.net.NetBase.Pt {
	public pt_update_ride_lev_d439()
	{
		Id = 0xD439;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_ride_lev_d439();
	}
	public uint ride_id;
	public uint state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ride_id = reader.Read_uint();
		state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ride_id);
		writer.write_int(state);
		return writer.data;
	}

}
