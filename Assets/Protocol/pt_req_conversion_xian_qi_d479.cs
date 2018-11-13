using System.Collections;
using System.Collections.Generic;

public class pt_req_conversion_xian_qi_d479 : st.net.NetBase.Pt {
	public pt_req_conversion_xian_qi_d479()
	{
		Id = 0xD479;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_conversion_xian_qi_d479();
	}
	public uint num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(num);
		return writer.data;
	}

}
