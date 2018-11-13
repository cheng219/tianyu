using System.Collections;
using System.Collections.Generic;

public class pt_action_d002 : st.net.NetBase.Pt {
	public pt_action_d002()
	{
		Id = 0xD002;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_action_d002();
	}
	public int action;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(action);
		return writer.data;
	}

}
