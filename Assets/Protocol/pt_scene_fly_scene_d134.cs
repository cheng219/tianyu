using System.Collections;
using System.Collections.Generic;

public class pt_scene_fly_scene_d134 : st.net.NetBase.Pt {
	public pt_scene_fly_scene_d134()
	{
		Id = 0xD134;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_fly_scene_d134();
	}
	public int fly_scene_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		fly_scene_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(fly_scene_id);
		return writer.data;
	}

}
