using System.Collections;
using System.Collections.Generic;

public class pt_team_leader_chg_d028 : st.net.NetBase.Pt {
	public pt_team_leader_chg_d028()
	{
		Id = 0xD028;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_leader_chg_d028();
	}
	public uint new_leader_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		new_leader_uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(new_leader_uid);
		return writer.data;
	}

}
