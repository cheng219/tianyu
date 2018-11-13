using System.Collections;
using System.Collections.Generic;

public class pt_copy_exist_time_d223 : st.net.NetBase.Pt {
	public pt_copy_exist_time_d223()
	{
		Id = 0xD223;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_exist_time_d223();
	}
	public int time_len;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		time_len = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(time_len);
		return writer.data;
	}

}
