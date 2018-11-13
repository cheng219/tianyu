using System.Collections;
using System.Collections.Generic;

public class pt_scene_skill_c005 : st.net.NetBase.Pt {
	public pt_scene_skill_c005()
	{
		Id = 0xC005;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_skill_c005();
	}
	public uint skill;
	public uint lev;
	public uint oid;
	public float x;
	public float y;
	public float z;
	public float dir;
	public uint target_id;
	public float target_x;
	public float target_y;
	public float target_z;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		skill = reader.Read_uint();
		lev = reader.Read_uint();
		oid = reader.Read_uint();
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
		dir = reader.Read_float();
		target_id = reader.Read_uint();
		target_x = reader.Read_float();
		target_y = reader.Read_float();
		target_z = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(skill);
		writer.write_int(lev);
		writer.write_int(oid);
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		writer.write_float(dir);
		writer.write_int(target_id);
		writer.write_float(target_x);
		writer.write_float(target_y);
		writer.write_float(target_z);
		return writer.data;
	}

}
