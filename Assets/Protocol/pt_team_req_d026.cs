using System.Collections;
using System.Collections.Generic;

public class pt_team_req_d026 : st.net.NetBase.Pt {
	public pt_team_req_d026()
	{
		Id = 0xD026;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_req_d026();
	}
	public uint req_uid;
	public string req_name;
	public byte req_lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_uid = reader.Read_uint();
		req_name = reader.Read_str();
		req_lev = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_uid);
		writer.write_str(req_name);
		writer.write_byte(req_lev);
		return writer.data;
	}

}
