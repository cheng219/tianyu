using System.Collections;
using System.Collections.Generic;

public class pt_req_inlay_gem_d362 : st.net.NetBase.Pt {
	public pt_req_inlay_gem_d362()
	{
		Id = 0xD362;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_inlay_gem_d362();
	}
	public uint gem_id;
	public uint equip_id;
	public uint pos;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		gem_id = reader.Read_uint();
		equip_id = reader.Read_uint();
		pos = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(gem_id);
		writer.write_int(equip_id);
		writer.write_int(pos);
		return writer.data;
	}

}
