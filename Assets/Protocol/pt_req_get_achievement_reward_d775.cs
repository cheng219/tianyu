using System.Collections;
using System.Collections.Generic;

public class pt_req_get_achievement_reward_d775 : st.net.NetBase.Pt {
	public pt_req_get_achievement_reward_d775()
	{
		Id = 0xD775;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_achievement_reward_d775();
	}
	public int achievement_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		achievement_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(achievement_id);
		return writer.data;
	}

}
