using System.Collections;
using System.Collections.Generic;

public class pt_stra_reward_list_d437 : st.net.NetBase.Pt {
	public pt_stra_reward_list_d437()
	{
		Id = 0xD437;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_stra_reward_list_d437();
	}
	public int chapter_id;
	public int star_id;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		chapter_id = reader.Read_int();
		star_id = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(chapter_id);
		writer.write_int(star_id);
		writer.write_int(state);
		return writer.data;
	}

}
