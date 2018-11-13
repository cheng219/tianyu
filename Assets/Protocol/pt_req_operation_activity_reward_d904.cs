using System.Collections;
using System.Collections.Generic;

public class pt_req_operation_activity_reward_d904 : st.net.NetBase.Pt {
	public pt_req_operation_activity_reward_d904()
	{
		Id = 0xD904;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_operation_activity_reward_d904();
	}
	public uint act_id;
	public byte index;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		act_id = reader.Read_uint();
		index = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(act_id);
		writer.write_byte(index);
		return writer.data;
	}

}
