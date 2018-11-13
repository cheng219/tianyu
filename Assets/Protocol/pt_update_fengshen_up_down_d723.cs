using System.Collections;
using System.Collections.Generic;

public class pt_update_fengshen_up_down_d723 : st.net.NetBase.Pt {
	public pt_update_fengshen_up_down_d723()
	{
		Id = 0xD723;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_fengshen_up_down_d723();
	}
	public int state;
	public int count_down;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		count_down = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(count_down);
		return writer.data;
	}

}
