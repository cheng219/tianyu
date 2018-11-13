using System.Collections;
using System.Collections.Generic;

public class pt_pet_updata_property_d409 : st.net.NetBase.Pt {
	public pt_pet_updata_property_d409()
	{
		Id = 0xD409;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pet_updata_property_d409();
	}
	public int state;
	public int pet_type;
	public int lev;
	public int num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		pet_type = reader.Read_int();
		lev = reader.Read_int();
		num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(pet_type);
		writer.write_int(lev);
		writer.write_int(num);
		return writer.data;
	}

}
