using System.Collections;
using System.Collections.Generic;

public class pt_entourage_create_d01f : st.net.NetBase.Pt {
	public pt_entourage_create_d01f()
	{
		Id = 0xD01F;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_entourage_create_d01f();
	}
	public uint oid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		return writer.data;
	}

}
