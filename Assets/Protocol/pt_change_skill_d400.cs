using System.Collections;
using System.Collections.Generic;

public class pt_change_skill_d400 : st.net.NetBase.Pt {
	public pt_change_skill_d400()
	{
		Id = 0xD400;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_change_skill_d400();
	}
	public List<st.net.NetBase.change_skill_list> change_skill_list = new List<st.net.NetBase.change_skill_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenchange_skill_list = reader.Read_ushort();
		change_skill_list = new List<st.net.NetBase.change_skill_list>();
		for(int i_change_skill_list = 0 ; i_change_skill_list < lenchange_skill_list ; i_change_skill_list ++)
		{
			st.net.NetBase.change_skill_list listData = new st.net.NetBase.change_skill_list();
			listData.fromBinary(reader);
			change_skill_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenchange_skill_list = (ushort)change_skill_list.Count;
		writer.write_short(lenchange_skill_list);
		for(int i_change_skill_list = 0 ; i_change_skill_list < lenchange_skill_list ; i_change_skill_list ++)
		{
			st.net.NetBase.change_skill_list listData = change_skill_list[i_change_skill_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
