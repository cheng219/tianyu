using System.Collections;
using System.Collections.Generic;

public class pt_update_red_dot_d776 : st.net.NetBase.Pt {
	public pt_update_red_dot_d776()
	{
		Id = 0xD776;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_red_dot_d776();
	}
	public int type;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(state);
		return writer.data;
	}

}
