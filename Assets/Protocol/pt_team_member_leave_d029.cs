using System.Collections;
using System.Collections.Generic;

public class pt_team_member_leave_d029 : st.net.NetBase.Pt {
	public pt_team_member_leave_d029()
	{
		Id = 0xD029;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_member_leave_d029();
	}
	public uint leave_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		leave_uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(leave_uid);
		return writer.data;
	}

}
