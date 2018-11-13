using System.Collections;
using System.Collections.Generic;

public class pt_action_string_d004 : st.net.NetBase.Pt {
	public pt_action_string_d004()
	{
		Id = 0xD004;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_action_string_d004();
	}
	public int action;
	public string data;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_int();
		data = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(action);
		writer.write_str(data);
		return writer.data;
	}

}
