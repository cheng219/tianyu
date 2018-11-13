using System.Collections;
using System.Collections.Generic;

public class pt_crowd_num_d224 : st.net.NetBase.Pt {
	public pt_crowd_num_d224()
	{
		Id = 0xD224;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_crowd_num_d224();
	}
	public int num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(num);
		return writer.data;
	}

}
