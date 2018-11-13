using System.Collections;
using System.Collections.Generic;

public class pt_action_float_d005 : st.net.NetBase.Pt {
	public pt_action_float_d005()
	{
		Id = 0xD005;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_action_float_d005();
	}
	public int action;
	public float data;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_int();
		data = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(action);
		writer.write_float(data);
		return writer.data;
	}

}
