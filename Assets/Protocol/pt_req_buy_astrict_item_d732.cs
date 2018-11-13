using System.Collections;
using System.Collections.Generic;

public class pt_req_buy_astrict_item_d732 : st.net.NetBase.Pt {
	public pt_req_buy_astrict_item_d732()
	{
		Id = 0xD732;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_buy_astrict_item_d732();
	}
	public int item_id;
	public int item_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		item_id = reader.Read_int();
		item_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(item_id);
		writer.write_int(item_num);
		return writer.data;
	}

}
