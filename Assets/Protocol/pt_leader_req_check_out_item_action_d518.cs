using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_check_out_item_action_d518 : st.net.NetBase.Pt {
	public pt_leader_req_check_out_item_action_d518()
	{
		Id = 0xD518;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_check_out_item_action_d518();
	}
	public int target_uid;
	public int item_id;
	public int action_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		target_uid = reader.Read_int();
		item_id = reader.Read_int();
		action_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(target_uid);
		writer.write_int(item_id);
		writer.write_int(action_type);
		return writer.data;
	}

}
