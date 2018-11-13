using System.Collections;
using System.Collections.Generic;

public class pt_update_function_start_reward_c105 : st.net.NetBase.Pt {
	public pt_update_function_start_reward_c105()
	{
		Id = 0xC105;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_function_start_reward_c105();
	}
	public int get_reward_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		get_reward_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(get_reward_id);
		return writer.data;
	}

}
