using System.Collections;
using System.Collections.Generic;

public class pt_update_wing_lev_d420 : st.net.NetBase.Pt {
	public pt_update_wing_lev_d420()
	{
		Id = 0xD420;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_wing_lev_d420();
	}
	public int wing_id;
	public int lev;
	public int exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		wing_id = reader.Read_int();
		lev = reader.Read_int();
		exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(wing_id);
		writer.write_int(lev);
		writer.write_int(exp);
		return writer.data;
	}

}
