using System.Collections;
using System.Collections.Generic;

public class pt_submit_ready_d219 : st.net.NetBase.Pt {
	public pt_submit_ready_d219()
	{
		Id = 0xD219;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_submit_ready_d219();
	}
	public string name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(name);
		return writer.data;
	}

}
