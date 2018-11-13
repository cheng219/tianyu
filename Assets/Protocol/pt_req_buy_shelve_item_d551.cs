using System.Collections;
using System.Collections.Generic;

public class pt_req_buy_shelve_item_d551 : st.net.NetBase.Pt {
	public pt_req_buy_shelve_item_d551()
	{
		Id = 0xD551;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_buy_shelve_item_d551();
	}
	public uint id;
	public uint num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(num);
		return writer.data;
	}

}
