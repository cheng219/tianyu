using System.Collections;
using System.Collections.Generic;

public class pt_look_usr_list_d712 : st.net.NetBase.Pt {
	public pt_look_usr_list_d712()
	{
		Id = 0xD712;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_look_usr_list_d712();
	}
	public int uid;
	public string name;
	public int battle;
	public int lev;
	public int vip_lev;
	public int prof;
	public string guild_name;
	public int slaughter;
	public List<st.net.NetBase.property> target_property = new List<st.net.NetBase.property>();
	public int luck_num;
	public List<st.net.NetBase.normal_skill_list> target_skill = new List<st.net.NetBase.normal_skill_list>();
	public List<int> model_clothes_id = new List<int>();
	public int wing_id;
	public int wing_lev;
	public List<st.net.NetBase.item_des> target_equip_list = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		name = reader.Read_str();
		battle = reader.Read_int();
		lev = reader.Read_int();
		vip_lev = reader.Read_int();
		prof = reader.Read_int();
		guild_name = reader.Read_str();
		slaughter = reader.Read_int();
		ushort lentarget_property = reader.Read_ushort();
		target_property = new List<st.net.NetBase.property>();
		for(int i_target_property = 0 ; i_target_property < lentarget_property ; i_target_property ++)
		{
			st.net.NetBase.property listData = new st.net.NetBase.property();
			listData.fromBinary(reader);
			target_property.Add(listData);
		}
		luck_num = reader.Read_int();
		ushort lentarget_skill = reader.Read_ushort();
		target_skill = new List<st.net.NetBase.normal_skill_list>();
		for(int i_target_skill = 0 ; i_target_skill < lentarget_skill ; i_target_skill ++)
		{
			st.net.NetBase.normal_skill_list listData = new st.net.NetBase.normal_skill_list();
			listData.fromBinary(reader);
			target_skill.Add(listData);
		}
		ushort lenmodel_clothes_id = reader.Read_ushort();
		model_clothes_id = new List<int>();
		for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
		{
			int listData = reader.Read_int();
			model_clothes_id.Add(listData);
		}
		wing_id = reader.Read_int();
		wing_lev = reader.Read_int();
		ushort lentarget_equip_list = reader.Read_ushort();
		target_equip_list = new List<st.net.NetBase.item_des>();
		for(int i_target_equip_list = 0 ; i_target_equip_list < lentarget_equip_list ; i_target_equip_list ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			target_equip_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_str(name);
		writer.write_int(battle);
		writer.write_int(lev);
		writer.write_int(vip_lev);
		writer.write_int(prof);
		writer.write_str(guild_name);
		writer.write_int(slaughter);
		ushort lentarget_property = (ushort)target_property.Count;
		writer.write_short(lentarget_property);
		for(int i_target_property = 0 ; i_target_property < lentarget_property ; i_target_property ++)
		{
			st.net.NetBase.property listData = target_property[i_target_property];
			listData.toBinary(writer);
		}
		writer.write_int(luck_num);
		ushort lentarget_skill = (ushort)target_skill.Count;
		writer.write_short(lentarget_skill);
		for(int i_target_skill = 0 ; i_target_skill < lentarget_skill ; i_target_skill ++)
		{
			st.net.NetBase.normal_skill_list listData = target_skill[i_target_skill];
			listData.toBinary(writer);
		}
		ushort lenmodel_clothes_id = (ushort)model_clothes_id.Count;
		writer.write_short(lenmodel_clothes_id);
		for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
		{
			int listData = model_clothes_id[i_model_clothes_id];
			writer.write_int(listData);
		}
		writer.write_int(wing_id);
		writer.write_int(wing_lev);
		ushort lentarget_equip_list = (ushort)target_equip_list.Count;
		writer.write_short(lentarget_equip_list);
		for(int i_target_equip_list = 0 ; i_target_equip_list < lentarget_equip_list ; i_target_equip_list ++)
		{
			st.net.NetBase.item_des listData = target_equip_list[i_target_equip_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
