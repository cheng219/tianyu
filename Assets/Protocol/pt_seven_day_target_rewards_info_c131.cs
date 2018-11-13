using System.Collections;
using System.Collections.Generic;

public class pt_seven_day_target_rewards_info_c131 : st.net.NetBase.Pt {
	public pt_seven_day_target_rewards_info_c131()
	{
		Id = 0xC131;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_seven_day_target_rewards_info_c131();
	}
	public List<st.net.NetBase.seven_day_target_list> seven_day_target = new List<st.net.NetBase.seven_day_target_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenseven_day_target = reader.Read_ushort();
		seven_day_target = new List<st.net.NetBase.seven_day_target_list>();
		for(int i_seven_day_target = 0 ; i_seven_day_target < lenseven_day_target ; i_seven_day_target ++)
		{
			st.net.NetBase.seven_day_target_list listData = new st.net.NetBase.seven_day_target_list();
			listData.fromBinary(reader);
			seven_day_target.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenseven_day_target = (ushort)seven_day_target.Count;
		writer.write_short(lenseven_day_target);
		for(int i_seven_day_target = 0 ; i_seven_day_target < lenseven_day_target ; i_seven_day_target ++)
		{
			st.net.NetBase.seven_day_target_list listData = seven_day_target[i_seven_day_target];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
