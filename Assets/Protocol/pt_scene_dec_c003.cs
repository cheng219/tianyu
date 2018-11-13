using System.Collections;
using System.Collections.Generic;

public class pt_scene_dec_c003 : st.net.NetBase.Pt {
	public pt_scene_dec_c003()
	{
		Id = 0xC003;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_dec_c003();
	}
	public uint oid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		return writer.data;
	}

}
