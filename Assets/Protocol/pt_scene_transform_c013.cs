using System.Collections;
using System.Collections.Generic;

public class pt_scene_transform_c013 : st.net.NetBase.Pt {
	public pt_scene_transform_c013()
	{
		Id = 0xC013;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_transform_c013();
	}
	public uint oid;
	public uint obj_sort;
	public uint type;
	public float time;
	public float x;
	public float y;
	public float z;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		obj_sort = reader.Read_uint();
		type = reader.Read_uint();
		time = reader.Read_float();
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_int(obj_sort);
		writer.write_int(type);
		writer.write_float(time);
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		return writer.data;
	}

}
