using System.Collections;
using System.Collections.Generic;

public class pt_scene_info_b106 : st.net.NetBase.Pt {
	public pt_scene_info_b106()
	{
		Id = 0xB106;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_info_b106();
	}
	public uint scene;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		scene = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(scene);
		return writer.data;
	}

}
