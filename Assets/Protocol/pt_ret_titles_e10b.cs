using System.Collections;
using System.Collections.Generic;

public class pt_ret_titles_e10b : st.net.NetBase.Pt {
	public pt_ret_titles_e10b()
	{
		Id = 0xE10B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_titles_e10b();
	}
	public ushort used;
	public List<st.net.NetBase.title_obj> titles = new List<st.net.NetBase.title_obj>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		used = reader.Read_ushort();
		ushort lentitles = reader.Read_ushort();
		titles = new List<st.net.NetBase.title_obj>();
		for(int i_titles = 0 ; i_titles < lentitles ; i_titles ++)
		{
			st.net.NetBase.title_obj listData = new st.net.NetBase.title_obj();
			listData.fromBinary(reader);
			titles.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_short(used);
		ushort lentitles = (ushort)titles.Count;
		writer.write_short(lentitles);
		for(int i_titles = 0 ; i_titles < lentitles ; i_titles ++)
		{
			st.net.NetBase.title_obj listData = titles[i_titles];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
