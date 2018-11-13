using System.Collections;
using System.Collections.Generic;

public class pt_req_unstall_gem_d363 : st.net.NetBase.Pt {
	public pt_req_unstall_gem_d363()
	{
		Id = 0xD363;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_unstall_gem_d363();
	}
	public uint equip_id;
	public uint pos;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		equip_id = reader.Read_uint();
		pos = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(equip_id);
		writer.write_int(pos);
		return writer.data;
	}

}
