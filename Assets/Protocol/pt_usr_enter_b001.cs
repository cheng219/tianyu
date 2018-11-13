using System.Collections;
using System.Collections.Generic;

public class pt_usr_enter_b001 : st.net.NetBase.Pt {
	public pt_usr_enter_b001()
	{
		Id = 0xB001;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_enter_b001();
	}
	public uint uid;
	public string key;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		key = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_str(key);
		return writer.data;
	}

}
