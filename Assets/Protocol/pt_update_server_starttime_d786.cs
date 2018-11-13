using System.Collections;
using System.Collections.Generic;

public class pt_update_server_starttime_d786 : st.net.NetBase.Pt {
	public pt_update_server_starttime_d786()
	{
		Id = 0xD786;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_server_starttime_d786();
	}
	public int start_time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		start_time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(start_time);
		return writer.data;
	}

}
