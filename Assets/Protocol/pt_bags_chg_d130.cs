using System.Collections;
using System.Collections.Generic;

public class pt_bags_chg_d130 : st.net.NetBase.Pt {
	public pt_bags_chg_d130()
	{
		Id = 0xD130;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_bags_chg_d130();
	}
	public uint can_use_bags;
	public uint unlock_bags_num;
	public uint rest_time;
	public uint static_rest_time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		can_use_bags = reader.Read_uint();
		unlock_bags_num = reader.Read_uint();
		rest_time = reader.Read_uint();
		static_rest_time = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(can_use_bags);
		writer.write_int(unlock_bags_num);
		writer.write_int(rest_time);
		writer.write_int(static_rest_time);
		return writer.data;
	}

}
