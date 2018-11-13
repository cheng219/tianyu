using System.Collections;
using System.Collections.Generic;

public class pt_update_budo_log_list_d720 : st.net.NetBase.Pt {
	public pt_update_budo_log_list_d720()
	{
		Id = 0xD720;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_budo_log_list_d720();
	}
	public List<st.net.NetBase.budo_log_list> budo_log_list = new List<st.net.NetBase.budo_log_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenbudo_log_list = reader.Read_ushort();
		budo_log_list = new List<st.net.NetBase.budo_log_list>();
		for(int i_budo_log_list = 0 ; i_budo_log_list < lenbudo_log_list ; i_budo_log_list ++)
		{
			st.net.NetBase.budo_log_list listData = new st.net.NetBase.budo_log_list();
			listData.fromBinary(reader);
			budo_log_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenbudo_log_list = (ushort)budo_log_list.Count;
		writer.write_short(lenbudo_log_list);
		for(int i_budo_log_list = 0 ; i_budo_log_list < lenbudo_log_list ; i_budo_log_list ++)
		{
			st.net.NetBase.budo_log_list listData = budo_log_list[i_budo_log_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
