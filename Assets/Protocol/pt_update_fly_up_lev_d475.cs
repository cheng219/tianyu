using System.Collections;
using System.Collections.Generic;

public class pt_update_fly_up_lev_d475 : st.net.NetBase.Pt {
	public pt_update_fly_up_lev_d475()
	{
		Id = 0xD475;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_fly_up_lev_d475();
	}
	public uint lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		lev = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(lev);
		return writer.data;
	}

}
