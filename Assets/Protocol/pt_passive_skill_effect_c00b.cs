using System.Collections;
using System.Collections.Generic;

public class pt_passive_skill_effect_c00b : st.net.NetBase.Pt {
	public pt_passive_skill_effect_c00b()
	{
		Id = 0xC00B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_passive_skill_effect_c00b();
	}
	public uint skill_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		skill_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(skill_id);
		return writer.data;
	}

}
