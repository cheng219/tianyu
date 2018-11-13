using System.Collections;
using System.Collections.Generic;

public class pt_req_strengthen_equip_d360 : st.net.NetBase.Pt {
	public pt_req_strengthen_equip_d360()
	{
		Id = 0xD360;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_strengthen_equip_d360();
	}
	public uint id;
	public uint type;
	public int quik_buy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		type = reader.Read_uint();
		quik_buy = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(type);
		writer.write_int(quik_buy);
		return writer.data;
	}

}
