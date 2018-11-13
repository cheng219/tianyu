using System.Collections;
using System.Collections.Generic;

public class pt_login_data_a002 : st.net.NetBase.Pt {
	public pt_login_data_a002()
	{
		Id = 0xA002;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_login_data_a002();
	}
	public string data;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		data = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(data);
		return writer.data;
	}

}
