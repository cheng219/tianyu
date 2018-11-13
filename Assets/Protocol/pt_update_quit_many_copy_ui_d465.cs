using System.Collections;
using System.Collections.Generic;

public class pt_update_quit_many_copy_ui_d465 : st.net.NetBase.Pt {
	public pt_update_quit_many_copy_ui_d465()
	{
		Id = 0xD465;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_quit_many_copy_ui_d465();
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
