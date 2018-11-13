using System.Collections;
using System.Collections.Generic;

public class pt_start_timer_d212 : st.net.NetBase.Pt {
	public pt_start_timer_d212()
	{
		Id = 0xD212;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_start_timer_d212();
	}
	public uint timelen;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		timelen = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(timelen);
		return writer.data;
	}

}
