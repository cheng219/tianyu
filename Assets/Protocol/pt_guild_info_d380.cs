using System.Collections;
using System.Collections.Generic;

public class pt_guild_info_d380 : st.net.NetBase.Pt {
	public pt_guild_info_d380()
	{
		Id = 0xD380;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_info_d380();
	}
	public int id;
	public string lead_name;
	public int rank;
	public string name;
	public int exp;
	public int lev;
	public int members;
	public string purpose;
	public int pos;
	public int get_salary_state;
	public int expand_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		lead_name = reader.Read_str();
		rank = reader.Read_int();
		name = reader.Read_str();
		exp = reader.Read_int();
		lev = reader.Read_int();
		members = reader.Read_int();
		purpose = reader.Read_str();
		pos = reader.Read_int();
		get_salary_state = reader.Read_int();
		expand_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_str(lead_name);
		writer.write_int(rank);
		writer.write_str(name);
		writer.write_int(exp);
		writer.write_int(lev);
		writer.write_int(members);
		writer.write_str(purpose);
		writer.write_int(pos);
		writer.write_int(get_salary_state);
		writer.write_int(expand_num);
		return writer.data;
	}

}
