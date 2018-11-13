using System.Collections;
using System.Collections.Generic;

public class pt_quell_demon_info_c147 : st.net.NetBase.Pt {
	public pt_quell_demon_info_c147()
	{
		Id = 0xC147;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_quell_demon_info_c147();
	}
	public int challenge_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		challenge_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(challenge_state);
		return writer.data;
	}

}
