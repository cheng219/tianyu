using System.Collections;
using System.Collections.Generic;

public class pt_reply_open_server_gift_info_d911 : st.net.NetBase.Pt {
	public pt_reply_open_server_gift_info_d911()
	{
		Id = 0xD911;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_open_server_gift_info_d911();
	}
	public uint rest_time;
	public List<st.net.NetBase.open_server_gift_ware_info> wares = new List<st.net.NetBase.open_server_gift_ware_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rest_time = reader.Read_uint();
		ushort lenwares = reader.Read_ushort();
		wares = new List<st.net.NetBase.open_server_gift_ware_info>();
		for(int i_wares = 0 ; i_wares < lenwares ; i_wares ++)
		{
			st.net.NetBase.open_server_gift_ware_info listData = new st.net.NetBase.open_server_gift_ware_info();
			listData.fromBinary(reader);
			wares.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rest_time);
		ushort lenwares = (ushort)wares.Count;
		writer.write_short(lenwares);
		for(int i_wares = 0 ; i_wares < lenwares ; i_wares ++)
		{
			st.net.NetBase.open_server_gift_ware_info listData = wares[i_wares];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
