using System.Collections;
using System.Collections.Generic;

public class pt_all_skill_d100 : st.net.NetBase.Pt {
	public pt_all_skill_d100()
	{
		Id = 0xD100;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_all_skill_d100();
	}
	public List<st.net.NetBase.normal_skill_list> normal_skill_list = new List<st.net.NetBase.normal_skill_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lennormal_skill_list = reader.Read_ushort();
		normal_skill_list = new List<st.net.NetBase.normal_skill_list>();
		for(int i_normal_skill_list = 0 ; i_normal_skill_list < lennormal_skill_list ; i_normal_skill_list ++)
		{
			st.net.NetBase.normal_skill_list listData = new st.net.NetBase.normal_skill_list();
			listData.fromBinary(reader);
			normal_skill_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lennormal_skill_list = (ushort)normal_skill_list.Count;
		writer.write_short(lennormal_skill_list);
		for(int i_normal_skill_list = 0 ; i_normal_skill_list < lennormal_skill_list ; i_normal_skill_list ++)
		{
			st.net.NetBase.normal_skill_list listData = normal_skill_list[i_normal_skill_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
