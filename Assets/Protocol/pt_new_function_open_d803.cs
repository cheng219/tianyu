using System.Collections;
using System.Collections.Generic;

public class pt_new_function_open_d803 : st.net.NetBase.Pt {
	public pt_new_function_open_d803()
	{
		Id = 0xD803;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_new_function_open_d803();
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
