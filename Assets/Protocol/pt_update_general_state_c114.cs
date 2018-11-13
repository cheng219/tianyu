using System.Collections;
using System.Collections.Generic;

public class pt_update_general_state_c114 : st.net.NetBase.Pt {
	public pt_update_general_state_c114()
	{
		Id = 0xC114;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_general_state_c114();
	}
	public List<st.net.NetBase.general_list> fairyland_general = new List<st.net.NetBase.general_list>();
	public List<st.net.NetBase.general_list> demon_general = new List<st.net.NetBase.general_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenfairyland_general = reader.Read_ushort();
		fairyland_general = new List<st.net.NetBase.general_list>();
		for(int i_fairyland_general = 0 ; i_fairyland_general < lenfairyland_general ; i_fairyland_general ++)
		{
			st.net.NetBase.general_list listData = new st.net.NetBase.general_list();
			listData.fromBinary(reader);
			fairyland_general.Add(listData);
		}
		ushort lendemon_general = reader.Read_ushort();
		demon_general = new List<st.net.NetBase.general_list>();
		for(int i_demon_general = 0 ; i_demon_general < lendemon_general ; i_demon_general ++)
		{
			st.net.NetBase.general_list listData = new st.net.NetBase.general_list();
			listData.fromBinary(reader);
			demon_general.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenfairyland_general = (ushort)fairyland_general.Count;
		writer.write_short(lenfairyland_general);
		for(int i_fairyland_general = 0 ; i_fairyland_general < lenfairyland_general ; i_fairyland_general ++)
		{
			st.net.NetBase.general_list listData = fairyland_general[i_fairyland_general];
			listData.toBinary(writer);
		}
		ushort lendemon_general = (ushort)demon_general.Count;
		writer.write_short(lendemon_general);
		for(int i_demon_general = 0 ; i_demon_general < lendemon_general ; i_demon_general ++)
		{
			st.net.NetBase.general_list listData = demon_general[i_demon_general];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
