using System.Collections;
using System.Collections.Generic;

public class pt_shilian_task_info_d698 : st.net.NetBase.Pt {
	public pt_shilian_task_info_d698()
	{
		Id = 0xD698;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_shilian_task_info_d698();
	}
	public int num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(num);
		return writer.data;
	}

}
