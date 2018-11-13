using System.Collections;
using System.Collections.Generic;

public class pt_draw_times_d128 : st.net.NetBase.Pt {
	public pt_draw_times_d128()
	{
		Id = 0xD128;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_draw_times_d128();
	}
	public List<st.net.NetBase.draw_times> draw_times = new List<st.net.NetBase.draw_times>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lendraw_times = reader.Read_ushort();
		draw_times = new List<st.net.NetBase.draw_times>();
		for(int i_draw_times = 0 ; i_draw_times < lendraw_times ; i_draw_times ++)
		{
			st.net.NetBase.draw_times listData = new st.net.NetBase.draw_times();
			listData.fromBinary(reader);
			draw_times.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lendraw_times = (ushort)draw_times.Count;
		writer.write_short(lendraw_times);
		for(int i_draw_times = 0 ; i_draw_times < lendraw_times ; i_draw_times ++)
		{
			st.net.NetBase.draw_times listData = draw_times[i_draw_times];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
