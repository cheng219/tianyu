using System.Collections;
using System.Collections.Generic;

public class pt_new_function_open_aready_d804 : st.net.NetBase.Pt {
	public pt_new_function_open_aready_d804()
	{
		Id = 0xD804;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_new_function_open_aready_d804();
	}
	public int openlists;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		openlists = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(openlists);
		return writer.data;
	}

}
