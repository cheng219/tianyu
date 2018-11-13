using System.Collections;
using System.Collections.Generic;

public class pt_req_trade_s2c_d631 : st.net.NetBase.Pt {
	public pt_req_trade_s2c_d631()
	{
		Id = 0xD631;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_trade_s2c_d631();
	}
	public uint uid;
	public uint req_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		req_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_int(req_id);
		return writer.data;
	}

}
