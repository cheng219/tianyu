using System.Collections;
using System.Collections.Generic;

public class pt_sync_time_b105 : st.net.NetBase.Pt {
	public pt_sync_time_b105()
	{
		Id = 0xB105;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_sync_time_b105();
	}
	public uint time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		time = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(time);
		return writer.data;
	}

}
