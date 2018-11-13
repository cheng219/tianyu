using System.Collections;
using System.Collections.Generic;

public class pt_add_bags_d131 : st.net.NetBase.Pt {
	public pt_add_bags_d131()
	{
		Id = 0xD131;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_add_bags_d131();
	}
	public uint add_bags;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		add_bags = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(add_bags);
		return writer.data;
	}

}
