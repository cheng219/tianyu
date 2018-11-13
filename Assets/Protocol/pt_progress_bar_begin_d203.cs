using System.Collections;
using System.Collections.Generic;

public class pt_progress_bar_begin_d203 : st.net.NetBase.Pt {
	public pt_progress_bar_begin_d203()
	{
		Id = 0xD203;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_progress_bar_begin_d203();
	}
	public uint oid;
	public uint type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_int(type);
		return writer.data;
	}

}
