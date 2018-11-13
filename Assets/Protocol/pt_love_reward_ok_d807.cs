using System.Collections;
using System.Collections.Generic;

public class pt_love_reward_ok_d807 : st.net.NetBase.Pt {
	public pt_love_reward_ok_d807()
	{
		Id = 0xD807;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_love_reward_ok_d807();
	}
	public int id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
