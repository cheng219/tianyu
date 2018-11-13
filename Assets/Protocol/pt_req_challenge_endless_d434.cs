using System.Collections;
using System.Collections.Generic;

public class pt_req_challenge_endless_d434 : st.net.NetBase.Pt {
	public pt_req_challenge_endless_d434()
	{
		Id = 0xD434;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_challenge_endless_d434();
	}
	public int chapter_id;
	public int pass_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		chapter_id = reader.Read_int();
		pass_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(chapter_id);
		writer.write_int(pass_id);
		return writer.data;
	}

}
