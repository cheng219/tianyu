using System.Collections;
using System.Collections.Generic;

public class pt_update_wing_state_d443 : st.net.NetBase.Pt {
	public pt_update_wing_state_d443()
	{
		Id = 0xD443;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_wing_state_d443();
	}
	public int wing_id;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		wing_id = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(wing_id);
		writer.write_int(state);
		return writer.data;
	}

}
