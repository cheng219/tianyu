using System.Collections;
using System.Collections.Generic;

public class pt_update_ride_skin_lev_d757 : st.net.NetBase.Pt {
	public pt_update_ride_skin_lev_d757()
	{
		Id = 0xD757;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_ride_skin_lev_d757();
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