using System.Collections;
using System.Collections.Generic;

public class pt_lucky_wheel_info_d961 : st.net.NetBase.Pt {
	public pt_lucky_wheel_info_d961()
	{
		Id = 0xD961;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lucky_wheel_info_d961();
	}
	public uint rest_time;
	public uint price1;
	public uint price2;
	public uint jackpot;
	public List<st.net.NetBase.lucky_wheel_reward_info> wheel_info = new List<st.net.NetBase.lucky_wheel_reward_info>();
	public byte flag;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rest_time = reader.Read_uint();
		price1 = reader.Read_uint();
		price2 = reader.Read_uint();
		jackpot = reader.Read_uint();
		ushort lenwheel_info = reader.Read_ushort();
		wheel_info = new List<st.net.NetBase.lucky_wheel_reward_info>();
		for(int i_wheel_info = 0 ; i_wheel_info < lenwheel_info ; i_wheel_info ++)
		{
			st.net.NetBase.lucky_wheel_reward_info listData = new st.net.NetBase.lucky_wheel_reward_info();
			listData.fromBinary(reader);
			wheel_info.Add(listData);
		}
		flag = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rest_time);
		writer.write_int(price1);
		writer.write_int(price2);
		writer.write_int(jackpot);
		ushort lenwheel_info = (ushort)wheel_info.Count;
		writer.write_short(lenwheel_info);
		for(int i_wheel_info = 0 ; i_wheel_info < lenwheel_info ; i_wheel_info ++)
		{
			st.net.NetBase.lucky_wheel_reward_info listData = wheel_info[i_wheel_info];
			listData.toBinary(writer);
		}
		writer.write_byte(flag);
		return writer.data;
	}

}
