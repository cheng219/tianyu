using System.Collections;
using System.Collections.Generic;

public class pt_progress_bar_end_d204 : st.net.NetBase.Pt {
	public pt_progress_bar_end_d204()
	{
		Id = 0xD204;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_progress_bar_end_d204();
	}
	public uint oid;
	public byte result;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		result = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_byte(result);
		return writer.data;
	}

}
