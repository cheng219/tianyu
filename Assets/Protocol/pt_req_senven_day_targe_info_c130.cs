using System.Collections;
using System.Collections.Generic;

public class pt_req_senven_day_targe_info_c130 : st.net.NetBase.Pt {
	public pt_req_senven_day_targe_info_c130()
	{
		Id = 0xC130;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_senven_day_targe_info_c130();
	}
	public uint req_type;
	public uint req_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_type = reader.Read_uint();
		req_state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_type);
		writer.write_int(req_state);
		return writer.data;
	}

}
