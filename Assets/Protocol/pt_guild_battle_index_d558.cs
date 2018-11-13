using System.Collections;
using System.Collections.Generic;

public class pt_guild_battle_index_d558 : st.net.NetBase.Pt {
	public pt_guild_battle_index_d558()
	{
		Id = 0xD558;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_battle_index_d558();
	}
	public List<int> index_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenindex_list = reader.Read_ushort();
		index_list = new List<int>();
		for(int i_index_list = 0 ; i_index_list < lenindex_list ; i_index_list ++)
		{
			int listData = reader.Read_int();
			index_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenindex_list = (ushort)index_list.Count;
		writer.write_short(lenindex_list);
		for(int i_index_list = 0 ; i_index_list < lenindex_list ; i_index_list ++)
		{
			int listData = index_list[i_index_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
