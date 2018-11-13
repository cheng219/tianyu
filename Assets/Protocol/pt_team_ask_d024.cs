using System.Collections;
using System.Collections.Generic;

public class pt_team_ask_d024 : st.net.NetBase.Pt {
	public pt_team_ask_d024()
	{
		Id = 0xD024;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_ask_d024();
	}
	public uint ask_uid;
	public string ask_usr_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ask_uid = reader.Read_uint();
		ask_usr_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ask_uid);
		writer.write_str(ask_usr_name);
		return writer.data;
	}

}
