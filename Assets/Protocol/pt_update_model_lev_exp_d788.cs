using System.Collections;
using System.Collections.Generic;

public class pt_update_model_lev_exp_d788 : st.net.NetBase.Pt {
	public pt_update_model_lev_exp_d788()
	{
		Id = 0xD788;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_model_lev_exp_d788();
	}
	public int lev;
	public int exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		lev = reader.Read_int();
		exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(lev);
		writer.write_int(exp);
		return writer.data;
	}

}
