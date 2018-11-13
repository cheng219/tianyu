using System.Collections;
using System.Collections.Generic;

public class pt_req_task_surround_ui_info_c142 : st.net.NetBase.Pt {
	public pt_req_task_surround_ui_info_c142()
	{
		Id = 0xC142;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_task_surround_ui_info_c142();
	}
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		return writer.data;
	}

}
