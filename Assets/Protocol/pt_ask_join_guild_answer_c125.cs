using System.Collections;
using System.Collections.Generic;

public class pt_ask_join_guild_answer_c125 : st.net.NetBase.Pt {
	public pt_ask_join_guild_answer_c125()
	{
		Id = 0xC125;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ask_join_guild_answer_c125();
	}
	public int answer;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		answer = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(answer);
		return writer.data;
	}

}
