using System.Collections;
using System.Collections.Generic;

public class pt_guild_battle_info_ex_d557 : st.net.NetBase.Pt {
	public pt_guild_battle_info_ex_d557()
	{
		Id = 0xD557;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_battle_info_ex_d557();
	}
	public int index;
	public string champion;
	public int state;
	public List<st.net.NetBase.guild_battle_group_info_list> guild_battle_group_one = new List<st.net.NetBase.guild_battle_group_info_list>();
	public List<st.net.NetBase.guild_battle_group_info_list> guild_battle_group_two = new List<st.net.NetBase.guild_battle_group_info_list>();
	public List<st.net.NetBase.guild_battle_group_info_list> guild_battle_group_three = new List<st.net.NetBase.guild_battle_group_info_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		index = reader.Read_int();
		champion = reader.Read_str();
		state = reader.Read_int();
		ushort lenguild_battle_group_one = reader.Read_ushort();
		guild_battle_group_one = new List<st.net.NetBase.guild_battle_group_info_list>();
		for(int i_guild_battle_group_one = 0 ; i_guild_battle_group_one < lenguild_battle_group_one ; i_guild_battle_group_one ++)
		{
			st.net.NetBase.guild_battle_group_info_list listData = new st.net.NetBase.guild_battle_group_info_list();
			listData.fromBinary(reader);
			guild_battle_group_one.Add(listData);
		}
		ushort lenguild_battle_group_two = reader.Read_ushort();
		guild_battle_group_two = new List<st.net.NetBase.guild_battle_group_info_list>();
		for(int i_guild_battle_group_two = 0 ; i_guild_battle_group_two < lenguild_battle_group_two ; i_guild_battle_group_two ++)
		{
			st.net.NetBase.guild_battle_group_info_list listData = new st.net.NetBase.guild_battle_group_info_list();
			listData.fromBinary(reader);
			guild_battle_group_two.Add(listData);
		}
		ushort lenguild_battle_group_three = reader.Read_ushort();
		guild_battle_group_three = new List<st.net.NetBase.guild_battle_group_info_list>();
		for(int i_guild_battle_group_three = 0 ; i_guild_battle_group_three < lenguild_battle_group_three ; i_guild_battle_group_three ++)
		{
			st.net.NetBase.guild_battle_group_info_list listData = new st.net.NetBase.guild_battle_group_info_list();
			listData.fromBinary(reader);
			guild_battle_group_three.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(index);
		writer.write_str(champion);
		writer.write_int(state);
		ushort lenguild_battle_group_one = (ushort)guild_battle_group_one.Count;
		writer.write_short(lenguild_battle_group_one);
		for(int i_guild_battle_group_one = 0 ; i_guild_battle_group_one < lenguild_battle_group_one ; i_guild_battle_group_one ++)
		{
			st.net.NetBase.guild_battle_group_info_list listData = guild_battle_group_one[i_guild_battle_group_one];
			listData.toBinary(writer);
		}
		ushort lenguild_battle_group_two = (ushort)guild_battle_group_two.Count;
		writer.write_short(lenguild_battle_group_two);
		for(int i_guild_battle_group_two = 0 ; i_guild_battle_group_two < lenguild_battle_group_two ; i_guild_battle_group_two ++)
		{
			st.net.NetBase.guild_battle_group_info_list listData = guild_battle_group_two[i_guild_battle_group_two];
			listData.toBinary(writer);
		}
		ushort lenguild_battle_group_three = (ushort)guild_battle_group_three.Count;
		writer.write_short(lenguild_battle_group_three);
		for(int i_guild_battle_group_three = 0 ; i_guild_battle_group_three < lenguild_battle_group_three ; i_guild_battle_group_three ++)
		{
			st.net.NetBase.guild_battle_group_info_list listData = guild_battle_group_three[i_guild_battle_group_three];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
