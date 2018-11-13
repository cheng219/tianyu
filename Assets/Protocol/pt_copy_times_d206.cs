using System.Collections;
using System.Collections.Generic;

public class pt_copy_times_d206 : st.net.NetBase.Pt {
	public pt_copy_times_d206()
	{
		Id = 0xD206;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_times_d206();
	}
	public List<st.net.NetBase.copy_times> copys = new List<st.net.NetBase.copy_times>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lencopys = reader.Read_ushort();
		copys = new List<st.net.NetBase.copy_times>();
		for(int i_copys = 0 ; i_copys < lencopys ; i_copys ++)
		{
			st.net.NetBase.copy_times listData = new st.net.NetBase.copy_times();
			listData.fromBinary(reader);
			copys.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lencopys = (ushort)copys.Count;
		writer.write_short(lencopys);
		for(int i_copys = 0 ; i_copys < lencopys ; i_copys ++)
		{
			st.net.NetBase.copy_times listData = copys[i_copys];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
