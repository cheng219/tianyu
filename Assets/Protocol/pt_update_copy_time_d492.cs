using System.Collections;
using System.Collections.Generic;

public class pt_update_copy_time_d492 : st.net.NetBase.Pt {
	public pt_update_copy_time_d492()
	{
		Id = 0xD492;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_copy_time_d492();
	}
	public int copy_time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_time);
		return writer.data;
	}

}
