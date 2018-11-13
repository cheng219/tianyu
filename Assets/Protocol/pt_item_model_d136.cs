using System.Collections;
using System.Collections.Generic;

public class pt_item_model_d136 : st.net.NetBase.Pt {
	public pt_item_model_d136()
	{
		Id = 0xD136;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_item_model_d136();
	}
	public int item_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		item_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(item_id);
		return writer.data;
	}

}
