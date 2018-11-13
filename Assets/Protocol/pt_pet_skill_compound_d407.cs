using System.Collections;
using System.Collections.Generic;

public class pt_pet_skill_compound_d407 : st.net.NetBase.Pt {
	public pt_pet_skill_compound_d407()
	{
		Id = 0xD407;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pet_skill_compound_d407();
	}
	public int need_compound_item;
	public List<int> skill_compound_item = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		need_compound_item = reader.Read_int();
		ushort lenskill_compound_item = reader.Read_ushort();
		skill_compound_item = new List<int>();
		for(int i_skill_compound_item = 0 ; i_skill_compound_item < lenskill_compound_item ; i_skill_compound_item ++)
		{
			int listData = reader.Read_int();
			skill_compound_item.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(need_compound_item);
		ushort lenskill_compound_item = (ushort)skill_compound_item.Count;
		writer.write_short(lenskill_compound_item);
		for(int i_skill_compound_item = 0 ; i_skill_compound_item < lenskill_compound_item ; i_skill_compound_item ++)
		{
			int listData = skill_compound_item[i_skill_compound_item];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
