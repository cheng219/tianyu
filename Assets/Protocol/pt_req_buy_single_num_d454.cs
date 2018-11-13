using System.Collections;
using System.Collections.Generic;

public class pt_req_buy_single_num_d454 : st.net.NetBase.Pt {
	public pt_req_buy_single_num_d454()
	{
		Id = 0xD454;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_buy_single_num_d454();
	}
	public int copy_id;
	public int buy_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_int();
		buy_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(buy_num);
		return writer.data;
	}

}
