using System.Collections;
using System.Collections.Generic;

public class pt_mequip_strengthen_d572 : st.net.NetBase.Pt {
	public pt_mequip_strengthen_d572()
	{
		Id = 0xD572;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mequip_strengthen_d572();
	}
	public uint item_id;
	public byte type;
	public byte quick_buy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		item_id = reader.Read_uint();
		type = reader.Read_byte();
		quick_buy = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(item_id);
		writer.write_byte(type);
		writer.write_byte(quick_buy);
		return writer.data;
	}

}
