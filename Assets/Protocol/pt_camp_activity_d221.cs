using System.Collections;
using System.Collections.Generic;

public class pt_camp_activity_d221 : st.net.NetBase.Pt {
	public pt_camp_activity_d221()
	{
		Id = 0xD221;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_camp_activity_d221();
	}
	public uint union_score;
	public uint tribe_score;
	public uint time_len;
	public uint need_exp;
	public uint curr_exp;
	public uint can_get_prize;
	public uint win_camp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		union_score = reader.Read_uint();
		tribe_score = reader.Read_uint();
		time_len = reader.Read_uint();
		need_exp = reader.Read_uint();
		curr_exp = reader.Read_uint();
		can_get_prize = reader.Read_uint();
		win_camp = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(union_score);
		writer.write_int(tribe_score);
		writer.write_int(time_len);
		writer.write_int(need_exp);
		writer.write_int(curr_exp);
		writer.write_int(can_get_prize);
		writer.write_int(win_camp);
		return writer.data;
	}

}
