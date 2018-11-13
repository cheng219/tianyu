using System.Collections;
using System.Collections.Generic;

public class pt_usr_enter_scene_b003 : st.net.NetBase.Pt {
	public pt_usr_enter_scene_b003()
	{
		Id = 0xB003;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_enter_scene_b003();
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
