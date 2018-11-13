using System.Collections;
using System.Collections.Generic;

public class pt_scene_delete_arrow_c016 : st.net.NetBase.Pt {
	public pt_scene_delete_arrow_c016()
	{
		Id = 0xC016;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_delete_arrow_c016();
	}
	public uint aid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		aid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(aid);
		return writer.data;
	}

}
