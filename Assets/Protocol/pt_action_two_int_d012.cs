using System.Collections;
using System.Collections.Generic;

public class pt_action_two_int_d012 : st.net.NetBase.Pt {
	public pt_action_two_int_d012()
	{
		Id = 0xD012;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_action_two_int_d012();
	}
	public int action;
	public int data_One;
	public int data_Two;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_int();
		data_One = reader.Read_int();
		data_Two = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(action);
		writer.write_int(data_One);
		writer.write_int(data_Two);
		return writer.data;
	}

}
