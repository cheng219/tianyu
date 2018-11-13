using System.Collections;
using System.Collections.Generic;

public class pt_reply_get_weekcard_reward_d923 : st.net.NetBase.Pt {
	public pt_reply_get_weekcard_reward_d923()
	{
		Id = 0xD923;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_get_weekcard_reward_d923();
	}
	public uint type;
	public byte status;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_uint();
		status = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_byte(status);
		return writer.data;
	}

}
