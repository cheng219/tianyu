using System.Collections;
using System.Collections.Generic;

public class pt_on_hook_ui_info_c135 : st.net.NetBase.Pt {
	public pt_on_hook_ui_info_c135()
	{
		Id = 0xC135;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_on_hook_ui_info_c135();
	}
	public int surplus_monster;
	public int surplus_buy_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		surplus_monster = reader.Read_int();
		surplus_buy_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(surplus_monster);
		writer.write_int(surplus_buy_num);
		return writer.data;
	}

}
