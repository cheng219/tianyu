using System.Collections;
using System.Collections.Generic;

public class pt_match_succ_d218 : st.net.NetBase.Pt {
	public pt_match_succ_d218()
	{
		Id = 0xD218;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_match_succ_d218();
	}
	public uint groupid;
	public List<st.net.NetBase.match_succ_list> memebers = new List<st.net.NetBase.match_succ_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		groupid = reader.Read_uint();
		ushort lenmemebers = reader.Read_ushort();
		memebers = new List<st.net.NetBase.match_succ_list>();
		for(int i_memebers = 0 ; i_memebers < lenmemebers ; i_memebers ++)
		{
			st.net.NetBase.match_succ_list listData = new st.net.NetBase.match_succ_list();
			listData.fromBinary(reader);
			memebers.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(groupid);
		ushort lenmemebers = (ushort)memebers.Count;
		writer.write_short(lenmemebers);
		for(int i_memebers = 0 ; i_memebers < lenmemebers ; i_memebers ++)
		{
			st.net.NetBase.match_succ_list listData = memebers[i_memebers];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
