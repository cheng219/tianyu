using System.Collections;
using System.Collections.Generic;

public class pt_friends_info_d107 : st.net.NetBase.Pt {
	public pt_friends_info_d107()
	{
		Id = 0xD107;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_friends_info_d107();
	}
	public uint add_or_remove;
	public uint thumb_up_num;
	public List<st.net.NetBase.friends_info> friends_info = new List<st.net.NetBase.friends_info>();
	public List<st.net.NetBase.enemy_info> enemy_info = new List<st.net.NetBase.enemy_info>();
	public List<st.net.NetBase.history_info_list> history_info_list = new List<st.net.NetBase.history_info_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		add_or_remove = reader.Read_uint();
		thumb_up_num = reader.Read_uint();
		ushort lenfriends_info = reader.Read_ushort();
		friends_info = new List<st.net.NetBase.friends_info>();
		for(int i_friends_info = 0 ; i_friends_info < lenfriends_info ; i_friends_info ++)
		{
			st.net.NetBase.friends_info listData = new st.net.NetBase.friends_info();
			listData.fromBinary(reader);
			friends_info.Add(listData);
		}
		ushort lenenemy_info = reader.Read_ushort();
		enemy_info = new List<st.net.NetBase.enemy_info>();
		for(int i_enemy_info = 0 ; i_enemy_info < lenenemy_info ; i_enemy_info ++)
		{
			st.net.NetBase.enemy_info listData = new st.net.NetBase.enemy_info();
			listData.fromBinary(reader);
			enemy_info.Add(listData);
		}
		ushort lenhistory_info_list = reader.Read_ushort();
		history_info_list = new List<st.net.NetBase.history_info_list>();
		for(int i_history_info_list = 0 ; i_history_info_list < lenhistory_info_list ; i_history_info_list ++)
		{
			st.net.NetBase.history_info_list listData = new st.net.NetBase.history_info_list();
			listData.fromBinary(reader);
			history_info_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(add_or_remove);
		writer.write_int(thumb_up_num);
		ushort lenfriends_info = (ushort)friends_info.Count;
		writer.write_short(lenfriends_info);
		for(int i_friends_info = 0 ; i_friends_info < lenfriends_info ; i_friends_info ++)
		{
			st.net.NetBase.friends_info listData = friends_info[i_friends_info];
			listData.toBinary(writer);
		}
		ushort lenenemy_info = (ushort)enemy_info.Count;
		writer.write_short(lenenemy_info);
		for(int i_enemy_info = 0 ; i_enemy_info < lenenemy_info ; i_enemy_info ++)
		{
			st.net.NetBase.enemy_info listData = enemy_info[i_enemy_info];
			listData.toBinary(writer);
		}
		ushort lenhistory_info_list = (ushort)history_info_list.Count;
		writer.write_short(lenhistory_info_list);
		for(int i_history_info_list = 0 ; i_history_info_list < lenhistory_info_list ; i_history_info_list ++)
		{
			st.net.NetBase.history_info_list listData = history_info_list[i_history_info_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
