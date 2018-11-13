using System.Collections;
using System.Collections.Generic;

public class pt_task_count_talk_to_npc_d699 : st.net.NetBase.Pt {
	public pt_task_count_talk_to_npc_d699()
	{
		Id = 0xD699;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_task_count_talk_to_npc_d699();
	}
	public int npc_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		npc_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(npc_id);
		return writer.data;
	}

}
