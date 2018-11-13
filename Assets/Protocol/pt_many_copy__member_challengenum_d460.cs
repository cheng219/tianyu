using System.Collections;
using System.Collections.Generic;

public class pt_many_copy__member_challengenum_d460 : st.net.NetBase.Pt {
	public pt_many_copy__member_challengenum_d460()
	{
		Id = 0xD460;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_many_copy__member_challengenum_d460();
	}
	public int copy_id;
	public int copy_type;
	public List<st.net.NetBase.member_challengenum_list> member_challengenum = new List<st.net.NetBase.member_challengenum_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_int();
		copy_type = reader.Read_int();
		ushort lenmember_challengenum = reader.Read_ushort();
		member_challengenum = new List<st.net.NetBase.member_challengenum_list>();
		for(int i_member_challengenum = 0 ; i_member_challengenum < lenmember_challengenum ; i_member_challengenum ++)
		{
			st.net.NetBase.member_challengenum_list listData = new st.net.NetBase.member_challengenum_list();
			listData.fromBinary(reader);
			member_challengenum.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(copy_type);
		ushort lenmember_challengenum = (ushort)member_challengenum.Count;
		writer.write_short(lenmember_challengenum);
		for(int i_member_challengenum = 0 ; i_member_challengenum < lenmember_challengenum ; i_member_challengenum ++)
		{
			st.net.NetBase.member_challengenum_list listData = member_challengenum[i_member_challengenum];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
