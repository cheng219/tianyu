using System.Collections;
using System.Collections.Generic;

public class pt_quick_buy_d787 : st.net.NetBase.Pt {
	public pt_quick_buy_d787()
	{
		Id = 0xD787;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_quick_buy_d787();
	}
	public int buy_item;
	public int quick_buy_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		buy_item = reader.Read_int();
		quick_buy_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(buy_item);
		writer.write_int(quick_buy_type);
		return writer.data;
	}

}
