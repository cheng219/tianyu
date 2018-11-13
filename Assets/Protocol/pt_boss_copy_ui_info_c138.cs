using System.Collections;
using System.Collections.Generic;

public class pt_boss_copy_ui_info_c138 : st.net.NetBase.Pt {
	public pt_boss_copy_ui_info_c138()
	{
		Id = 0xC138;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_boss_copy_ui_info_c138();
	}
	public int challenge_num;
	public int surplus_buy_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		challenge_num = reader.Read_int();
		surplus_buy_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(challenge_num);
		writer.write_int(surplus_buy_num);
		return writer.data;
	}

}
