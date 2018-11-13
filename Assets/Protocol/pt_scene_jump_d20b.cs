using System.Collections;
using System.Collections.Generic;

public class pt_scene_jump_d20b : st.net.NetBase.Pt {
	public pt_scene_jump_d20b()
	{
		Id = 0xD20B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_jump_d20b();
	}
	public uint pid;
	public float dir;
	public float x;
	public float y;
	public float z;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		pid = reader.Read_uint();
		dir = reader.Read_float();
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(pid);
		writer.write_float(dir);
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		return writer.data;
	}

}
