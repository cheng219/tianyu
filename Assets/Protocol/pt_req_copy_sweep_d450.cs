using System.Collections;
using System.Collections.Generic;

public class pt_req_copy_sweep_d450 : st.net.NetBase.Pt {
	public pt_req_copy_sweep_d450()
	{
		Id = 0xD450;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_copy_sweep_d450();
	}
	public int state;
	public int scene_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		scene_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(scene_type);
		return writer.data;
	}

}
