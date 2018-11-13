using System.Collections;
using System.Collections.Generic;

public class pt_usr_info_b102 : st.net.NetBase.Pt {
	public pt_usr_info_b102()
	{
		Id = 0xB102;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_info_b102();
	}
	public uint id;
	public string name;
	public uint level;
	public ulong exp;
	public uint prof;
	public uint cur_hp;
	public uint cur_mp;
	public List<st.net.NetBase.property> property_list = new List<st.net.NetBase.property>();
	public List<uint> equip_id_list = new List<uint>();
	public uint camp;
	public List<st.net.NetBase.resource_list> resource_list = new List<st.net.NetBase.resource_list>();
	public string guild_name;
	public List<int> model_clothes_id = new List<int>();
	public uint title_id;
	public uint sla;
	public int magic_weapon_id;
	public int magic_strength_lev;
	public int magic_strength_star;
	public int fiight_score;
	public uint vip_lev;
	public uint guild_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		name = reader.Read_str();
		level = reader.Read_uint();
		exp = reader.Read_ulong();
		prof = reader.Read_uint();
		cur_hp = reader.Read_uint();
		cur_mp = reader.Read_uint();
		ushort lenproperty_list = reader.Read_ushort();
		property_list = new List<st.net.NetBase.property>();
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property listData = new st.net.NetBase.property();
			listData.fromBinary(reader);
			property_list.Add(listData);
		}
		ushort lenequip_id_list = reader.Read_ushort();
		equip_id_list = new List<uint>();
		for(int i_equip_id_list = 0 ; i_equip_id_list < lenequip_id_list ; i_equip_id_list ++)
		{
			uint listData = reader.Read_uint();
			equip_id_list.Add(listData);
		}
		camp = reader.Read_uint();
		ushort lenresource_list = reader.Read_ushort();
		resource_list = new List<st.net.NetBase.resource_list>();
		for(int i_resource_list = 0 ; i_resource_list < lenresource_list ; i_resource_list ++)
		{
			st.net.NetBase.resource_list listData = new st.net.NetBase.resource_list();
			listData.fromBinary(reader);
			resource_list.Add(listData);
		}
		guild_name = reader.Read_str();
		ushort lenmodel_clothes_id = reader.Read_ushort();
		model_clothes_id = new List<int>();
		for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
		{
			int listData = reader.Read_int();
			model_clothes_id.Add(listData);
		}
		title_id = reader.Read_uint();
		sla = reader.Read_uint();
		magic_weapon_id = reader.Read_int();
		magic_strength_lev = reader.Read_int();
		magic_strength_star = reader.Read_int();
		fiight_score = reader.Read_int();
		vip_lev = reader.Read_uint();
		guild_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_str(name);
		writer.write_int(level);
		writer.write_long(exp);
		writer.write_int(prof);
		writer.write_int(cur_hp);
		writer.write_int(cur_mp);
		ushort lenproperty_list = (ushort)property_list.Count;
		writer.write_short(lenproperty_list);
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property listData = property_list[i_property_list];
			listData.toBinary(writer);
		}
		ushort lenequip_id_list = (ushort)equip_id_list.Count;
		writer.write_short(lenequip_id_list);
		for(int i_equip_id_list = 0 ; i_equip_id_list < lenequip_id_list ; i_equip_id_list ++)
		{
			uint listData = equip_id_list[i_equip_id_list];
			writer.write_int(listData);
		}
		writer.write_int(camp);
		ushort lenresource_list = (ushort)resource_list.Count;
		writer.write_short(lenresource_list);
		for(int i_resource_list = 0 ; i_resource_list < lenresource_list ; i_resource_list ++)
		{
			st.net.NetBase.resource_list listData = resource_list[i_resource_list];
			listData.toBinary(writer);
		}
		writer.write_str(guild_name);
		ushort lenmodel_clothes_id = (ushort)model_clothes_id.Count;
		writer.write_short(lenmodel_clothes_id);
		for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
		{
			int listData = model_clothes_id[i_model_clothes_id];
			writer.write_int(listData);
		}
		writer.write_int(title_id);
		writer.write_int(sla);
		writer.write_int(magic_weapon_id);
		writer.write_int(magic_strength_lev);
		writer.write_int(magic_strength_star);
		writer.write_int(fiight_score);
		writer.write_int(vip_lev);
		writer.write_int(guild_id);
		return writer.data;
	}

}
