using System.Collections;
using System.Collections.Generic;

public class pt_delete_usr_a109 : st.net.NetBase.Pt {
	public pt_delete_usr_a109()
	{
		Id = 0xA109;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_delete_usr_a109();
	}
	public uint uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		return writer.data;
	}

}
