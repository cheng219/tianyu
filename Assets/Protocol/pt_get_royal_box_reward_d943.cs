using System.Collections;
using System.Collections.Generic;

public class pt_get_royal_box_reward_d943 : st.net.NetBase.Pt {
	public pt_get_royal_box_reward_d943()
	{
		Id = 0xD943;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_get_royal_box_reward_d943();
	}
	public uint id;
	public byte type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		type = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_byte(type);
		return writer.data;
	}

}
