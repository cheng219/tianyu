using System.Collections;
using System.Collections.Generic;

public class pt_scene_skill_effect_c006 : st.net.NetBase.Pt {
	public pt_scene_skill_effect_c006()
	{
		Id = 0xC006;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_skill_effect_c006();
	}
	public uint oid;
	public byte obj_sort;
	public uint skill;
	public uint lev;
	public uint rune;
	public byte effect_sort;
	public float x;
	public float y;
	public float z;
	public float dir;
	public uint target_id;
	public float target_x;
	public float target_y;
	public float target_z;
	public float shift_x;
	public float shift_y;
	public float shift_z;
	public List<st.net.NetBase.skill_effect> effect_list = new List<st.net.NetBase.skill_effect>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		obj_sort = reader.Read_byte();
		skill = reader.Read_uint();
		lev = reader.Read_uint();
		rune = reader.Read_uint();
		effect_sort = reader.Read_byte();
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
		dir = reader.Read_float();
		target_id = reader.Read_uint();
		target_x = reader.Read_float();
		target_y = reader.Read_float();
		target_z = reader.Read_float();
		shift_x = reader.Read_float();
		shift_y = reader.Read_float();
		shift_z = reader.Read_float();
		ushort leneffect_list = reader.Read_ushort();
		effect_list = new List<st.net.NetBase.skill_effect>();
		for(int i_effect_list = 0 ; i_effect_list < leneffect_list ; i_effect_list ++)
		{
			st.net.NetBase.skill_effect listData = new st.net.NetBase.skill_effect();
			listData.fromBinary(reader);
			effect_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_byte(obj_sort);
		writer.write_int(skill);
		writer.write_int(lev);
		writer.write_int(rune);
		writer.write_byte(effect_sort);
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		writer.write_float(dir);
		writer.write_int(target_id);
		writer.write_float(target_x);
		writer.write_float(target_y);
		writer.write_float(target_z);
		writer.write_float(shift_x);
		writer.write_float(shift_y);
		writer.write_float(shift_z);
		ushort leneffect_list = (ushort)effect_list.Count;
		writer.write_short(leneffect_list);
		for(int i_effect_list = 0 ; i_effect_list < leneffect_list ; i_effect_list ++)
		{
			st.net.NetBase.skill_effect listData = effect_list[i_effect_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
