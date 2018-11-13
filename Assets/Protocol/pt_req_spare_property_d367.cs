using System.Collections;
using System.Collections.Generic;

public class pt_req_spare_property_d367 : st.net.NetBase.Pt {
	public pt_req_spare_property_d367()
	{
		Id = 0xD367;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_spare_property_d367();
	}
	public int equip_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		equip_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(equip_id);
		return writer.data;
	}

}
