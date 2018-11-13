using System.Collections;
using System.Collections.Generic;

public class pt_update_fengshen_score_d735 : st.net.NetBase.Pt {
	public pt_update_fengshen_score_d735()
	{
		Id = 0xD735;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_fengshen_score_d735();
	}
	public int cur_score;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		cur_score = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(cur_score);
		return writer.data;
	}

}
