using System.Collections;
using System.Collections.Generic;

public class pt_ans_recruit_friend_d749 : st.net.NetBase.Pt {
	public pt_ans_recruit_friend_d749()
	{
		Id = 0xD749;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ans_recruit_friend_d749();
	}
	public int uid;
	public string name;
	public int lev;
	public int fight_score;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		name = reader.Read_str();
		lev = reader.Read_int();
		fight_score = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_str(name);
		writer.write_int(lev);
		writer.write_int(fight_score);
		return writer.data;
	}

}
