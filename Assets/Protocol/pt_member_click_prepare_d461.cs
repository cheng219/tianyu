using System.Collections;
using System.Collections.Generic;

public class pt_member_click_prepare_d461 : st.net.NetBase.Pt {
	public pt_member_click_prepare_d461()
	{
		Id = 0xD461;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_member_click_prepare_d461();
	}
	public uint state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		return writer.data;
	}

}
