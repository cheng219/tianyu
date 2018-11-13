using System.Collections;
using System.Collections.Generic;

public class pt_chat_d208 : st.net.NetBase.Pt {
	public pt_chat_d208()
	{
		Id = 0xD208;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_chat_d208();
	}
	public uint pid;
	public string name;
	public string rec_name;
	public uint vip_lev;
	public uint chanle;
	public string content;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		pid = reader.Read_uint();
		name = reader.Read_str();
		rec_name = reader.Read_str();
		vip_lev = reader.Read_uint();
		chanle = reader.Read_uint();
		content = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(pid);
		writer.write_str(name);
		writer.write_str(rec_name);
		writer.write_int(vip_lev);
		writer.write_int(chanle);
		writer.write_str(content);
		return writer.data;
	}

}
