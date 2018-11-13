using System.Collections;
using System.Collections.Generic;

public class pt_update_little_window_c119 : st.net.NetBase.Pt {
	public pt_update_little_window_c119()
	{
		Id = 0xC119;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_little_window_c119();
	}
	public int id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
