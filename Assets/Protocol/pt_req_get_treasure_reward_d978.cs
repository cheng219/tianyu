using System.Collections;
using System.Collections.Generic;

public class pt_req_get_treasure_reward_d978 : st.net.NetBase.Pt {
	public pt_req_get_treasure_reward_d978()
	{
		Id = 0xD978;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_treasure_reward_d978();
	}
	public uint id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
