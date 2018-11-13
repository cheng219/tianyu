using System.Collections;
using System.Collections.Generic;

public class pt_req_continue_hc_e118 : st.net.NetBase.Pt {
	public pt_req_continue_hc_e118()
	{
		Id = 0xE118;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_continue_hc_e118();
	}
	public List<st.net.NetBase.drop_des> drops = new List<st.net.NetBase.drop_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lendrops = reader.Read_ushort();
		drops = new List<st.net.NetBase.drop_des>();
		for(int i_drops = 0 ; i_drops < lendrops ; i_drops ++)
		{
			st.net.NetBase.drop_des listData = new st.net.NetBase.drop_des();
			listData.fromBinary(reader);
			drops.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lendrops = (ushort)drops.Count;
		writer.write_short(lendrops);
		for(int i_drops = 0 ; i_drops < lendrops ; i_drops ++)
		{
			st.net.NetBase.drop_des listData = drops[i_drops];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
