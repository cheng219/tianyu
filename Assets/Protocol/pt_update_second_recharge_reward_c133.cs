using System.Collections;
using System.Collections.Generic;

public class pt_update_second_recharge_reward_c133 : st.net.NetBase.Pt {
	public pt_update_second_recharge_reward_c133()
	{
		Id = 0xC133;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_second_recharge_reward_c133();
	}
	public byte status;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		status = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(status);
		return writer.data;
	}

}
