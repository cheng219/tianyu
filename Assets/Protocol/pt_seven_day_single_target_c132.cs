using System.Collections;
using System.Collections.Generic;

public class pt_seven_day_single_target_c132 : st.net.NetBase.Pt {
	public pt_seven_day_single_target_c132()
	{
		Id = 0xC132;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_seven_day_single_target_c132();
	}
	public List<st.net.NetBase.single_day_info> single_day_info = new List<st.net.NetBase.single_day_info>();
	public uint days;
	public uint finish_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lensingle_day_info = reader.Read_ushort();
		single_day_info = new List<st.net.NetBase.single_day_info>();
		for(int i_single_day_info = 0 ; i_single_day_info < lensingle_day_info ; i_single_day_info ++)
		{
			st.net.NetBase.single_day_info listData = new st.net.NetBase.single_day_info();
			listData.fromBinary(reader);
			single_day_info.Add(listData);
		}
		days = reader.Read_uint();
		finish_num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lensingle_day_info = (ushort)single_day_info.Count;
		writer.write_short(lensingle_day_info);
		for(int i_single_day_info = 0 ; i_single_day_info < lensingle_day_info ; i_single_day_info ++)
		{
			st.net.NetBase.single_day_info listData = single_day_info[i_single_day_info];
			listData.toBinary(writer);
		}
		writer.write_int(days);
		writer.write_int(finish_num);
		return writer.data;
	}

}
