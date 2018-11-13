using System.Collections;
using System.Collections.Generic;

public class pt_update_mountain_flames_score_c110 : st.net.NetBase.Pt {
	public pt_update_mountain_flames_score_c110()
	{
		Id = 0xC110;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_mountain_flames_score_c110();
	}
	public List<st.net.NetBase.mountain_amount_score> mountain_amount_score = new List<st.net.NetBase.mountain_amount_score>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmountain_amount_score = reader.Read_ushort();
		mountain_amount_score = new List<st.net.NetBase.mountain_amount_score>();
		for(int i_mountain_amount_score = 0 ; i_mountain_amount_score < lenmountain_amount_score ; i_mountain_amount_score ++)
		{
			st.net.NetBase.mountain_amount_score listData = new st.net.NetBase.mountain_amount_score();
			listData.fromBinary(reader);
			mountain_amount_score.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmountain_amount_score = (ushort)mountain_amount_score.Count;
		writer.write_short(lenmountain_amount_score);
		for(int i_mountain_amount_score = 0 ; i_mountain_amount_score < lenmountain_amount_score ; i_mountain_amount_score ++)
		{
			st.net.NetBase.mountain_amount_score listData = mountain_amount_score[i_mountain_amount_score];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
