using System.Collections;
using System.Collections.Generic;

public class pt_keepsake_info_d534 : st.net.NetBase.Pt {
	public pt_keepsake_info_d534()
	{
		Id = 0xD534;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_keepsake_info_d534();
	}
	public int type;
	public int lev;
	public int exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		lev = reader.Read_int();
		exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(lev);
		writer.write_int(exp);
		return writer.data;
	}

}
