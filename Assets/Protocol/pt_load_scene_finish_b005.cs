using System.Collections;
using System.Collections.Generic;

public class pt_load_scene_finish_b005 : st.net.NetBase.Pt {
	public pt_load_scene_finish_b005()
	{
		Id = 0xB005;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_load_scene_finish_b005();
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
