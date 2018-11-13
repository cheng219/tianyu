using System.Collections;
using System.Collections.Generic;

public class pt_update_back_city_time_c113 : st.net.NetBase.Pt {
	public pt_update_back_city_time_c113()
	{
		Id = 0xC113;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_back_city_time_c113();
	}
	public int surplus;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		surplus = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(surplus);
		return writer.data;
	}

}
