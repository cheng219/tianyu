using System.Collections;
using System.Collections.Generic;

public class pt_req_boss_cpoy_buy_c137 : st.net.NetBase.Pt {
	public pt_req_boss_cpoy_buy_c137()
	{
		Id = 0xC137;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_boss_cpoy_buy_c137();
	}
	public uint req_type;
	public byte buy_nums;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_type = reader.Read_uint();
		buy_nums = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_type);
		writer.write_byte(buy_nums);
		return writer.data;
	}

}
