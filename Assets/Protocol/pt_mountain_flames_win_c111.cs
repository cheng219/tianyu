using System.Collections;
using System.Collections.Generic;

public class pt_mountain_flames_win_c111 : st.net.NetBase.Pt {
	public pt_mountain_flames_win_c111()
	{
		Id = 0xC111;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mountain_flames_win_c111();
	}
	public int win_state;
	public List<st.net.NetBase.mountain_flames_win> fairyland_list = new List<st.net.NetBase.mountain_flames_win>();
	public List<st.net.NetBase.mountain_flames_win> demon_list = new List<st.net.NetBase.mountain_flames_win>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		win_state = reader.Read_int();
		ushort lenfairyland_list = reader.Read_ushort();
		fairyland_list = new List<st.net.NetBase.mountain_flames_win>();
		for(int i_fairyland_list = 0 ; i_fairyland_list < lenfairyland_list ; i_fairyland_list ++)
		{
			st.net.NetBase.mountain_flames_win listData = new st.net.NetBase.mountain_flames_win();
			listData.fromBinary(reader);
			fairyland_list.Add(listData);
		}
		ushort lendemon_list = reader.Read_ushort();
		demon_list = new List<st.net.NetBase.mountain_flames_win>();
		for(int i_demon_list = 0 ; i_demon_list < lendemon_list ; i_demon_list ++)
		{
			st.net.NetBase.mountain_flames_win listData = new st.net.NetBase.mountain_flames_win();
			listData.fromBinary(reader);
			demon_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(win_state);
		ushort lenfairyland_list = (ushort)fairyland_list.Count;
		writer.write_short(lenfairyland_list);
		for(int i_fairyland_list = 0 ; i_fairyland_list < lenfairyland_list ; i_fairyland_list ++)
		{
			st.net.NetBase.mountain_flames_win listData = fairyland_list[i_fairyland_list];
			listData.toBinary(writer);
		}
		ushort lendemon_list = (ushort)demon_list.Count;
		writer.write_short(lendemon_list);
		for(int i_demon_list = 0 ; i_demon_list < lendemon_list ; i_demon_list ++)
		{
			st.net.NetBase.mountain_flames_win listData = demon_list[i_demon_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
