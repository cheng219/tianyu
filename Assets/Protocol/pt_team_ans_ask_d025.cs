using System.Collections;
using System.Collections.Generic;

public class pt_team_ans_ask_d025 : st.net.NetBase.Pt {
	public pt_team_ans_ask_d025()
	{
		Id = 0xD025;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_ans_ask_d025();
	}
	public uint ans_uid;
	public uint ans;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ans_uid = reader.Read_uint();
		ans = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ans_uid);
		writer.write_int(ans);
		return writer.data;
	}

}
