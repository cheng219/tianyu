using System.Collections;
using System.Collections.Generic;

public class pt_update_fly_up_num_d442 : st.net.NetBase.Pt {
	public pt_update_fly_up_num_d442()
	{
		Id = 0xD442;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_fly_up_num_d442();
	}
	public uint num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(num);
		return writer.data;
	}

}
