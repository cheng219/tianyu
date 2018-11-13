using System.Collections;
using System.Collections.Generic;

public class pt_req_pet_info_d403 : st.net.NetBase.Pt {
	public pt_req_pet_info_d403()
	{
		Id = 0xD403;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_pet_info_d403();
	}
	public uint req_state;
	public uint pet_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_state = reader.Read_uint();
		pet_type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_state);
		writer.write_int(pet_type);
		return writer.data;
	}

}
