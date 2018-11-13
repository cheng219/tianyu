using System.Collections;
using System.Collections.Generic;

public class pt_copy_pick_item_time_d753 : st.net.NetBase.Pt {
	public pt_copy_pick_item_time_d753()
	{
		Id = 0xD753;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_pick_item_time_d753();
	}
	public int time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(time);
		return writer.data;
	}

}
