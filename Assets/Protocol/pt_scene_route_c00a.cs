using System.Collections;
using System.Collections.Generic;

public class pt_scene_route_c00a : st.net.NetBase.Pt {
	public pt_scene_route_c00a()
	{
		Id = 0xC00A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_route_c00a();
	}
	public float x;
	public float y;
	public float z;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		return writer.data;
	}

}
