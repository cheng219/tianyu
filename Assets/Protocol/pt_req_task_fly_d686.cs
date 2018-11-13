using System.Collections;
using System.Collections.Generic;

public class pt_req_task_fly_d686 : st.net.NetBase.Pt {
	public pt_req_task_fly_d686()
	{
		Id = 0xD686;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_task_fly_d686();
	}
	public int scene;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		scene = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(scene);
		return writer.data;
	}

}
