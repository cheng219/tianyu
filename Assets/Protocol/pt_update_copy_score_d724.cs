using System.Collections;
using System.Collections.Generic;

public class pt_update_copy_score_d724 : st.net.NetBase.Pt {
	public pt_update_copy_score_d724()
	{
		Id = 0xD724;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_copy_score_d724();
	}
	public int cur_score;
	public int max_score;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		cur_score = reader.Read_int();
		max_score = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(cur_score);
		writer.write_int(max_score);
		return writer.data;
	}

}
