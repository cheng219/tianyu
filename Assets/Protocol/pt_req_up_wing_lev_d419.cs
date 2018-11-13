using System.Collections;
using System.Collections.Generic;

public class pt_req_up_wing_lev_d419 : st.net.NetBase.Pt {
	public pt_req_up_wing_lev_d419()
	{
		Id = 0xD419;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_up_wing_lev_d419();
	}
	public int state;
	public int wing_id;
	public int quick_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		wing_id = reader.Read_int();
		quick_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(wing_id);
		writer.write_int(quick_state);
		return writer.data;
	}

}
