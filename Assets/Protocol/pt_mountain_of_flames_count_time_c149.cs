using System.Collections;
using System.Collections.Generic;

public class pt_mountain_of_flames_count_time_c149 : st.net.NetBase.Pt {
	public pt_mountain_of_flames_count_time_c149()
	{
		Id = 0xC149;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mountain_of_flames_count_time_c149();
	}
	public int count_time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		count_time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(count_time);
		return writer.data;
	}

}
