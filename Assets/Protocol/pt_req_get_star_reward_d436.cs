using System.Collections;
using System.Collections.Generic;

public class pt_req_get_star_reward_d436 : st.net.NetBase.Pt {
	public pt_req_get_star_reward_d436()
	{
		Id = 0xD436;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_star_reward_d436();
	}
	public int chapter_id;
	public int star_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		chapter_id = reader.Read_int();
		star_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(chapter_id);
		writer.write_int(star_id);
		return writer.data;
	}

}
