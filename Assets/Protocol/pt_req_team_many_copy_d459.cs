using System.Collections;
using System.Collections.Generic;

public class pt_req_team_many_copy_d459 : st.net.NetBase.Pt {
	public pt_req_team_many_copy_d459()
	{
		Id = 0xD459;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_team_many_copy_d459();
	}
	public int copy_id;
	public int copy_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_int();
		copy_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(copy_type);
		return writer.data;
	}

}
