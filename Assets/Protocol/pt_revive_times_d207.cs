using System.Collections;
using System.Collections.Generic;

public class pt_revive_times_d207 : st.net.NetBase.Pt {
	public pt_revive_times_d207()
	{
		Id = 0xD207;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_revive_times_d207();
	}
	public uint times;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		times = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(times);
		return writer.data;
	}

}
