using System.Collections;
using System.Collections.Generic;

public class pt_action_int_d003 : st.net.NetBase.Pt {
	public pt_action_int_d003()
	{
		Id = 0xD003;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_action_int_d003();
	}
	public int action;
	public int data;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_int();
		data = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(action);
		writer.write_int(data);
		return writer.data;
	}

}
