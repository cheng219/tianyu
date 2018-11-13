using System.Collections;
using System.Collections.Generic;

public class pt_req_fresh_surround_task_star_d330 : st.net.NetBase.Pt {
	public pt_req_fresh_surround_task_star_d330()
	{
		Id = 0xD330;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_fresh_surround_task_star_d330();
	}
	public uint refresh_difficulty;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		refresh_difficulty = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(refresh_difficulty);
		return writer.data;
	}

}
