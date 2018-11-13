using System.Collections;
using System.Collections.Generic;

public class pt_req_fly_on_hook_copy_c140 : st.net.NetBase.Pt {
	public pt_req_fly_on_hook_copy_c140()
	{
		Id = 0xC140;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_fly_on_hook_copy_c140();
	}
	public uint fly_scene;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		fly_scene = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(fly_scene);
		return writer.data;
	}

}
