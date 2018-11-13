using System.Collections;
using System.Collections.Generic;

public class pt_backpack_upgrade_d140 : st.net.NetBase.Pt {
	public pt_backpack_upgrade_d140()
	{
		Id = 0xD140;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_backpack_upgrade_d140();
	}
	public int backpack_lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		backpack_lev = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(backpack_lev);
		return writer.data;
	}

}
