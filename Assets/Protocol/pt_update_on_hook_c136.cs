using System.Collections;
using System.Collections.Generic;

public class pt_update_on_hook_c136 : st.net.NetBase.Pt {
	public pt_update_on_hook_c136()
	{
		Id = 0xC136;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_on_hook_c136();
	}
	public int surplus_monster;
	public int on_hook_exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		surplus_monster = reader.Read_int();
		on_hook_exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(surplus_monster);
		writer.write_int(on_hook_exp);
		return writer.data;
	}

}
