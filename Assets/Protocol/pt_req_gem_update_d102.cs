using System.Collections;
using System.Collections.Generic;

public class pt_req_gem_update_d102 : st.net.NetBase.Pt {
	public pt_req_gem_update_d102()
	{
		Id = 0xD102;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_gem_update_d102();
	}
	public int gem_id;
	public int item_id;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		gem_id = reader.Read_int();
		item_id = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(gem_id);
		writer.write_int(item_id);
		writer.write_int(state);
		return writer.data;
	}

}
