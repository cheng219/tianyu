using System.Collections;
using System.Collections.Generic;

public class pt_update_pet_name_d426 : st.net.NetBase.Pt {
	public pt_update_pet_name_d426()
	{
		Id = 0xD426;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_pet_name_d426();
	}
	public int pet_type;
	public string new_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		pet_type = reader.Read_int();
		new_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(pet_type);
		writer.write_str(new_name);
		return writer.data;
	}

}
