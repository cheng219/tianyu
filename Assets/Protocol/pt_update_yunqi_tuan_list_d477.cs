using System.Collections;
using System.Collections.Generic;

public class pt_update_yunqi_tuan_list_d477 : st.net.NetBase.Pt {
	public pt_update_yunqi_tuan_list_d477()
	{
		Id = 0xD477;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_yunqi_tuan_list_d477();
	}
	public List<int> xian_qi_list = new List<int>();
	public List<int> ling_qi_list = new List<int>();
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenxian_qi_list = reader.Read_ushort();
		xian_qi_list = new List<int>();
		for(int i_xian_qi_list = 0 ; i_xian_qi_list < lenxian_qi_list ; i_xian_qi_list ++)
		{
			int listData = reader.Read_int();
			xian_qi_list.Add(listData);
		}
		ushort lenling_qi_list = reader.Read_ushort();
		ling_qi_list = new List<int>();
		for(int i_ling_qi_list = 0 ; i_ling_qi_list < lenling_qi_list ; i_ling_qi_list ++)
		{
			int listData = reader.Read_int();
			ling_qi_list.Add(listData);
		}
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenxian_qi_list = (ushort)xian_qi_list.Count;
		writer.write_short(lenxian_qi_list);
		for(int i_xian_qi_list = 0 ; i_xian_qi_list < lenxian_qi_list ; i_xian_qi_list ++)
		{
			int listData = xian_qi_list[i_xian_qi_list];
			writer.write_int(listData);
		}
		ushort lenling_qi_list = (ushort)ling_qi_list.Count;
		writer.write_short(lenling_qi_list);
		for(int i_ling_qi_list = 0 ; i_ling_qi_list < lenling_qi_list ; i_ling_qi_list ++)
		{
			int listData = ling_qi_list[i_ling_qi_list];
			writer.write_int(listData);
		}
		writer.write_int(state);
		return writer.data;
	}

}
