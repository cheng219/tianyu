using System.Collections;
using System.Collections.Generic;

public class pt_pet_updata_state_d404 : st.net.NetBase.Pt {
	public pt_pet_updata_state_d404()
	{
		Id = 0xD404;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pet_updata_state_d404();
	}
	public int state;
	public int pet_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		pet_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(pet_type);
		return writer.data;
	}

}
