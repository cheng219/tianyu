using System.Collections;
using System.Collections.Generic;

public class pt_scene_delete_trap_c017 : st.net.NetBase.Pt {
	public pt_scene_delete_trap_c017()
	{
		Id = 0xC017;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_delete_trap_c017();
	}
	public uint tid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		tid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(tid);
		return writer.data;
	}

}
