using System.Collections;
using System.Collections.Generic;

public class pt_scene_usr_team_chg_c018 : st.net.NetBase.Pt {
	public pt_scene_usr_team_chg_c018()
	{
		Id = 0xC018;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_usr_team_chg_c018();
	}
	public uint uid;
	public uint team_id;
	public uint team_leader;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		team_id = reader.Read_uint();
		team_leader = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_int(team_id);
		writer.write_int(team_leader);
		return writer.data;
	}

}
