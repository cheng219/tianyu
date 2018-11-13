using System.Collections;
using System.Collections.Generic;

public class pt_update_ding_rescue_time_d498 : st.net.NetBase.Pt {
	public pt_update_ding_rescue_time_d498()
	{
		Id = 0xD498;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_ding_rescue_time_d498();
	}
	public int rescue_time;
	public string dd_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rescue_time = reader.Read_int();
		dd_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rescue_time);
		writer.write_str(dd_name);
		return writer.data;
	}

}
