using System.Collections;
using System.Collections.Generic;

public class pt_recharge_flag_d80b : st.net.NetBase.Pt {
	public pt_recharge_flag_d80b()
	{
		Id = 0xD80B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_recharge_flag_d80b();
	}
	public List<st.net.NetBase.recharge_flag_info> flag_list = new List<st.net.NetBase.recharge_flag_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenflag_list = reader.Read_ushort();
		flag_list = new List<st.net.NetBase.recharge_flag_info>();
		for(int i_flag_list = 0 ; i_flag_list < lenflag_list ; i_flag_list ++)
		{
			st.net.NetBase.recharge_flag_info listData = new st.net.NetBase.recharge_flag_info();
			listData.fromBinary(reader);
			flag_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenflag_list = (ushort)flag_list.Count;
		writer.write_short(lenflag_list);
		for(int i_flag_list = 0 ; i_flag_list < lenflag_list ; i_flag_list ++)
		{
			st.net.NetBase.recharge_flag_info listData = flag_list[i_flag_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
