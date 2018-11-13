using System.Collections;
using System.Collections.Generic;

public class pt_req_buy_open_server_gift_d912 : st.net.NetBase.Pt {
	public pt_req_buy_open_server_gift_d912()
	{
		Id = 0xD912;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_buy_open_server_gift_d912();
	}
	public uint ware_id;
	public uint amount;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ware_id = reader.Read_uint();
		amount = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ware_id);
		writer.write_int(amount);
		return writer.data;
	}

}
