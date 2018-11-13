using System.Collections;
using System.Collections.Generic;

public class pt_bind_result_d815 : st.net.NetBase.Pt {
	public pt_bind_result_d815()
	{
		Id = 0xD815;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_bind_result_d815();
	}
	public uint id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
