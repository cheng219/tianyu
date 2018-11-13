using System.Collections;
using System.Collections.Generic;

public class pt_mdf_team_info_e10e : st.net.NetBase.Pt {
	public pt_mdf_team_info_e10e()
	{
		Id = 0xE10E;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mdf_team_info_e10e();
	}
	public byte min_lev;
	public byte max_plys;
	public byte need_verify;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		min_lev = reader.Read_byte();
		max_plys = reader.Read_byte();
		need_verify = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(min_lev);
		writer.write_byte(max_plys);
		writer.write_byte(need_verify);
		return writer.data;
	}

}
