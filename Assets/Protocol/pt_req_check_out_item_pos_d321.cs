using System.Collections;
using System.Collections.Generic;

public class pt_req_check_out_item_pos_d321 : st.net.NetBase.Pt {
	public pt_req_check_out_item_pos_d321()
	{
		Id = 0xD321;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_check_out_item_pos_d321();
	}
	public uint id;
	public uint action;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		action = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(action);
		return writer.data;
	}

}
