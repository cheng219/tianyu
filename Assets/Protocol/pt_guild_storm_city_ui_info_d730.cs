using System.Collections;
using System.Collections.Generic;

public class pt_guild_storm_city_ui_info_d730 : st.net.NetBase.Pt {
	public pt_guild_storm_city_ui_info_d730()
	{
		Id = 0xD730;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_storm_city_ui_info_d730();
	}
	public string guild_name;
	public string castellan;
	public int start_state;
	public int apply_state;
	public int prof;
	public List<int> model_id = new List<int>();
	public int magic_weapon_id;
	public int magic_strength_lev;
	public int magic_strength_star;
	public int wing_id;
	public int wing_lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guild_name = reader.Read_str();
		castellan = reader.Read_str();
		start_state = reader.Read_int();
		apply_state = reader.Read_int();
		prof = reader.Read_int();
		ushort lenmodel_id = reader.Read_ushort();
		model_id = new List<int>();
		for(int i_model_id = 0 ; i_model_id < lenmodel_id ; i_model_id ++)
		{
			int listData = reader.Read_int();
			model_id.Add(listData);
		}
		magic_weapon_id = reader.Read_int();
		magic_strength_lev = reader.Read_int();
		magic_strength_star = reader.Read_int();
		wing_id = reader.Read_int();
		wing_lev = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(guild_name);
		writer.write_str(castellan);
		writer.write_int(start_state);
		writer.write_int(apply_state);
		writer.write_int(prof);
		ushort lenmodel_id = (ushort)model_id.Count;
		writer.write_short(lenmodel_id);
		for(int i_model_id = 0 ; i_model_id < lenmodel_id ; i_model_id ++)
		{
			int listData = model_id[i_model_id];
			writer.write_int(listData);
		}
		writer.write_int(magic_weapon_id);
		writer.write_int(magic_strength_lev);
		writer.write_int(magic_strength_star);
		writer.write_int(wing_id);
		writer.write_int(wing_lev);
		return writer.data;
	}

}
