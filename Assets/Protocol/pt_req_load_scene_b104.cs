using System.Collections;
using System.Collections.Generic;

public class pt_req_load_scene_b104 : st.net.NetBase.Pt {
	public pt_req_load_scene_b104()
	{
		Id = 0xB104;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_load_scene_b104();
	}
	public uint scene;
	public float x;
	public float y;
	public float z;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		scene = reader.Read_uint();
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(scene);
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		return writer.data;
	}

}
