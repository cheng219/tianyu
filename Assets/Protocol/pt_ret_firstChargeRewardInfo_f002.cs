using System.Collections;
using System.Collections.Generic;

public class pt_ret_firstChargeRewardInfo_f002 : st.net.NetBase.Pt {
	public pt_ret_firstChargeRewardInfo_f002()
	{
		Id = 0xF002;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_firstChargeRewardInfo_f002();
	}
	public byte action;
	public byte reward_info;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_byte();
		reward_info = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(action);
		writer.write_byte(reward_info);
		return writer.data;
	}

}
