using System.Collections;
using System.Collections.Generic;

public class pt_camp_task_d135 : st.net.NetBase.Pt {
	public pt_camp_task_d135()
	{
		Id = 0xD135;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_camp_task_d135();
	}
	public uint camp_task_star;
	public uint camp_task_loop;
	public uint camp_rewards_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		camp_task_star = reader.Read_uint();
		camp_task_loop = reader.Read_uint();
		camp_rewards_state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(camp_task_star);
		writer.write_int(camp_task_loop);
		writer.write_int(camp_rewards_state);
		return writer.data;
	}

}
