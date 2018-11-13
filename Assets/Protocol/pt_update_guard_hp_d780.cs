using System.Collections;
using System.Collections.Generic;

public class pt_update_guard_hp_d780 : st.net.NetBase.Pt {
	public pt_update_guard_hp_d780()
	{
		Id = 0xD780;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_guard_hp_d780();
	}
	public int guard_npc_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guard_npc_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(guard_npc_id);
		return writer.data;
	}

}
