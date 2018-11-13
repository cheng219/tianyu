using System.Collections;
using System.Collections.Generic;

public class pt_scene_item_c019 : st.net.NetBase.Pt {
	public pt_scene_item_c019()
	{
		Id = 0xC019;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_item_c019();
	}
	public uint target_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		target_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(target_id);
		return writer.data;
	}

}
