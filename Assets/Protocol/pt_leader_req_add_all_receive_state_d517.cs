using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_add_all_receive_state_d517 : st.net.NetBase.Pt {
	public pt_leader_req_add_all_receive_state_d517()
	{
		Id = 0xD517;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_add_all_receive_state_d517();
	}
	public int open_state;
	public int fight_score;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		open_state = reader.Read_int();
		fight_score = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(open_state);
		writer.write_int(fight_score);
		return writer.data;
	}

}
