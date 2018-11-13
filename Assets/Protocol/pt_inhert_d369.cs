using System.Collections;
using System.Collections.Generic;

public class pt_inhert_d369 : st.net.NetBase.Pt {
	public pt_inhert_d369()
	{
		Id = 0xD369;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_inhert_d369();
	}
	public int id;
	public int target_id;
	public List<int> inhert_list = new List<int>();
	public int quik_buy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		target_id = reader.Read_int();
		ushort leninhert_list = reader.Read_ushort();
		inhert_list = new List<int>();
		for(int i_inhert_list = 0 ; i_inhert_list < leninhert_list ; i_inhert_list ++)
		{
			int listData = reader.Read_int();
			inhert_list.Add(listData);
		}
		quik_buy = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(target_id);
		ushort leninhert_list = (ushort)inhert_list.Count;
		writer.write_short(leninhert_list);
		for(int i_inhert_list = 0 ; i_inhert_list < leninhert_list ; i_inhert_list ++)
		{
			int listData = inhert_list[i_inhert_list];
			writer.write_int(listData);
		}
		writer.write_int(quik_buy);
		return writer.data;
	}

}
