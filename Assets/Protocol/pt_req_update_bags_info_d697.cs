using System.Collections;
using System.Collections.Generic;

public class pt_req_update_bags_info_d697 : st.net.NetBase.Pt {
	public pt_req_update_bags_info_d697()
	{
		Id = 0xD697;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_update_bags_info_d697();
	}
	public int num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(num);
		return writer.data;
	}

}
