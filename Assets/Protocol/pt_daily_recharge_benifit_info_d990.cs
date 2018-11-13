using System.Collections;
using System.Collections.Generic;

public class pt_daily_recharge_benifit_info_d990 : st.net.NetBase.Pt {
	public pt_daily_recharge_benifit_info_d990()
	{
		Id = 0xD990;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_daily_recharge_benifit_info_d990();
	}
	public uint rest_time;
	public uint percent;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rest_time = reader.Read_uint();
		percent = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rest_time);
		writer.write_int(percent);
		return writer.data;
	}

}
