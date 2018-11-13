using System.Collections;
using System.Collections.Generic;

public class pt_strengthen_info_d361 : st.net.NetBase.Pt {
	public pt_strengthen_info_d361()
	{
		Id = 0xD361;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_strengthen_info_d361();
	}
	public uint id;
	public uint exp;
	public uint lev;
	public uint success;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		exp = reader.Read_uint();
		lev = reader.Read_uint();
		success = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(exp);
		writer.write_int(lev);
		writer.write_int(success);
		return writer.data;
	}

}
