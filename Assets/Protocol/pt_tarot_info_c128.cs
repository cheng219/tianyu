using System.Collections;
using System.Collections.Generic;

public class pt_tarot_info_c128 : st.net.NetBase.Pt {
	public pt_tarot_info_c128()
	{
		Id = 0xC128;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_tarot_info_c128();
	}
	public uint surplus_num;
	public uint surplus_times;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		surplus_num = reader.Read_uint();
		surplus_times = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(surplus_num);
		writer.write_int(surplus_times);
		return writer.data;
	}

}
