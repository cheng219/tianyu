using System.Collections;
using System.Collections.Generic;

public class pt_req_chat_d209 : st.net.NetBase.Pt {
	public pt_req_chat_d209()
	{
		Id = 0xD209;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_chat_d209();
	}
	public string rec_name;
	public uint chanle;
	public string content;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rec_name = reader.Read_str();
		chanle = reader.Read_uint();
		content = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(rec_name);
		writer.write_int(chanle);
		writer.write_str(content);
		return writer.data;
	}

}
