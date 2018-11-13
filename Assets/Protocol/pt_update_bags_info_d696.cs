using System.Collections;
using System.Collections.Generic;

public class pt_update_bags_info_d696 : st.net.NetBase.Pt {
	public pt_update_bags_info_d696()
	{
		Id = 0xD696;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_bags_info_d696();
	}
	public int time;
	public int sycee;
	public int exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		time = reader.Read_int();
		sycee = reader.Read_int();
		exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(time);
		writer.write_int(sycee);
		writer.write_int(exp);
		return writer.data;
	}

}
