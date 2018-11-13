using System.Collections;
using System.Collections.Generic;

public class pt_reply_operation_activity_reward_d905 : st.net.NetBase.Pt {
	public pt_reply_operation_activity_reward_d905()
	{
		Id = 0xD905;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_operation_activity_reward_d905();
	}
	public uint act_id;
	public byte index;
	public byte result;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		act_id = reader.Read_uint();
		index = reader.Read_byte();
		result = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(act_id);
		writer.write_byte(index);
		writer.write_byte(result);
		return writer.data;
	}

}
