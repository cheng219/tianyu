using System.Collections;
using System.Collections.Generic;

public class pt_lost_item_active_d01e : st.net.NetBase.Pt {
	public pt_lost_item_active_d01e()
	{
		Id = 0xD01E;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lost_item_active_d01e();
	}
	public uint itemid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		itemid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(itemid);
		return writer.data;
	}

}
