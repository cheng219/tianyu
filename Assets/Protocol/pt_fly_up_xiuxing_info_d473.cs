using System.Collections;
using System.Collections.Generic;

public class pt_fly_up_xiuxing_info_d473 : st.net.NetBase.Pt {
	public pt_fly_up_xiuxing_info_d473()
	{
		Id = 0xD473;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_fly_up_xiuxing_info_d473();
	}
	public uint num1;
	public uint num2;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		num1 = reader.Read_uint();
		num2 = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(num1);
		writer.write_int(num2);
		return writer.data;
	}

}
