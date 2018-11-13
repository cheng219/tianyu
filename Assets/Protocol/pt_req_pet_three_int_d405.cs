using System.Collections;
using System.Collections.Generic;

public class pt_req_pet_three_int_d405 : st.net.NetBase.Pt {
	public pt_req_pet_three_int_d405()
	{
		Id = 0xD405;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_pet_three_int_d405();
	}
	public uint req_state;
	public uint parameter_1;
	public uint parameter_2;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_state = reader.Read_uint();
		parameter_1 = reader.Read_uint();
		parameter_2 = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_state);
		writer.write_int(parameter_1);
		writer.write_int(parameter_2);
		return writer.data;
	}

}
