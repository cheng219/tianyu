using System.Collections;
using System.Collections.Generic;

public class pt_req_fly_single_many_copy_d456 : st.net.NetBase.Pt {
	public pt_req_fly_single_many_copy_d456()
	{
		Id = 0xD456;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_fly_single_many_copy_d456();
	}
	public int copy_id;
	public int scene_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_int();
		scene_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(scene_type);
		return writer.data;
	}

}
