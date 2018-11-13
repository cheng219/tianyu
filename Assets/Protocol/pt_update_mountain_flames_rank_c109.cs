using System.Collections;
using System.Collections.Generic;

public class pt_update_mountain_flames_rank_c109 : st.net.NetBase.Pt {
	public pt_update_mountain_flames_rank_c109()
	{
		Id = 0xC109;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_mountain_flames_rank_c109();
	}
	public List<st.net.NetBase.mountain_flames_rank> mountain_flames_rank = new List<st.net.NetBase.mountain_flames_rank>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmountain_flames_rank = reader.Read_ushort();
		mountain_flames_rank = new List<st.net.NetBase.mountain_flames_rank>();
		for(int i_mountain_flames_rank = 0 ; i_mountain_flames_rank < lenmountain_flames_rank ; i_mountain_flames_rank ++)
		{
			st.net.NetBase.mountain_flames_rank listData = new st.net.NetBase.mountain_flames_rank();
			listData.fromBinary(reader);
			mountain_flames_rank.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmountain_flames_rank = (ushort)mountain_flames_rank.Count;
		writer.write_short(lenmountain_flames_rank);
		for(int i_mountain_flames_rank = 0 ; i_mountain_flames_rank < lenmountain_flames_rank ; i_mountain_flames_rank ++)
		{
			st.net.NetBase.mountain_flames_rank listData = mountain_flames_rank[i_mountain_flames_rank];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
