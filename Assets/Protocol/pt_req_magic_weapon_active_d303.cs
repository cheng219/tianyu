using System.Collections;
using System.Collections.Generic;

public class pt_req_magic_weapon_active_d303 : st.net.NetBase.Pt {
	public pt_req_magic_weapon_active_d303()
	{
		Id = 0xD303;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_magic_weapon_active_d303();
	}
	public int id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
