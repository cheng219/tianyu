using System.Collections;
using System.Collections.Generic;

public class pt_lost_item_get_d01a : st.net.NetBase.Pt {
	public pt_lost_item_get_d01a()
	{
		Id = 0xD01A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lost_item_get_d01a();
	}
	public uint itemid;
	public uint lev;
	public uint debris_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		itemid = reader.Read_uint();
		lev = reader.Read_uint();
		debris_num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(itemid);
		writer.write_int(lev);
		writer.write_int(debris_num);
		return writer.data;
	}

}
