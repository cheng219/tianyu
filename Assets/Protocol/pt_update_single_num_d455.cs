using System.Collections;
using System.Collections.Generic;

public class pt_update_single_num_d455 : st.net.NetBase.Pt {
	public pt_update_single_num_d455()
	{
		Id = 0xD455;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_single_num_d455();
	}
	public int copy_id;
	public int challenge_num;
	public int buy_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_int();
		challenge_num = reader.Read_int();
		buy_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(challenge_num);
		writer.write_int(buy_num);
		return writer.data;
	}

}
