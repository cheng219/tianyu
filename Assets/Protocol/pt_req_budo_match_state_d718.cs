using System.Collections;
using System.Collections.Generic;

public class pt_req_budo_match_state_d718 : st.net.NetBase.Pt {
	public pt_req_budo_match_state_d718()
	{
		Id = 0xD718;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_budo_match_state_d718();
	}
	public uint state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		return writer.data;
	}

}
