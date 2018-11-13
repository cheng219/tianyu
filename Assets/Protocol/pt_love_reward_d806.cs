using System.Collections;
using System.Collections.Generic;

public class pt_love_reward_d806 : st.net.NetBase.Pt {
	public pt_love_reward_d806()
	{
		Id = 0xD806;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_love_reward_d806();
	}
	public int diamo;
	public int stage;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		diamo = reader.Read_int();
		stage = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(diamo);
		writer.write_int(stage);
		return writer.data;
	}

}
