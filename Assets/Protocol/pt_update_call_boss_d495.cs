using System.Collections;
using System.Collections.Generic;

public class pt_update_call_boss_d495 : st.net.NetBase.Pt {
	public pt_update_call_boss_d495()
	{
		Id = 0xD495;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_call_boss_d495();
	}
	public List<st.net.NetBase.call_boss_list> call_boss_list = new List<st.net.NetBase.call_boss_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lencall_boss_list = reader.Read_ushort();
		call_boss_list = new List<st.net.NetBase.call_boss_list>();
		for(int i_call_boss_list = 0 ; i_call_boss_list < lencall_boss_list ; i_call_boss_list ++)
		{
			st.net.NetBase.call_boss_list listData = new st.net.NetBase.call_boss_list();
			listData.fromBinary(reader);
			call_boss_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lencall_boss_list = (ushort)call_boss_list.Count;
		writer.write_short(lencall_boss_list);
		for(int i_call_boss_list = 0 ; i_call_boss_list < lencall_boss_list ; i_call_boss_list ++)
		{
			st.net.NetBase.call_boss_list listData = call_boss_list[i_call_boss_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
