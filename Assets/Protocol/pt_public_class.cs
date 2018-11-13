using System.Collections;
using System.Collections.Generic;

namespace st.net.NetBase
{
	public class normal_info
	{
		public int type;
		public string data;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			data = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_str(data);
		}
	}
	public class point3
	{
		public float x;
		public float y;
		public float z;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
		}
	}
	public class create_usr_info
	{
		public uint id;
		public string name;
		public uint level;
		public uint prof;
		public uint create_time;
		public uint raw_server_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			name = reader.Read_str();
			level = reader.Read_uint();
			prof = reader.Read_uint();
			create_time = reader.Read_uint();
			raw_server_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_str(name);
			writer.write_int(level);
			writer.write_int(prof);
			writer.write_int(create_time);
			writer.write_int(raw_server_id);
		}
	}
	public class scene_ply
	{
		public uint pid;
		public string name;
		public uint level;
		public byte prof;
		public float dir;
		public float x;
		public float y;
		public float z;
		public byte camp;
		public uint hp;
		public uint max_hp;
		public uint mp;
		public uint max_mp;
		public uint move_speed;
		public uint team_id;
		public uint team_leader;
		public byte is_die;
		public uint title;
		public uint fighting;
		public byte ride_state;
		public uint ride_type;
		public int ride_lev;
		public uint currskin;
		public string guildName;
		public List<int> model_clothes_id = new List<int>();
		public byte pk_level;
		public int magic_weapon_id;
		public int magic_strength_lev;
		public int magic_strength_star;
		public int wing_id;
		public int wing_lev;
		public int strenthen_min_lev;
		public byte counter_status;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			pid = reader.Read_uint();
			name = reader.Read_str();
			level = reader.Read_uint();
			prof = reader.Read_byte();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
			camp = reader.Read_byte();
			hp = reader.Read_uint();
			max_hp = reader.Read_uint();
			mp = reader.Read_uint();
			max_mp = reader.Read_uint();
			move_speed = reader.Read_uint();
			team_id = reader.Read_uint();
			team_leader = reader.Read_uint();
			is_die = reader.Read_byte();
			title = reader.Read_uint();
			fighting = reader.Read_uint();
			ride_state = reader.Read_byte();
			ride_type = reader.Read_uint();
			ride_lev = reader.Read_int();
			currskin = reader.Read_uint();
			guildName = reader.Read_str();
			ushort lenmodel_clothes_id = reader.Read_ushort();
			model_clothes_id = new List<int>();
			for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
			{
				int listData = reader.Read_int();
				model_clothes_id.Add(listData);
			}
			pk_level = reader.Read_byte();
			magic_weapon_id = reader.Read_int();
			magic_strength_lev = reader.Read_int();
			magic_strength_star = reader.Read_int();
			wing_id = reader.Read_int();
			wing_lev = reader.Read_int();
			strenthen_min_lev = reader.Read_int();
			counter_status = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(pid);
			writer.write_str(name);
			writer.write_int(level);
			writer.write_byte(prof);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
			writer.write_byte(camp);
			writer.write_int(hp);
			writer.write_int(max_hp);
			writer.write_int(mp);
			writer.write_int(max_mp);
			writer.write_int(move_speed);
			writer.write_int(team_id);
			writer.write_int(team_leader);
			writer.write_byte(is_die);
			writer.write_int(title);
			writer.write_int(fighting);
			writer.write_byte(ride_state);
			writer.write_int(ride_type);
			writer.write_int(ride_lev);
			writer.write_int(currskin);
			writer.write_str(guildName);
			ushort lenmodel_clothes_id = (ushort)model_clothes_id.Count;
			writer.write_short(lenmodel_clothes_id);
			for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
			{
				int listData = model_clothes_id[i_model_clothes_id];
				writer.write_int(listData);
			}
			writer.write_byte(pk_level);
			writer.write_int(magic_weapon_id);
			writer.write_int(magic_strength_lev);
			writer.write_int(magic_strength_star);
			writer.write_int(wing_id);
			writer.write_int(wing_lev);
			writer.write_int(strenthen_min_lev);
			writer.write_byte(counter_status);
		}
	}
	public class scene_monster
	{
		public uint mid;
		public uint type;
		public uint level;
		public float dir;
		public float x;
		public float y;
		public float z;
		public uint camp;
		public uint hp;
		public uint max_hp;
		public uint mp;
		public uint max_mp;
		public int ownerID;
		public uint cart_owner;
		public string cart_owner_name;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			mid = reader.Read_uint();
			type = reader.Read_uint();
			level = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
			camp = reader.Read_uint();
			hp = reader.Read_uint();
			max_hp = reader.Read_uint();
			mp = reader.Read_uint();
			max_mp = reader.Read_uint();
			ownerID = reader.Read_int();
			cart_owner = reader.Read_uint();
			cart_owner_name = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(mid);
			writer.write_int(type);
			writer.write_int(level);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
			writer.write_int(camp);
			writer.write_int(hp);
			writer.write_int(max_hp);
			writer.write_int(mp);
			writer.write_int(max_mp);
			writer.write_int(ownerID);
			writer.write_int(cart_owner);
			writer.write_str(cart_owner_name);
		}
	}
	public class scene_item
	{
		public uint iid;
		public uint type;
		public float dir;
		public float x;
		public float y;
		public float z;
		public uint camp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			iid = reader.Read_uint();
			type = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
			camp = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(iid);
			writer.write_int(type);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
			writer.write_int(camp);
		}
	}
	public class skill_effect
	{
		public uint target_id;
		public uint target_sort;
		public uint atk_sort;
		public uint def_sort;
		public uint demage;
		public uint cur_hp;
		public float effect_x;
		public float effect_y;
		public float effect_z;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			target_id = reader.Read_uint();
			target_sort = reader.Read_uint();
			atk_sort = reader.Read_uint();
			def_sort = reader.Read_uint();
			demage = reader.Read_uint();
			cur_hp = reader.Read_uint();
			effect_x = reader.Read_float();
			effect_y = reader.Read_float();
			effect_z = reader.Read_float();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(target_id);
			writer.write_int(target_sort);
			writer.write_int(atk_sort);
			writer.write_int(def_sort);
			writer.write_int(demage);
			writer.write_int(cur_hp);
			writer.write_float(effect_x);
			writer.write_float(effect_y);
			writer.write_float(effect_z);
		}
	}
	public class property
	{
		public byte sort;
		public ushort type;
		public int data;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			sort = reader.Read_byte();
			type = reader.Read_ushort();
			data = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_byte(sort);
			writer.write_short(type);
			writer.write_int(data);
		}
	}
	public class property64
	{
		public byte sort;
		public ushort type;
		public long data;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			sort = reader.Read_byte();
			type = reader.Read_ushort();
			data = reader.Read_long();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_byte(sort);
			writer.write_short(type);
			writer.write_long(data);
		}
	}
	public class item_des
	{
		public uint id;
		public uint type;
		public uint num;
		public uint pos;
		public byte bind;
		public uint lev;
		public byte color;
		public uint equOne;
		public uint equTwo;
		public uint equThree;
		public uint equFour;
		public uint strenthen_exp;
		public uint recast_exp;
		public uint exp_expire_time;
		public uint luck;
		public List<st.net.NetBase.pos_des> pos_list = new List<st.net.NetBase.pos_des>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			type = reader.Read_uint();
			num = reader.Read_uint();
			pos = reader.Read_uint();
			bind = reader.Read_byte();
			lev = reader.Read_uint();
			color = reader.Read_byte();
			equOne = reader.Read_uint();
			equTwo = reader.Read_uint();
			equThree = reader.Read_uint();
			equFour = reader.Read_uint();
			strenthen_exp = reader.Read_uint();
			recast_exp = reader.Read_uint();
			exp_expire_time = reader.Read_uint();
			luck = reader.Read_uint();
			ushort lenpos_list = reader.Read_ushort();
			pos_list = new List<st.net.NetBase.pos_des>();
			for(int i_pos_list = 0 ; i_pos_list < lenpos_list ; i_pos_list ++)
			{
				st.net.NetBase.pos_des listData = new st.net.NetBase.pos_des();
				listData.fromBinary(reader);
				pos_list.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(num);
			writer.write_int(pos);
			writer.write_byte(bind);
			writer.write_int(lev);
			writer.write_byte(color);
			writer.write_int(equOne);
			writer.write_int(equTwo);
			writer.write_int(equThree);
			writer.write_int(equFour);
			writer.write_int(strenthen_exp);
			writer.write_int(recast_exp);
			writer.write_int(exp_expire_time);
			writer.write_int(luck);
			ushort lenpos_list = (ushort)pos_list.Count;
			writer.write_short(lenpos_list);
			for(int i_pos_list = 0 ; i_pos_list < lenpos_list ; i_pos_list ++)
			{
				st.net.NetBase.pos_des listData = pos_list[i_pos_list];
				listData.toBinary(writer);
			}
		}
	}
	public class drop_des
	{
		public uint id;
		public List<uint> owner = new List<uint>();
		public uint type;
		public uint num;
		public uint from_id;
		public float x;
		public float y;
		public float z;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			ushort lenowner = reader.Read_ushort();
			owner = new List<uint>();
			for(int i_owner = 0 ; i_owner < lenowner ; i_owner ++)
			{
				uint listData = reader.Read_uint();
				owner.Add(listData);
			}
			type = reader.Read_uint();
			num = reader.Read_uint();
			from_id = reader.Read_uint();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			ushort lenowner = (ushort)owner.Count;
			writer.write_short(lenowner);
			for(int i_owner = 0 ; i_owner < lenowner ; i_owner ++)
			{
				uint listData = owner[i_owner];
				writer.write_int(listData);
			}
			writer.write_int(type);
			writer.write_int(num);
			writer.write_int(from_id);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
		}
	}
	public class pickup_des
	{
		public uint deleted_state;
		public uint state;
		public uint id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			deleted_state = reader.Read_uint();
			state = reader.Read_uint();
			id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(deleted_state);
			writer.write_int(state);
			writer.write_int(id);
		}
	}
	public class lost_list
	{
		public uint lost_id;
		public uint can_get;
		public uint lev;
		public uint debris_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			lost_id = reader.Read_uint();
			can_get = reader.Read_uint();
			lev = reader.Read_uint();
			debris_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(lost_id);
			writer.write_int(can_get);
			writer.write_int(lev);
			writer.write_int(debris_num);
		}
	}
	public class skill_list
	{
		public uint skillId;
		public uint activateType;
		public uint skillLev;
		public uint runeId;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			skillId = reader.Read_uint();
			activateType = reader.Read_uint();
			skillLev = reader.Read_uint();
			runeId = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(skillId);
			writer.write_int(activateType);
			writer.write_int(skillLev);
			writer.write_int(runeId);
		}
	}
	public class equip_list
	{
		public uint equipId;
		public uint activateType;
		public uint itemType;
		public uint itemNum;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			equipId = reader.Read_uint();
			activateType = reader.Read_uint();
			itemType = reader.Read_uint();
			itemNum = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(equipId);
			writer.write_int(activateType);
			writer.write_int(itemType);
			writer.write_int(itemNum);
		}
	}
	public class property_list
	{
		public uint propertyId;
		public uint propertyVal;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			propertyId = reader.Read_uint();
			propertyVal = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(propertyId);
			writer.write_int(propertyVal);
		}
	}
	public class entourage_list
	{
		public uint etype;
		public uint fighting;
		public uint lev;
		public uint itemType;
		public uint itemVal;
		public uint estar;
		public uint fightType;
		public uint exp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			etype = reader.Read_uint();
			fighting = reader.Read_uint();
			lev = reader.Read_uint();
			itemType = reader.Read_uint();
			itemVal = reader.Read_uint();
			estar = reader.Read_uint();
			fightType = reader.Read_uint();
			exp = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(etype);
			writer.write_int(fighting);
			writer.write_int(lev);
			writer.write_int(itemType);
			writer.write_int(itemVal);
			writer.write_int(estar);
			writer.write_int(fightType);
			writer.write_int(exp);
		}
	}
	public class task_list_info
	{
		public uint taskid;
		public uint taskstep;
		public uint state;
		public uint c1_num;
		public uint c2_num;
		public uint c3_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			taskid = reader.Read_uint();
			taskstep = reader.Read_uint();
			state = reader.Read_uint();
			c1_num = reader.Read_uint();
			c2_num = reader.Read_uint();
			c3_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(taskid);
			writer.write_int(taskstep);
			writer.write_int(state);
			writer.write_int(c1_num);
			writer.write_int(c2_num);
			writer.write_int(c3_num);
		}
	}
public class normal_skill_list
	{
		public uint skill_id;
		public uint skill_lev;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			skill_id = reader.Read_uint();
			skill_lev = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(skill_id);
			writer.write_int(skill_lev);
		}
	}
	
		public class usr_use_skill_list
	{
		public int skill_1;
		public int skill_2;
		public int skill_3;
		public int skill_4;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			skill_1 = reader.Read_int();
			skill_2 = reader.Read_int();
			skill_3 = reader.Read_int();
			skill_4 = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(skill_1);
			writer.write_int(skill_2);
			writer.write_int(skill_3);
			writer.write_int(skill_4);
		}
	}
	
			public class scene_entourage
	{
		public uint eid;
		public uint type;
		public uint level;
		public float dir;
		public float x;
		public float y;
		public float z;
		public uint owner;
		public string owner_name;
		public uint camp;
		public uint hp;
		public uint max_hp;
		public uint mp;
		public uint max_mp;
		public string pet_name;
		public int pet_grow_up;
		public int pet_aptitude_lev;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			eid = reader.Read_uint();
			type = reader.Read_uint();
			level = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
			owner = reader.Read_uint();
			owner_name = reader.Read_str();
			camp = reader.Read_uint();
			hp = reader.Read_uint();
			max_hp = reader.Read_uint();
			mp = reader.Read_uint();
			max_mp = reader.Read_uint();
			pet_name = reader.Read_str();
			pet_grow_up = reader.Read_int();
			pet_aptitude_lev = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(eid);
			writer.write_int(type);
			writer.write_int(level);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
			writer.write_int(owner);
			writer.write_str(owner_name);
			writer.write_int(camp);
			writer.write_int(hp);
			writer.write_int(max_hp);
			writer.write_int(mp);
			writer.write_int(max_mp);
			writer.write_str(pet_name);
			writer.write_int(pet_grow_up);
			writer.write_int(pet_aptitude_lev);
		}
	}
	public class mastery_list
	{
		public uint mastery_id;
		public uint mastery_lev;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			mastery_id = reader.Read_uint();
			mastery_lev = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(mastery_id);
			writer.write_int(mastery_lev);
		}
	}
	public class team_member_list
	{
		public uint uid;
		public uint lev;
		public uint prof;
		public string name;
		public uint scene;
		public uint hp;
		public uint limit_hp;
		public uint fighting;
		public int robot_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			lev = reader.Read_uint();
			prof = reader.Read_uint();
			name = reader.Read_str();
			scene = reader.Read_uint();
			hp = reader.Read_uint();
			limit_hp = reader.Read_uint();
			fighting = reader.Read_uint();
			robot_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(lev);
			writer.write_int(prof);
			writer.write_str(name);
			writer.write_int(scene);
			writer.write_int(hp);
			writer.write_int(limit_hp);
			writer.write_int(fighting);
			writer.write_int(robot_state);
		}
	}
	public class usr_equip_list
	{
		public uint uid;
		public uint equip_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			equip_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(equip_id);
		}
	}
	public class equip_id_list
	{
		public uint equip_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			equip_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(equip_id);
		}
	}
	public class equip_id_state_list
	{
		public uint equip_id;
		public uint equip_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			equip_id = reader.Read_uint();
			equip_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(equip_id);
			writer.write_int(equip_state);
		}
	}
	public class scene_arrow
	{
		public uint owner_id;
		public uint owner_sort;
		public uint aid;
		public uint type;
		public float dir;
		public float x;
		public float y;
		public float z;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			owner_id = reader.Read_uint();
			owner_sort = reader.Read_uint();
			aid = reader.Read_uint();
			type = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(owner_id);
			writer.write_int(owner_sort);
			writer.write_int(aid);
			writer.write_int(type);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
		}
	}
	public class scene_trap
	{
		public uint owner_id;
		public uint owner_sort;
		public uint did;
		public uint type;
		public float dir;
		public float x;
		public float y;
		public float z;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			owner_id = reader.Read_uint();
			owner_sort = reader.Read_uint();
			did = reader.Read_uint();
			type = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(owner_id);
			writer.write_int(owner_sort);
			writer.write_int(did);
			writer.write_int(type);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
		}
	}
	public class pet_info
	{
		public uint config_id;
		public uint state;
		public uint time_len;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			config_id = reader.Read_uint();
			state = reader.Read_uint();
			time_len = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(config_id);
			writer.write_int(state);
			writer.write_int(time_len);
		}
	}
	public class store_cell_info
	{
		public uint config_id;
		public uint cell_id;
		public uint buy_num;
		public uint buy_price;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			config_id = reader.Read_uint();
			cell_id = reader.Read_uint();
			buy_num = reader.Read_uint();
			buy_price = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(config_id);
			writer.write_int(cell_id);
			writer.write_int(buy_num);
			writer.write_int(buy_price);
		}
	}
	public class gem_list
	{
		public uint gem_id;
		public uint gem_lev;
		public uint gem_exp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			gem_id = reader.Read_uint();
			gem_lev = reader.Read_uint();
			gem_exp = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(gem_id);
			writer.write_int(gem_lev);
			writer.write_int(gem_exp);
		}
	}
	public class pet_list
	{
		public uint uid;
		public uint pet_id;
		public uint pet_type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			pet_id = reader.Read_uint();
			pet_type = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(pet_id);
			writer.write_int(pet_type);
		}
	}
	public class recycle_list
	{
		public uint item_id;
		public uint item_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			item_id = reader.Read_uint();
			item_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(item_id);
			writer.write_int(item_num);
		}
	}
	public class sign_list
	{
		public uint sign_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			sign_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(sign_state);
		}
	}
	public class sign_rewards_list
	{
		public uint sign_rewards;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			sign_rewards = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(sign_rewards);
		}
	}
	public class activity_info
	{
		public uint activity_id;
		public uint activity_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			activity_id = reader.Read_uint();
			activity_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(activity_id);
			writer.write_int(activity_time);
		}
	}
	public class activity_rewards
	{
		public uint rewards_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			rewards_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(rewards_id);
		}
	}
	public class friends_info
	{
		public uint uid;
		public uint lev;
		public uint camp;
		public uint prof;
		public string name;
		public uint state;
		public uint by_thumb_up;
		public uint thumb_up;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			lev = reader.Read_uint();
			camp = reader.Read_uint();
			prof = reader.Read_uint();
			name = reader.Read_str();
			state = reader.Read_uint();
			by_thumb_up = reader.Read_uint();
			thumb_up = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(lev);
			writer.write_int(camp);
			writer.write_int(prof);
			writer.write_str(name);
			writer.write_int(state);
			writer.write_int(by_thumb_up);
			writer.write_int(thumb_up);
		}
	}
	public class item_list
	{
		public uint item_id;
		public uint item_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			item_id = reader.Read_uint();
			item_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(item_id);
			writer.write_int(item_num);
		}
	}
	public class history_info_list
	{
		public uint history_sort;
		public string object_name;
		public uint time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			history_sort = reader.Read_uint();
			object_name = reader.Read_str();
			time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(history_sort);
			writer.write_str(object_name);
			writer.write_int(time);
		}
	}
	public class enemy_info
	{
		public uint uid;
		public uint lev;
		public uint camp;
		public uint prof;
		public string name;
		public uint state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			lev = reader.Read_uint();
			camp = reader.Read_uint();
			prof = reader.Read_uint();
			name = reader.Read_str();
			state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(lev);
			writer.write_int(camp);
			writer.write_int(prof);
			writer.write_str(name);
			writer.write_int(state);
		}
	}
	public class copy_times
	{
		public uint group_id;
		public uint times;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			group_id = reader.Read_uint();
			times = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(group_id);
			writer.write_int(times);
		}
	}
	public class get_success_list
	{
		public uint success_action;
		public uint success_data;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			success_action = reader.Read_uint();
			success_data = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(success_action);
			writer.write_int(success_data);
		}
	}
	public class relation_info
	{
		public uint uid;
		public uint lev;
		public uint camp;
		public uint prof;
		public string name;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			lev = reader.Read_uint();
			camp = reader.Read_uint();
			prof = reader.Read_uint();
			name = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(lev);
			writer.write_int(camp);
			writer.write_int(prof);
			writer.write_str(name);
		}
	}
	public class guild_info_list
	{
		public uint guild_ranking;
		public uint guild_id;
		public string guild_name;
		public uint guild_camp;
		public string president_name;
		public uint member_amount;
		public uint guild_level;
		public uint req_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			guild_ranking = reader.Read_uint();
			guild_id = reader.Read_uint();
			guild_name = reader.Read_str();
			guild_camp = reader.Read_uint();
			president_name = reader.Read_str();
			member_amount = reader.Read_uint();
			guild_level = reader.Read_uint();
			req_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(guild_ranking);
			writer.write_int(guild_id);
			writer.write_str(guild_name);
			writer.write_int(guild_camp);
			writer.write_str(president_name);
			writer.write_int(member_amount);
			writer.write_int(guild_level);
			writer.write_int(req_state);
		}
	}
	public class guild_member_list
	{
		public uint member_id;
		public string member_name;
		public uint member_post;
		public uint memberlevel;
		public uint contribution;
		public uint recentOnlineTime;
		public uint friends_state;
		public uint online_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			member_id = reader.Read_uint();
			member_name = reader.Read_str();
			member_post = reader.Read_uint();
			memberlevel = reader.Read_uint();
			contribution = reader.Read_uint();
			recentOnlineTime = reader.Read_uint();
			friends_state = reader.Read_uint();
			online_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(member_id);
			writer.write_str(member_name);
			writer.write_int(member_post);
			writer.write_int(memberlevel);
			writer.write_int(contribution);
			writer.write_int(recentOnlineTime);
			writer.write_int(friends_state);
			writer.write_int(online_state);
		}
	}
	public class guild_members_entry_list
	{
		public uint uid;
		public string name;
		public uint lev;
		public uint prof;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			name = reader.Read_str();
			lev = reader.Read_uint();
			prof = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(prof);
		}
	}
	public class item_prop_list
	{
		public uint prop_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			prop_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(prop_id);
		}
	}
	public class mail_list
	{
		public uint mail_id;
		public string title;
		public uint resoure;
		public uint is_read;
		public uint s_time;
		public uint config_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			mail_id = reader.Read_uint();
			title = reader.Read_str();
			resoure = reader.Read_uint();
			is_read = reader.Read_uint();
			s_time = reader.Read_uint();
			config_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(mail_id);
			writer.write_str(title);
			writer.write_int(resoure);
			writer.write_int(is_read);
			writer.write_int(s_time);
			writer.write_int(config_id);
		}
	}
	public class title_obj
	{
		public ushort title;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			title = reader.Read_ushort();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_short(title);
		}
	}
	public class id_list
	{
		public uint id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
		}
	}
	public class guild_building_list
	{
		public uint building_id;
		public uint building_lev;
		public uint building_exp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			building_id = reader.Read_uint();
			building_lev = reader.Read_uint();
			building_exp = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(building_id);
			writer.write_int(building_lev);
			writer.write_int(building_exp);
		}
	}
	public class donation_record_list
	{
		public uint donation_time;
		public string donation_subscriber;
		public uint building_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			donation_time = reader.Read_uint();
			donation_subscriber = reader.Read_str();
			building_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(donation_time);
			writer.write_str(donation_subscriber);
			writer.write_int(building_num);
		}
	}
	public class update_mails
	{
		public uint mail_id;
		public List<st.net.NetBase.item_list> items = new List<st.net.NetBase.item_list>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			mail_id = reader.Read_uint();
			ushort lenitems = reader.Read_ushort();
			items = new List<st.net.NetBase.item_list>();
			for(int i_items = 0 ; i_items < lenitems ; i_items ++)
			{
				st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
				listData.fromBinary(reader);
				items.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(mail_id);
			ushort lenitems = (ushort)items.Count;
			writer.write_short(lenitems);
			for(int i_items = 0 ; i_items < lenitems ; i_items ++)
			{
				st.net.NetBase.item_list listData = items[i_items];
				listData.toBinary(writer);
			}
		}
	}
	public class monster_affiliation
	{
		public uint monster_id;
		public uint owner_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			monster_id = reader.Read_uint();
			owner_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(monster_id);
			writer.write_int(owner_id);
		}
	}
	public class guild_copy_list
	{
		public uint scene_id;
		public uint progress;
		public uint copy_open_state;
		public uint challenge_time;
		public uint remaining_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			scene_id = reader.Read_uint();
			progress = reader.Read_uint();
			copy_open_state = reader.Read_uint();
			challenge_time = reader.Read_uint();
			remaining_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(scene_id);
			writer.write_int(progress);
			writer.write_int(copy_open_state);
			writer.write_int(challenge_time);
			writer.write_int(remaining_time);
		}
	}
	public class guild_copy_trophy_list
	{
		public uint scene_id;
		public List<st.net.NetBase.guild_trophy_list> guild_trophy_list = new List<st.net.NetBase.guild_trophy_list>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			scene_id = reader.Read_uint();
			ushort lenguild_trophy_list = reader.Read_ushort();
			guild_trophy_list = new List<st.net.NetBase.guild_trophy_list>();
			for(int i_guild_trophy_list = 0 ; i_guild_trophy_list < lenguild_trophy_list ; i_guild_trophy_list ++)
			{
				st.net.NetBase.guild_trophy_list listData = new st.net.NetBase.guild_trophy_list();
				listData.fromBinary(reader);
				guild_trophy_list.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(scene_id);
			ushort lenguild_trophy_list = (ushort)guild_trophy_list.Count;
			writer.write_short(lenguild_trophy_list);
			for(int i_guild_trophy_list = 0 ; i_guild_trophy_list < lenguild_trophy_list ; i_guild_trophy_list ++)
			{
				st.net.NetBase.guild_trophy_list listData = guild_trophy_list[i_guild_trophy_list];
				listData.toBinary(writer);
			}
		}
	}
	public class guild_copy_enter_list
	{
		public uint scene_id;
		public uint enter_state;
		public string usr_name;
		public uint prof;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			scene_id = reader.Read_uint();
			enter_state = reader.Read_uint();
			usr_name = reader.Read_str();
			prof = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(scene_id);
			writer.write_int(enter_state);
			writer.write_str(usr_name);
			writer.write_int(prof);
		}
	}
	public class guild_copy_damage_ranking
	{
		public uint ranking;
		public string usr_name;
		public uint damage_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			ranking = reader.Read_uint();
			usr_name = reader.Read_str();
			damage_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(ranking);
			writer.write_str(usr_name);
			writer.write_int(damage_num);
		}
	}
	public class guild_trophy_list
	{
		public uint item_id;
		public uint apply_position;
		public uint item_num;
		public uint queued_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			item_id = reader.Read_uint();
			apply_position = reader.Read_uint();
			item_num = reader.Read_uint();
			queued_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(item_id);
			writer.write_int(apply_position);
			writer.write_int(item_num);
			writer.write_int(queued_num);
		}
	}
	public class scene_objs
	{
		public uint sort;
		public uint id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			sort = reader.Read_uint();
			id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(sort);
			writer.write_int(id);
		}
	}
	public class member_info
	{
		public uint id;
		public byte lev;
		public string name;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			lev = reader.Read_byte();
			name = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_byte(lev);
			writer.write_str(name);
		}
	}
	public class team_info
	{
		public uint tid;
		public uint leader_id;
		public byte max_plys;
		public uint fighting;
		public List<st.net.NetBase.member_info> members = new List<st.net.NetBase.member_info>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			tid = reader.Read_uint();
			leader_id = reader.Read_uint();
			max_plys = reader.Read_byte();
			fighting = reader.Read_uint();
			ushort lenmembers = reader.Read_ushort();
			members = new List<st.net.NetBase.member_info>();
			for(int i_members = 0 ; i_members < lenmembers ; i_members ++)
			{
				st.net.NetBase.member_info listData = new st.net.NetBase.member_info();
				listData.fromBinary(reader);
				members.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(tid);
			writer.write_int(leader_id);
			writer.write_byte(max_plys);
			writer.write_int(fighting);
			ushort lenmembers = (ushort)members.Count;
			writer.write_short(lenmembers);
			for(int i_members = 0 ; i_members < lenmembers ; i_members ++)
			{
				st.net.NetBase.member_info listData = members[i_members];
				listData.toBinary(writer);
			}
		}
	}
	public class ranklist
	{
		public uint uid;
		public uint rank;
		public string name;
		public uint lev;
		public uint camp;
		public uint prof;
		public uint fighting;
		public string guild_name;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			rank = reader.Read_uint();
			name = reader.Read_str();
			lev = reader.Read_uint();
			camp = reader.Read_uint();
			prof = reader.Read_uint();
			fighting = reader.Read_uint();
			guild_name = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(rank);
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(camp);
			writer.write_int(prof);
			writer.write_int(fighting);
			writer.write_str(guild_name);
		}
	}
	public class r_skin
	{
		public uint id;
		public uint type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			type = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
		}
	}
	public class prop_entry
	{
		public ushort prop_name;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			prop_name = reader.Read_ushort();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_short(prop_name);
			writer.write_int(num);
		}
	}
	public class rewards_receive_list
	{
		public uint rewards_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			rewards_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(rewards_id);
		}
	}
	public class match_succ_list
	{
		public string name;
		public uint lev;
		public uint prof;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			name = reader.Read_str();
			lev = reader.Read_uint();
			prof = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(prof);
		}
	}
	public class entourage_info_list
	{
		public uint etype;
		public uint lev;
		public uint estar;
		public uint estate;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			etype = reader.Read_uint();
			lev = reader.Read_uint();
			estar = reader.Read_uint();
			estate = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(etype);
			writer.write_int(lev);
			writer.write_int(estar);
			writer.write_int(estate);
		}
	}
	public class lost_item_info
	{
		public uint lost_item_type;
		public uint lost_item_lev;
		public uint lost_item_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			lost_item_type = reader.Read_uint();
			lost_item_lev = reader.Read_uint();
			lost_item_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(lost_item_type);
			writer.write_int(lost_item_lev);
			writer.write_int(lost_item_state);
		}
	}
	public class draw_times
	{
		public uint draw_type;
		public uint draw_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			draw_type = reader.Read_uint();
			draw_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(draw_type);
			writer.write_int(draw_time);
		}
	}
	public class draw_item_list
	{
		public uint draw_item_id;
		public uint draw_item_type;
		public uint draw_item_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			draw_item_id = reader.Read_uint();
			draw_item_type = reader.Read_uint();
			draw_item_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(draw_item_id);
			writer.write_int(draw_item_type);
			writer.write_int(draw_item_num);
		}
	}
	public class camp_vote_list
	{
		public uint rank;
		public uint uid;
		public uint military;
		public string name;
		public uint fighting;
		public uint vote_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			rank = reader.Read_uint();
			uid = reader.Read_uint();
			military = reader.Read_uint();
			name = reader.Read_str();
			fighting = reader.Read_uint();
			vote_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(rank);
			writer.write_int(uid);
			writer.write_int(military);
			writer.write_str(name);
			writer.write_int(fighting);
			writer.write_int(vote_num);
		}
	}
	public class trial_nums
	{
		public uint trialid;
		public byte num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			trialid = reader.Read_uint();
			num = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(trialid);
			writer.write_byte(num);
		}
	}
	public class camp_model_list
	{
		public uint pid;
		public string name;
		public uint prof;
		public float dir;
		public float x;
		public float y;
		public float z;
		public uint camp;
		public List<st.net.NetBase.id_list> equips = new List<st.net.NetBase.id_list>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			pid = reader.Read_uint();
			name = reader.Read_str();
			prof = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
			camp = reader.Read_uint();
			ushort lenequips = reader.Read_ushort();
			equips = new List<st.net.NetBase.id_list>();
			for(int i_equips = 0 ; i_equips < lenequips ; i_equips ++)
			{
				st.net.NetBase.id_list listData = new st.net.NetBase.id_list();
				listData.fromBinary(reader);
				equips.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(pid);
			writer.write_str(name);
			writer.write_int(prof);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
			writer.write_int(camp);
			ushort lenequips = (ushort)equips.Count;
			writer.write_short(lenequips);
			for(int i_equips = 0 ; i_equips < lenequips ; i_equips ++)
			{
				st.net.NetBase.id_list listData = equips[i_equips];
				listData.toBinary(writer);
			}
		}
	}
	public class scene_model
	{
		public uint id;
		public string name;
		public uint prof;
		public float dir;
		public float x;
		public float y;
		public float z;
		public uint camp;
		public List<uint> equip_list = new List<uint>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			name = reader.Read_str();
			prof = reader.Read_uint();
			dir = reader.Read_float();
			x = reader.Read_float();
			y = reader.Read_float();
			z = reader.Read_float();
			camp = reader.Read_uint();
			ushort lenequip_list = reader.Read_ushort();
			equip_list = new List<uint>();
			for(int i_equip_list = 0 ; i_equip_list < lenequip_list ; i_equip_list ++)
			{
				uint listData = reader.Read_uint();
				equip_list.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_str(name);
			writer.write_int(prof);
			writer.write_float(dir);
			writer.write_float(x);
			writer.write_float(y);
			writer.write_float(z);
			writer.write_int(camp);
			ushort lenequip_list = (ushort)equip_list.Count;
			writer.write_short(lenequip_list);
			for(int i_equip_list = 0 ; i_equip_list < lenequip_list ; i_equip_list ++)
			{
				uint listData = equip_list[i_equip_list];
				writer.write_int(listData);
			}
		}
	}
	public class entourage
	{
		public uint id;
		public uint hp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			hp = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(hp);
		}
	}
	public class resource_list
	{
		public uint resource_type;
		public uint resource_val;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			resource_type = reader.Read_uint();
			resource_val = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(resource_type);
			writer.write_int(resource_val);
		}
	}
	public class result_list
	{
		public uint type;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}
	public class change_skill_list
	{
		public int skill_1;
		public int skill_2;
		public int skill_3;
		public int skill_4;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			skill_1 = reader.Read_int();
			skill_2 = reader.Read_int();
			skill_3 = reader.Read_int();
			skill_4 = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(skill_1);
			writer.write_int(skill_2);
			writer.write_int(skill_3);
			writer.write_int(skill_4);
		}
	}
		public class magic_weapon_propertys
	{
		public uint type;
		public uint att;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			att = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(att);
		}
	}
		public class magic_weapon_zhuling_propertys
	{
		public uint type;
		public uint att;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			att = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(att);
		}
	}
		public class magic_weapons_state
	{
		public int id;
		public int type;
		public int active_state;
		public int equ_state;
		public int strenth_star;
		public int strenth_exp;
		public int strenth_lev;
		public int zhuling_star;
		public int zhuling_lev;
		public int zhuling_exp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			type = reader.Read_int();
			active_state = reader.Read_int();
			equ_state = reader.Read_int();
			strenth_star = reader.Read_int();
			strenth_exp = reader.Read_int();
			strenth_lev = reader.Read_int();
			zhuling_star = reader.Read_int();
			zhuling_lev = reader.Read_int();
			zhuling_exp = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(active_state);
			writer.write_int(equ_state);
			writer.write_int(strenth_star);
			writer.write_int(strenth_exp);
			writer.write_int(strenth_lev);
			writer.write_int(zhuling_star);
			writer.write_int(zhuling_lev);
			writer.write_int(zhuling_exp);
		}
	}
public class pet_base_info
	{
		public uint id;
		public uint type;
		public uint status;
		public uint fight_score;
		public uint aptitude;
		public uint grow_up;
		public uint lev;
		public string pet_name;
		public List<st.net.NetBase.pet_property_list> all_property = new List<st.net.NetBase.pet_property_list>();
		public List<st.net.NetBase.pet_property_list> grow_up_property = new List<st.net.NetBase.pet_property_list>();
		public List<st.net.NetBase.pet_property_list> jinghun_property = new List<st.net.NetBase.pet_property_list>();
		public uint tian_soul;
		public uint di_soul;
		public uint life_soul;
		public List<uint> pet_skill = new List<uint>();
		public uint grow_up_exp;
		public uint aptitude_exp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			type = reader.Read_uint();
			status = reader.Read_uint();
			fight_score = reader.Read_uint();
			aptitude = reader.Read_uint();
			grow_up = reader.Read_uint();
			lev = reader.Read_uint();
			pet_name = reader.Read_str();
			ushort lenall_property = reader.Read_ushort();
			all_property = new List<st.net.NetBase.pet_property_list>();
			for(int i_all_property = 0 ; i_all_property < lenall_property ; i_all_property ++)
			{
				st.net.NetBase.pet_property_list listData = new st.net.NetBase.pet_property_list();
				listData.fromBinary(reader);
				all_property.Add(listData);
			}
			ushort lengrow_up_property = reader.Read_ushort();
			grow_up_property = new List<st.net.NetBase.pet_property_list>();
			for(int i_grow_up_property = 0 ; i_grow_up_property < lengrow_up_property ; i_grow_up_property ++)
			{
				st.net.NetBase.pet_property_list listData = new st.net.NetBase.pet_property_list();
				listData.fromBinary(reader);
				grow_up_property.Add(listData);
			}
			ushort lenjinghun_property = reader.Read_ushort();
			jinghun_property = new List<st.net.NetBase.pet_property_list>();
			for(int i_jinghun_property = 0 ; i_jinghun_property < lenjinghun_property ; i_jinghun_property ++)
			{
				st.net.NetBase.pet_property_list listData = new st.net.NetBase.pet_property_list();
				listData.fromBinary(reader);
				jinghun_property.Add(listData);
			}
			tian_soul = reader.Read_uint();
			di_soul = reader.Read_uint();
			life_soul = reader.Read_uint();
			ushort lenpet_skill = reader.Read_ushort();
			pet_skill = new List<uint>();
			for(int i_pet_skill = 0 ; i_pet_skill < lenpet_skill ; i_pet_skill ++)
			{
				uint listData = reader.Read_uint();
				pet_skill.Add(listData);
			}
			grow_up_exp = reader.Read_uint();
			aptitude_exp = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(status);
			writer.write_int(fight_score);
			writer.write_int(aptitude);
			writer.write_int(grow_up);
			writer.write_int(lev);
			writer.write_str(pet_name);
			ushort lenall_property = (ushort)all_property.Count;
			writer.write_short(lenall_property);
			for(int i_all_property = 0 ; i_all_property < lenall_property ; i_all_property ++)
			{
				st.net.NetBase.pet_property_list listData = all_property[i_all_property];
				listData.toBinary(writer);
			}
			ushort lengrow_up_property = (ushort)grow_up_property.Count;
			writer.write_short(lengrow_up_property);
			for(int i_grow_up_property = 0 ; i_grow_up_property < lengrow_up_property ; i_grow_up_property ++)
			{
				st.net.NetBase.pet_property_list listData = grow_up_property[i_grow_up_property];
				listData.toBinary(writer);
			}
			ushort lenjinghun_property = (ushort)jinghun_property.Count;
			writer.write_short(lenjinghun_property);
			for(int i_jinghun_property = 0 ; i_jinghun_property < lenjinghun_property ; i_jinghun_property ++)
			{
				st.net.NetBase.pet_property_list listData = jinghun_property[i_jinghun_property];
				listData.toBinary(writer);
			}
			writer.write_int(tian_soul);
			writer.write_int(di_soul);
			writer.write_int(life_soul);
			ushort lenpet_skill = (ushort)pet_skill.Count;
			writer.write_short(lenpet_skill);
			for(int i_pet_skill = 0 ; i_pet_skill < lenpet_skill ; i_pet_skill ++)
			{
				uint listData = pet_skill[i_pet_skill];
				writer.write_int(listData);
			}
			writer.write_int(grow_up_exp);
			writer.write_int(aptitude_exp);
		}
	}
	
	
public class pet_property_list
	{
		public int type;
		public int num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}	

	public class pet_skills
	{
		public uint pet_skill_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			pet_skill_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(pet_skill_id);
		}
	}
	public class skill_compound_item
	{
		public uint compound_item_type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			compound_item_type = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(compound_item_type);
		}
	}
	public class model_clothes_list
	{
		public int model_id;
		public int own_state;
		public int put_state;
		public int remain_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			model_id = reader.Read_int();
			own_state = reader.Read_int();
			put_state = reader.Read_int();
			remain_time = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(model_id);
			writer.write_int(own_state);
			writer.write_int(put_state);
			writer.write_int(remain_time);
		}
	}
	public class wing_base_info
	{
		public uint wing_id;
		public uint lev;
		public uint exp;
		public uint passivity_skill;
		public uint put_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			wing_id = reader.Read_uint();
			lev = reader.Read_uint();
			exp = reader.Read_uint();
			passivity_skill = reader.Read_uint();
			put_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(wing_id);
			writer.write_int(lev);
			writer.write_int(exp);
			writer.write_int(passivity_skill);
			writer.write_int(put_state);
		}
	}
	
	public class title_base_info_list
	{
		public uint title_id;
		public uint own_state;
		public uint put_state;
		public uint time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			title_id = reader.Read_uint();
			own_state = reader.Read_uint();
			put_state = reader.Read_uint();
			time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(title_id);
			writer.write_int(own_state);
			writer.write_int(put_state);
			writer.write_int(time);
		}
	}

	
public class skin_base_info
	{
		public uint skin_id;
		public uint skin_state;
		public uint remain_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			skin_id = reader.Read_uint();
			skin_state = reader.Read_uint();
			remain_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(skin_id);
			writer.write_int(skin_state);
			writer.write_int(remain_time);
		}
	}
	public class base_mail_list
	{
		public int id;
		public int type;
		public int read_state;
		public string title;
		public string send_name;
		public int send_time;
		public int expire_time;
		public int content_type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			type = reader.Read_int();
			read_state = reader.Read_int();
			title = reader.Read_str();
			send_name = reader.Read_str();
			send_time = reader.Read_int();
			expire_time = reader.Read_int();
			content_type = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(read_state);
			writer.write_str(title);
			writer.write_str(send_name);
			writer.write_int(send_time);
			writer.write_int(expire_time);
			writer.write_int(content_type);
		}
	}
	public class mail_info_list
	{
		public int id;
		public int type;
		public int read_state;
		public string title;
		public string content;
		public List<st.net.NetBase.item_des> items = new List<st.net.NetBase.item_des>();
		public string send_name;
		public int send_time;
		public int expire_time;
		public int content_type;
		public List<st.net.NetBase.mail_item_args> system_mail_args = new List<st.net.NetBase.mail_item_args>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			type = reader.Read_int();
			read_state = reader.Read_int();
			title = reader.Read_str();
			content = reader.Read_str();
			ushort lenitems = reader.Read_ushort();
			items = new List<st.net.NetBase.item_des>();
			for(int i_items = 0 ; i_items < lenitems ; i_items ++)
			{
				st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
				listData.fromBinary(reader);
				items.Add(listData);
			}
			send_name = reader.Read_str();
			send_time = reader.Read_int();
			expire_time = reader.Read_int();
			content_type = reader.Read_int();
			ushort lensystem_mail_args = reader.Read_ushort();
			system_mail_args = new List<st.net.NetBase.mail_item_args>();
			for(int i_system_mail_args = 0 ; i_system_mail_args < lensystem_mail_args ; i_system_mail_args ++)
			{
				st.net.NetBase.mail_item_args listData = new st.net.NetBase.mail_item_args();
				listData.fromBinary(reader);
				system_mail_args.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(read_state);
			writer.write_str(title);
			writer.write_str(content);
			ushort lenitems = (ushort)items.Count;
			writer.write_short(lenitems);
			for(int i_items = 0 ; i_items < lenitems ; i_items ++)
			{
				st.net.NetBase.item_des listData = items[i_items];
				listData.toBinary(writer);
			}
			writer.write_str(send_name);
			writer.write_int(send_time);
			writer.write_int(expire_time);
			writer.write_int(content_type);
			ushort lensystem_mail_args = (ushort)system_mail_args.Count;
			writer.write_short(lensystem_mail_args);
			for(int i_system_mail_args = 0 ; i_system_mail_args < lensystem_mail_args ; i_system_mail_args ++)
			{
				st.net.NetBase.mail_item_args listData = system_mail_args[i_system_mail_args];
				listData.toBinary(writer);
			}
		}
	}
	public class star_id_list
	{
		public uint star_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			star_id = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(star_id);
		}
	}

	public class star_list
	{
		public uint star_id;
		public uint get_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			star_id = reader.Read_uint();
			get_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(star_id);
			writer.write_int(get_state);
		}
	}
public class endless_list
	{
		public uint chpter_id;
		public List<st.net.NetBase.pass_list> pass_list = new List<st.net.NetBase.pass_list>();
		public List<st.net.NetBase.pass_star_list> pass_star_list = new List<st.net.NetBase.pass_star_list>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			chpter_id = reader.Read_uint();
			ushort lenpass_list = reader.Read_ushort();
			pass_list = new List<st.net.NetBase.pass_list>();
			for(int i_pass_list = 0 ; i_pass_list < lenpass_list ; i_pass_list ++)
			{
				st.net.NetBase.pass_list listData = new st.net.NetBase.pass_list();
				listData.fromBinary(reader);
				pass_list.Add(listData);
			}
			ushort lenpass_star_list = reader.Read_ushort();
			pass_star_list = new List<st.net.NetBase.pass_star_list>();
			for(int i_pass_star_list = 0 ; i_pass_star_list < lenpass_star_list ; i_pass_star_list ++)
			{
				st.net.NetBase.pass_star_list listData = new st.net.NetBase.pass_star_list();
				listData.fromBinary(reader);
				pass_star_list.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(chpter_id);
			ushort lenpass_list = (ushort)pass_list.Count;
			writer.write_short(lenpass_list);
			for(int i_pass_list = 0 ; i_pass_list < lenpass_list ; i_pass_list ++)
			{
				st.net.NetBase.pass_list listData = pass_list[i_pass_list];
				listData.toBinary(writer);
			}
			ushort lenpass_star_list = (ushort)pass_star_list.Count;
			writer.write_short(lenpass_star_list);
			for(int i_pass_star_list = 0 ; i_pass_star_list < lenpass_star_list ; i_pass_star_list ++)
			{
				st.net.NetBase.pass_star_list listData = pass_star_list[i_pass_star_list];
				listData.toBinary(writer);
			}
		}
	}
	public class pass_list
	{
		public uint pass_id;
		public uint pass_state;
		public uint star_num;
		public uint pass_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			pass_id = reader.Read_uint();
			pass_state = reader.Read_uint();
			star_num = reader.Read_uint();
			pass_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(pass_id);
			writer.write_int(pass_state);
			writer.write_int(star_num);
			writer.write_int(pass_time);
		}
	}
	public class pass_star_list
	{
		public uint star_id;
		public uint star_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			star_id = reader.Read_uint();
			star_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(star_id);
			writer.write_int(star_state);
		}
	}
		public class pos_des
	{
		public int pos;
		public int type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			pos = reader.Read_int();
			type = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(pos);
			writer.write_int(type);
		}
	}
		public class base_list
	{
		public uint id;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(num);
		}
	}
	public class advanced_list
	{
		public uint id;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(num);
		}
	}
		public class spare_list
	{
		public int pos;
		public int att_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			pos = reader.Read_int();
			att_id = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(pos);
			writer.write_int(att_id);
		}
	}
public class copy_sweep_list
	{
		public uint type;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}

	
	public class single_many_list
	{
		public uint copy_id;
		public uint challenge_num;
		public uint buy_num;
		public List<st.net.NetBase.single_many_star> single_many_star = new List<st.net.NetBase.single_many_star>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			copy_id = reader.Read_uint();
			challenge_num = reader.Read_uint();
			buy_num = reader.Read_uint();
			ushort lensingle_many_star = reader.Read_ushort();
			single_many_star = new List<st.net.NetBase.single_many_star>();
			for(int i_single_many_star = 0 ; i_single_many_star < lensingle_many_star ; i_single_many_star ++)
			{
				st.net.NetBase.single_many_star listData = new st.net.NetBase.single_many_star();
				listData.fromBinary(reader);
				single_many_star.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(copy_id);
			writer.write_int(challenge_num);
			writer.write_int(buy_num);
			ushort lensingle_many_star = (ushort)single_many_star.Count;
			writer.write_short(lensingle_many_star);
			for(int i_single_many_star = 0 ; i_single_many_star < lensingle_many_star ; i_single_many_star ++)
			{
				st.net.NetBase.single_many_star listData = single_many_star[i_single_many_star];
				listData.toBinary(writer);
			}
		}
	}
	public class single_many_star
	{
		public uint copy_type;
		public uint star_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			copy_type = reader.Read_uint();
			star_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(copy_type);
			writer.write_int(star_num);
		}
	}
		public class shop_list
	{
		public uint id;
		public uint num;
		public uint from;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			num = reader.Read_uint();
			from = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(num);
			writer.write_int(from);
		}
	}
	
	public class member_challengenum_list
	{
		public uint uid;
		public uint challenge_num;
		public uint prepare;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			challenge_num = reader.Read_uint();
			prepare = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(challenge_num);
			writer.write_int(prepare);
		}
	}
	public class update_prepare
	{
		public uint uid;
		public uint prepare;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			prepare = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_int(prepare);
		}
	}
	public class reward_list
	{
		public uint type;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}
	public class lucky_brand_list
	{
		public uint type;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}
	public class brand_reward
	{
		public uint type;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}	
		public class treasure_list
	{
		public uint type;
		public uint num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}
	public class guild_member_info
	{
		public int uid;
		public string name;
		public int lev;
		public int fight_score;
		public int tribute;
		public int all_tribute;
		public int position;
		public int state;
		public int last_login_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			lev = reader.Read_int();
			fight_score = reader.Read_int();
			tribute = reader.Read_int();
			all_tribute = reader.Read_int();
			position = reader.Read_int();
			state = reader.Read_int();
			last_login_time = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(fight_score);
			writer.write_int(tribute);
			writer.write_int(all_tribute);
			writer.write_int(position);
			writer.write_int(state);
			writer.write_int(last_login_time);
		}
	}
public class guild_log
	{
		public uint event_id;
		public uint time;
		public string guild_log_args;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			event_id = reader.Read_uint();
			time = reader.Read_uint();
			guild_log_args = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(event_id);
			writer.write_int(time);
			writer.write_str(guild_log_args);
		}
	}
	public class guild_list_info
	{
		public int id;
		public string name;
		public int rank;
		public int lev;
		public string leader;
		public int cur_member;
		public int all_member;
		public int fight_score;
		public int ask_join_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			name = reader.Read_str();
			rank = reader.Read_int();
			lev = reader.Read_int();
			leader = reader.Read_str();
			cur_member = reader.Read_int();
			all_member = reader.Read_int();
			fight_score = reader.Read_int();
			ask_join_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_str(name);
			writer.write_int(rank);
			writer.write_int(lev);
			writer.write_str(leader);
			writer.write_int(cur_member);
			writer.write_int(all_member);
			writer.write_int(fight_score);
			writer.write_int(ask_join_state);
		}
	}
	public class ask_join_guild_list
	{
		public int uid;
		public string name;
		public int lev;
		public int fight_score;
		public int state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			lev = reader.Read_int();
			fight_score = reader.Read_int();
			state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(fight_score);
			writer.write_int(state);
		}
	}


	public class treasure_record_list
	{
		public int uid;
		public string name;
		public int type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			type = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(type);
		}
	}
	public class robot_list
	{
		public uint uid;
		public string name;
		public uint battle;
		public uint rank;
		public uint prof;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_uint();
			name = reader.Read_str();
			battle = reader.Read_uint();
			rank = reader.Read_uint();
			prof = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(battle);
			writer.write_int(rank);
			writer.write_int(prof);
		}
	}
	public class log_list
	{
		public uint log_id;
		public string arg;
		public uint challenge_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			log_id = reader.Read_uint();
			arg = reader.Read_str();
			challenge_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(log_id);
			writer.write_str(arg);
			writer.write_int(challenge_time);
		}
	}	
	public class guild_check_out_item_ask_list
	{
		public int uid;
		public string name;
		public int lev;
		public int all_contribute;
		public int item_type;
		public int item_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			lev = reader.Read_int();
			all_contribute = reader.Read_int();
			item_type = reader.Read_int();
			item_id = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(all_contribute);
			writer.write_int(item_type);
			writer.write_int(item_id);
		}
	}
		public class guild_items_log_list
	{
		public int uid;
		public string name;
		public int action;
		public int item_type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			action = reader.Read_int();
			item_type = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(action);
			writer.write_int(item_type);
		}
	}
	public class rank_info_base
	{
		public uint id;
		public string name;
		public int value1;
		public int value2;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			name = reader.Read_str();
			value1 = reader.Read_int();
			value2 = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_str(name);
			writer.write_int(value1);
			writer.write_int(value2);
		}
	}
	public class boss_count
	{
		public int monster_type;
		public int cur_num;
		public int amount;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			monster_type = reader.Read_int();
			cur_num = reader.Read_int();
			amount = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(monster_type);
			writer.write_int(cur_num);
			writer.write_int(amount);
		}
	}

	public class boss_challenge
	{
		public int boss_id;
		public int kill_state;
		public int surplus_time;
		public string kill_name;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			boss_id = reader.Read_int();
			kill_state = reader.Read_int();
			surplus_time = reader.Read_int();
			kill_name = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(boss_id);
			writer.write_int(kill_state);
			writer.write_int(surplus_time);
			writer.write_str(kill_name);
		}
	}
		public class brothers_list
	{
		public int uid;
		public string name;
		public int lev;
		public int friend_ship;
		public int prof;
		public int oline_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			lev = reader.Read_int();
			friend_ship = reader.Read_int();
			prof = reader.Read_int();
			oline_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(lev);
			writer.write_int(friend_ship);
			writer.write_int(prof);
			writer.write_int(oline_state);
		}
	}
	
	public class relation_list
	{
		public int type;
		public int uid;
		public string name;
		public int prof;
		public int lev;
		public int intimacy;
		public int offline_state;
		public int scene;
		public int fight_score;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			uid = reader.Read_int();
			name = reader.Read_str();
			prof = reader.Read_int();
			lev = reader.Read_int();
			intimacy = reader.Read_int();
			offline_state = reader.Read_int();
			scene = reader.Read_int();
			fight_score = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(prof);
			writer.write_int(lev);
			writer.write_int(intimacy);
			writer.write_int(offline_state);
			writer.write_int(scene);
			writer.write_int(fight_score);
		}
	}
	public class brother_reward_info
	{
		public int lev;
		public int state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			lev = reader.Read_int();
			state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(lev);
			writer.write_int(state);
		}
	}
	
	public class activity_list
	{
		public int id;
		public int challenge_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			challenge_num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(challenge_num);
		}
	}
	
	public class shelve_item_info
	{
		public uint id;
		public uint type;
		public uint num;
		public byte color;
		public uint price;
		public uint rest_time;
		public byte currency;
		public List<st.net.NetBase.item_des> item_info = new List<st.net.NetBase.item_des>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			type = reader.Read_uint();
			num = reader.Read_uint();
			color = reader.Read_byte();
			price = reader.Read_uint();
			rest_time = reader.Read_uint();
			currency = reader.Read_byte();
			ushort lenitem_info = reader.Read_ushort();
			item_info = new List<st.net.NetBase.item_des>();
			for(int i_item_info = 0 ; i_item_info < lenitem_info ; i_item_info ++)
			{
				st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
				listData.fromBinary(reader);
				item_info.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(num);
			writer.write_byte(color);
			writer.write_int(price);
			writer.write_int(rest_time);
			writer.write_byte(currency);
			ushort lenitem_info = (ushort)item_info.Count;
			writer.write_short(lenitem_info);
			for(int i_item_info = 0 ; i_item_info < lenitem_info ; i_item_info ++)
			{
				st.net.NetBase.item_des listData = item_info[i_item_info];
				listData.toBinary(writer);
			}
		}
	}
	public class budo_log_list
	{
		public int log_id;
		public string arg;
		public int rand_id;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			log_id = reader.Read_int();
			arg = reader.Read_str();
			rand_id = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(log_id);
			writer.write_str(arg);
			writer.write_int(rand_id);
		}
	}
		public class mail_item_args
	{
		public int type;
		public int num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}
		public class guild_battle_integer_list
	{
		public int uid;
		public string name;
		public int integer;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			integer = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(integer);
		}
	}
	
	public class guild_guard_rank
	{
		public int uid;
		public string name;
		public int damage;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			damage = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(damage);
		}
	}	
			public class guild_battle_group_info_list
	{
		public string name;
		public int state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			name = reader.Read_str();
			state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_str(name);
			writer.write_int(state);
		}
	}
public class req_apply_list
	{
		public int rank;
		public string guild_name;
		public int guild_lev;
		public string leader_name;
		public int member_num;
		public int guild_fight;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			rank = reader.Read_int();
			guild_name = reader.Read_str();
			guild_lev = reader.Read_int();
			leader_name = reader.Read_str();
			member_num = reader.Read_int();
			guild_fight = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(rank);
			writer.write_str(guild_name);
			writer.write_int(guild_lev);
			writer.write_str(leader_name);
			writer.write_int(member_num);
			writer.write_int(guild_fight);
		}
	}
	public class astrict_item_list
	{
		public int id;
		public int astrict_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			astrict_num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(astrict_num);
		}
	}	
	public class other_bonfire_list
	{
		public int rank;
		public int guild_id;
		public string guild_name;
		public int guild_lev;
		public int guild_fight;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			rank = reader.Read_int();
			guild_id = reader.Read_int();
			guild_name = reader.Read_str();
			guild_lev = reader.Read_int();
			guild_fight = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(rank);
			writer.write_int(guild_id);
			writer.write_str(guild_name);
			writer.write_int(guild_lev);
			writer.write_int(guild_fight);
		}
	}
	public class ride_list
	{
		public int ride_id;
		public int ride_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			ride_id = reader.Read_int();
			ride_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(ride_id);
			writer.write_int(ride_state);
		}
	}

	public class recruit_robot_list
	{
		public int uid;
		public string name;
		public int prof;
		public int lev;
		public int fight_score;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			prof = reader.Read_int();
			lev = reader.Read_int();
			fight_score = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(prof);
			writer.write_int(lev);
			writer.write_int(fight_score);
		}
	}
	public class rand_box_reward
	{
		public int id;
		public int type;
		public int num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			type = reader.Read_int();
			num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_int(num);
		}
	}
	public class trade_item_info
	{
		public uint id;
		public uint amount;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			amount = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(amount);
		}
	}
	public class sevenDayReward
	{
		public uint type;
		public byte normal;
		public byte vip;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			normal = reader.Read_byte();
			vip = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_byte(normal);
			writer.write_byte(vip);
		}
	}
	public class call_boss_list
	{
		public int boss_id;
		public byte call_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			boss_id = reader.Read_int();
			call_state = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(boss_id);
			writer.write_byte(call_state);
		}
	}
	public class achievement_reach
	{
		public int type;
		public int amount;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			amount = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(amount);
		}
	}
	public class achievement_reward
	{
		public int id;
		public int num;
		public int state;
		public int new_or_old;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			num = reader.Read_int();
			state = reader.Read_int();
			new_or_old = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(num);
			writer.write_int(state);
			writer.write_int(new_or_old);
		}
	}
	public class operation_activity_title_info
	{
		public uint id;
		public byte type;
		public string name;
		public byte reward_flag;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			type = reader.Read_byte();
			name = reader.Read_str();
			reward_flag = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_byte(type);
			writer.write_str(name);
			writer.write_byte(reward_flag);
		}
	}
	public class operation_activity_detail_info
	{
		public byte index;
		public string desc;
		public uint value1;
		public uint value2;
		public string svalue1;
		public string svalue2;
		public uint reward_times;
		public uint total_reward_times;
		public List<st.net.NetBase.item_list> reward_info = new List<st.net.NetBase.item_list>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			index = reader.Read_byte();
			desc = reader.Read_str();
			value1 = reader.Read_uint();
			value2 = reader.Read_uint();
			svalue1 = reader.Read_str();
			svalue2 = reader.Read_str();
			reward_times = reader.Read_uint();
			total_reward_times = reader.Read_uint();
			ushort lenreward_info = reader.Read_ushort();
			reward_info = new List<st.net.NetBase.item_list>();
			for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
			{
				st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
				listData.fromBinary(reader);
				reward_info.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_byte(index);
			writer.write_str(desc);
			writer.write_int(value1);
			writer.write_int(value2);
			writer.write_str(svalue1);
			writer.write_str(svalue2);
			writer.write_int(reward_times);
			writer.write_int(total_reward_times);
			ushort lenreward_info = (ushort)reward_info.Count;
			writer.write_short(lenreward_info);
			for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
			{
				st.net.NetBase.item_list listData = reward_info[i_reward_info];
				listData.toBinary(writer);
			}
		}
	}
	public class liveness_info
	{
		public int id;
		public int condtion_num;
		public int reward_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_int();
			condtion_num = reader.Read_int();
			reward_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(condtion_num);
			writer.write_int(reward_state);
		}
	}
	
	public class open_server_gift_ware_info
	{
		public uint id;
		public string name;
		public uint item_type;
		public uint orig_price;
		public uint cur_price;
		public uint max_buy_amount;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			name = reader.Read_str();
			item_type = reader.Read_uint();
			orig_price = reader.Read_uint();
			cur_price = reader.Read_uint();
			max_buy_amount = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_str(name);
			writer.write_int(item_type);
			writer.write_int(orig_price);
			writer.write_int(cur_price);
			writer.write_int(max_buy_amount);
		}
	}
	
	public class weekcard_info
	{
		public uint type;
		public byte status;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_uint();
			status = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_byte(status);
		}
	}
	public class sworn_task_finish_info
	{
		public int uid;
		public List<st.net.NetBase.sworn_task> sworn_task_list = new List<st.net.NetBase.sworn_task>();
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			ushort lensworn_task_list = reader.Read_ushort();
			sworn_task_list = new List<st.net.NetBase.sworn_task>();
			for(int i_sworn_task_list = 0 ; i_sworn_task_list < lensworn_task_list ; i_sworn_task_list ++)
			{
				st.net.NetBase.sworn_task listData = new st.net.NetBase.sworn_task();
				listData.fromBinary(reader);
				sworn_task_list.Add(listData);
			}
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			ushort lensworn_task_list = (ushort)sworn_task_list.Count;
			writer.write_short(lensworn_task_list);
			for(int i_sworn_task_list = 0 ; i_sworn_task_list < lensworn_task_list ; i_sworn_task_list ++)
			{
				st.net.NetBase.sworn_task listData = sworn_task_list[i_sworn_task_list];
				listData.toBinary(writer);
			}
		}
	}
	public class sworn_task
	{
		public int task_id;
		public int task_step;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			task_id = reader.Read_int();
			task_step = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(task_id);
			writer.write_int(task_step);
		}
	}
	public class love_reward_list
	{
		public int type;
		public int num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}
     public class red_dot_list
	{
		public int type;
		public int state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(state);
		}
	}
		public class jewelry_list
	{
		public int jewelry_id;
		public int num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			jewelry_id = reader.Read_int();
			num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(jewelry_id);
			writer.write_int(num);
		}
	}
		public class liveness_reward_list
	{
		public int reward_id;
		public int state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			reward_id = reader.Read_int();
			state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(reward_id);
			writer.write_int(state);
		}
	}
	public class royal_box_info
	{
		public uint id;
		public uint type;
		public byte active;
		public uint rest_time;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			type = reader.Read_uint();
			active = reader.Read_byte();
			rest_time = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(type);
			writer.write_byte(active);
			writer.write_int(rest_time);
		}
	}
public class mountain_flames_rank
	{
		public int uid;
		public string name;
		public int score;
		public int camp;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			score = reader.Read_int();
			camp = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(score);
			writer.write_int(camp);
		}
	}
	public class mountain_amount_score
	{
		public int camp;
		public int amount_score;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			camp = reader.Read_int();
			amount_score = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(camp);
			writer.write_int(amount_score);
		}
	}
	public class mountain_flames_win
	{
		public int uid;
		public string name;
		public int damage;
		public int kill_num;
		public int amount_score;
		public int grade_lev;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			uid = reader.Read_int();
			name = reader.Read_str();
			damage = reader.Read_int();
			kill_num = reader.Read_int();
			amount_score = reader.Read_int();
			grade_lev = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(uid);
			writer.write_str(name);
			writer.write_int(damage);
			writer.write_int(kill_num);
			writer.write_int(amount_score);
			writer.write_int(grade_lev);
		}
	}
	public class general_list
	{
		public int general_type;
		public int battle_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			general_type = reader.Read_int();
			battle_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(general_type);
			writer.write_int(battle_state);
		}
	}
	public class lucky_wheel_reward_info
	{
		public uint id;
		public uint item_type;
		public uint amount;
		public byte wheel_type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			item_type = reader.Read_uint();
			amount = reader.Read_uint();
			wheel_type = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_int(item_type);
			writer.write_int(amount);
			writer.write_byte(wheel_type);
		}
	}
	public class lucky_wheel_record
	{
		public string name;
		public uint item_type;
		public uint amount;
		public byte wheel_type;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			name = reader.Read_str();
			item_type = reader.Read_uint();
			amount = reader.Read_uint();
			wheel_type = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_str(name);
			writer.write_int(item_type);
			writer.write_int(amount);
			writer.write_byte(wheel_type);
		}
	}
	public class treasure_rank_reward
	{
		public List<st.net.NetBase.item_list> reward_info = new List<st.net.NetBase.item_list>();
		public string desc;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			ushort lenreward_info = reader.Read_ushort();
			reward_info = new List<st.net.NetBase.item_list>();
			for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
			{
				st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
				listData.fromBinary(reader);
				reward_info.Add(listData);
			}
			desc = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			ushort lenreward_info = (ushort)reward_info.Count;
			writer.write_short(lenreward_info);
			for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
			{
				st.net.NetBase.item_list listData = reward_info[i_reward_info];
				listData.toBinary(writer);
			}
			writer.write_str(desc);
		}
	}
	public class treasure_times_reward
	{
		public uint id;
		public byte status;
		public uint need_times;
		public List<st.net.NetBase.item_list> reward_info = new List<st.net.NetBase.item_list>();
		public string desc;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			id = reader.Read_uint();
			status = reader.Read_byte();
			need_times = reader.Read_uint();
			ushort lenreward_info = reader.Read_ushort();
			reward_info = new List<st.net.NetBase.item_list>();
			for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
			{
				st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
				listData.fromBinary(reader);
				reward_info.Add(listData);
			}
			desc = reader.Read_str();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(id);
			writer.write_byte(status);
			writer.write_int(need_times);
			ushort lenreward_info = (ushort)reward_info.Count;
			writer.write_short(lenreward_info);
			for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
			{
				st.net.NetBase.item_list listData = reward_info[i_reward_info];
				listData.toBinary(writer);
			}
			writer.write_str(desc);
		}
	}
		public class seven_day_target_list
	{
		public uint day_id;
		public byte finish_num;
		public uint reward_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			day_id = reader.Read_uint();
			finish_num = reader.Read_byte();
			reward_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(day_id);
			writer.write_byte(finish_num);
			writer.write_int(reward_state);
		}
	}
	public class single_day_info
	{
		public uint task_id;
		public int task_num;
		public uint finish_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			task_id = reader.Read_uint();
			task_num = reader.Read_int();
			finish_state = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(task_id);
			writer.write_int(task_num);
			writer.write_int(finish_state);
		}
	}
	public class boss_copy_list
	{
		public uint boss_id;
		public int boss_kill_state;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			boss_id = reader.Read_uint();
			boss_kill_state = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(boss_id);
			writer.write_int(boss_kill_state);
		}
	}
	public class guild_liveness_member_info
	{
		public string name;
		public uint liveness;
		public byte is_online;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			name = reader.Read_str();
			liveness = reader.Read_uint();
			is_online = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_str(name);
			writer.write_int(liveness);
			writer.write_byte(is_online);
		}
	}
	public class guild_liveness_task_info
	{
		public uint task_id;
		public uint counter;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			task_id = reader.Read_uint();
			counter = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(task_id);
			writer.write_int(counter);
		}
	}
	public class task_surround_info
	{
		public uint task_sort;
		public uint finish_num;
		public uint surplus_refresh_num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			task_sort = reader.Read_uint();
			finish_num = reader.Read_uint();
			surplus_refresh_num = reader.Read_uint();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(task_sort);
			writer.write_int(finish_num);
			writer.write_int(surplus_refresh_num);
		}
	}
	public class recharge_flag_info
	{
		public byte type;
		public byte flag;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_byte();
			flag = reader.Read_byte();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_byte(type);
			writer.write_byte(flag);
		}
	}
	public class team_reward_list
	{
		public int type;
		public int num;
		public void fromBinary(st.net.NetBase.ByteReader reader)
		{
			type = reader.Read_int();
			num = reader.Read_int();
		}
		public void toBinary(st.net.NetBase.ByteWriter writer)
		{
			writer.write_int(type);
			writer.write_int(num);
		}
	}	
}
