using System.Collections;
using System.Collections.Generic;

public class pt_req_quit_many_copy_ui_d464 : st.net.NetBase.Pt {
	public pt_req_quit_many_copy_ui_d464()
	{
		Id = 0xD464;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_quit_many_copy_ui_d464();
	}
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		return writer.data;
	}

}
