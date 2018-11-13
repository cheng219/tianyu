using System.Collections;
using System.Collections.Generic;

public class pt_req_mountain_flames_add_buff_c116 : st.net.NetBase.Pt {
	public pt_req_mountain_flames_add_buff_c116()
	{
		Id = 0xC116;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_mountain_flames_add_buff_c116();
	}
	public int id;
	public int in_and_out;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		in_and_out = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(in_and_out);
		return writer.data;
	}

}
