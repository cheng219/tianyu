using System.Collections;
using System.Collections.Generic;

public class pt_reply_holy_crystal_info_d611 : st.net.NetBase.Pt {
	public pt_reply_holy_crystal_info_d611()
	{
		Id = 0xD611;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_holy_crystal_info_d611();
	}
	public uint open_box_times;
	public uint total_exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		open_box_times = reader.Read_uint();
		total_exp = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(open_box_times);
		writer.write_int(total_exp);
		return writer.data;
	}

}
