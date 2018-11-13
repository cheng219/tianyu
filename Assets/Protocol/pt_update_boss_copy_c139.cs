using System.Collections;
using System.Collections.Generic;

public class pt_update_boss_copy_c139 : st.net.NetBase.Pt {
	public pt_update_boss_copy_c139()
	{
		Id = 0xC139;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_boss_copy_c139();
	}
	public int boss_surplus_num;
	public int add_property;
	public List<st.net.NetBase.boss_copy_list> boss_list = new List<st.net.NetBase.boss_copy_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		boss_surplus_num = reader.Read_int();
		add_property = reader.Read_int();
		ushort lenboss_list = reader.Read_ushort();
		boss_list = new List<st.net.NetBase.boss_copy_list>();
		for(int i_boss_list = 0 ; i_boss_list < lenboss_list ; i_boss_list ++)
		{
			st.net.NetBase.boss_copy_list listData = new st.net.NetBase.boss_copy_list();
			listData.fromBinary(reader);
			boss_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(boss_surplus_num);
		writer.write_int(add_property);
		ushort lenboss_list = (ushort)boss_list.Count;
		writer.write_short(lenboss_list);
		for(int i_boss_list = 0 ; i_boss_list < lenboss_list ; i_boss_list ++)
		{
			st.net.NetBase.boss_copy_list listData = boss_list[i_boss_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
