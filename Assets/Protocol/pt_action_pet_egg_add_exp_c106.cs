using System.Collections;
using System.Collections.Generic;

public class pt_action_pet_egg_add_exp_c106 : st.net.NetBase.Pt {
	public pt_action_pet_egg_add_exp_c106()
	{
		Id = 0xC106;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_action_pet_egg_add_exp_c106();
	}
	public int pet_id;
	public int pet_egg_type;
	public int pet_egg_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		pet_id = reader.Read_int();
		pet_egg_type = reader.Read_int();
		pet_egg_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(pet_id);
		writer.write_int(pet_egg_type);
		writer.write_int(pet_egg_num);
		return writer.data;
	}

}
