using System.Collections;
using System.Collections.Generic;

public class pt_req_store_pro_to_equip_d368 : st.net.NetBase.Pt {
	public pt_req_store_pro_to_equip_d368()
	{
		Id = 0xD368;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_store_pro_to_equip_d368();
	}
	public int equip_id;
	public int pos;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		equip_id = reader.Read_int();
		pos = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(equip_id);
		writer.write_int(pos);
		return writer.data;
	}

}
