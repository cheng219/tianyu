using System.Collections;
using System.Collections.Generic;

public class pt_liveness_state_d692 : st.net.NetBase.Pt {
	public pt_liveness_state_d692()
	{
		Id = 0xD692;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_liveness_state_d692();
	}
	public List<st.net.NetBase.liveness_info> liveness_list = new List<st.net.NetBase.liveness_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenliveness_list = reader.Read_ushort();
		liveness_list = new List<st.net.NetBase.liveness_info>();
		for(int i_liveness_list = 0 ; i_liveness_list < lenliveness_list ; i_liveness_list ++)
		{
			st.net.NetBase.liveness_info listData = new st.net.NetBase.liveness_info();
			listData.fromBinary(reader);
			liveness_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenliveness_list = (ushort)liveness_list.Count;
		writer.write_short(lenliveness_list);
		for(int i_liveness_list = 0 ; i_liveness_list < lenliveness_list ; i_liveness_list ++)
		{
			st.net.NetBase.liveness_info listData = liveness_list[i_liveness_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
