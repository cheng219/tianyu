using System.Collections;
using System.Collections.Generic;

public class pt_update_copy_boss_count_d700 : st.net.NetBase.Pt {
	public pt_update_copy_boss_count_d700()
	{
		Id = 0xD700;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_copy_boss_count_d700();
	}
	public List<st.net.NetBase.boss_count> boss_count = new List<st.net.NetBase.boss_count>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenboss_count = reader.Read_ushort();
		boss_count = new List<st.net.NetBase.boss_count>();
		for(int i_boss_count = 0 ; i_boss_count < lenboss_count ; i_boss_count ++)
		{
			st.net.NetBase.boss_count listData = new st.net.NetBase.boss_count();
			listData.fromBinary(reader);
			boss_count.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenboss_count = (ushort)boss_count.Count;
		writer.write_short(lenboss_count);
		for(int i_boss_count = 0 ; i_boss_count < lenboss_count ; i_boss_count ++)
		{
			st.net.NetBase.boss_count listData = boss_count[i_boss_count];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
