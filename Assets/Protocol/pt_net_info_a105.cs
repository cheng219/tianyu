using System.Collections;
using System.Collections.Generic;

public class pt_net_info_a105 : st.net.NetBase.Pt {
	public pt_net_info_a105()
	{
		Id = 0xA105;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_net_info_a105();
	}
	public string ip;
	public uint port;
	public uint uid;
	public string key;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ip = reader.Read_str();
		port = reader.Read_uint();
		uid = reader.Read_uint();
		key = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(ip);
		writer.write_int(port);
		writer.write_int(uid);
		writer.write_str(key);
		return writer.data;
	}

}
