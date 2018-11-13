using System.Collections;
using System.Collections.Generic;

public class pt_treasure_base_info_d979 : st.net.NetBase.Pt {
	public pt_treasure_base_info_d979()
	{
		Id = 0xD979;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_treasure_base_info_d979();
	}
	public uint rest_time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rest_time = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rest_time);
		return writer.data;
	}

}
