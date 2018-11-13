using System.Collections;
using System.Collections.Generic;

public class pt_store_buy_d034 : st.net.NetBase.Pt {
	public pt_store_buy_d034()
	{
		Id = 0xD034;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_store_buy_d034();
	}
	public uint store_id;
	public uint cell_id;
	public uint buy_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		store_id = reader.Read_uint();
		cell_id = reader.Read_uint();
		buy_num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(store_id);
		writer.write_int(cell_id);
		writer.write_int(buy_num);
		return writer.data;
	}

}
