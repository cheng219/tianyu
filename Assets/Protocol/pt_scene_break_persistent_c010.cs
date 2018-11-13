using System.Collections;
using System.Collections.Generic;

public class pt_scene_break_persistent_c010 : st.net.NetBase.Pt {
	public pt_scene_break_persistent_c010()
	{
		Id = 0xC010;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_break_persistent_c010();
	}
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		return writer.data;
	}

}
