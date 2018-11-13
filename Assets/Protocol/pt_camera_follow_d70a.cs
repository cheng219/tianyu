using System.Collections;
using System.Collections.Generic;

public class pt_camera_follow_d70a : st.net.NetBase.Pt {
	public pt_camera_follow_d70a()
	{
		Id = 0xD70A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_camera_follow_d70a();
	}
	public uint id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
