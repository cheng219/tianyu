using System.Collections;
using System.Collections.Generic;

public class pt_update_copy_tier_d497 : st.net.NetBase.Pt {
	public pt_update_copy_tier_d497()
	{
		Id = 0xD497;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_copy_tier_d497();
	}
	public int tier;
	public int max_tier;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		tier = reader.Read_int();
		max_tier = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(tier);
		writer.write_int(max_tier);
		return writer.data;
	}

}
