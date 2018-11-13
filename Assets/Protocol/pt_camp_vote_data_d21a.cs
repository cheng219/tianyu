using System.Collections;
using System.Collections.Generic;

public class pt_camp_vote_data_d21a : st.net.NetBase.Pt {
	public pt_camp_vote_data_d21a()
	{
		Id = 0xD21A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_camp_vote_data_d21a();
	}
	public uint can_vote;
	public List<st.net.NetBase.camp_vote_list> campvotes = new List<st.net.NetBase.camp_vote_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		can_vote = reader.Read_uint();
		ushort lencampvotes = reader.Read_ushort();
		campvotes = new List<st.net.NetBase.camp_vote_list>();
		for(int i_campvotes = 0 ; i_campvotes < lencampvotes ; i_campvotes ++)
		{
			st.net.NetBase.camp_vote_list listData = new st.net.NetBase.camp_vote_list();
			listData.fromBinary(reader);
			campvotes.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(can_vote);
		ushort lencampvotes = (ushort)campvotes.Count;
		writer.write_short(lencampvotes);
		for(int i_campvotes = 0 ; i_campvotes < lencampvotes ; i_campvotes ++)
		{
			st.net.NetBase.camp_vote_list listData = campvotes[i_campvotes];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
