using System.Collections;
using System.Collections.Generic;

public class pt_scene_skill_aleret_cancel_c022 : st.net.NetBase.Pt {
	public pt_scene_skill_aleret_cancel_c022()
	{
		Id = 0xC022;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_skill_aleret_cancel_c022();
	}
	public uint skill;
	public uint skill_rune;
	public uint lev;
	public uint oid;
	public uint obj_sort;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		skill = reader.Read_uint();
		skill_rune = reader.Read_uint();
		lev = reader.Read_uint();
		oid = reader.Read_uint();
		obj_sort = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(skill);
		writer.write_int(skill_rune);
		writer.write_int(lev);
		writer.write_int(oid);
		writer.write_int(obj_sort);
		return writer.data;
	}

}
