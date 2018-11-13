using System.Collections;
using System.Collections.Generic;

public class pt_scene_fly_by_fly_point_c004 : st.net.NetBase.Pt {
	public pt_scene_fly_by_fly_point_c004()
	{
		Id = 0xC004;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_fly_by_fly_point_c004();
	}
	public int fly_point_id;
	public int fly_scene_type;
	public int fly_x;
	public int fly_y;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		fly_point_id = reader.Read_int();
		fly_scene_type = reader.Read_int();
		fly_x = reader.Read_int();
		fly_y = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(fly_point_id);
		writer.write_int(fly_scene_type);
		writer.write_int(fly_x);
		writer.write_int(fly_y);
		return writer.data;
	}

}
