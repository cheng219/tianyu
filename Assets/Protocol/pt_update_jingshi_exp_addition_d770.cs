using System.Collections;
using System.Collections.Generic;

public class pt_update_jingshi_exp_addition_d770 : st.net.NetBase.Pt {
	public pt_update_jingshi_exp_addition_d770()
	{
		Id = 0xD770;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_jingshi_exp_addition_d770();
	}
	public int exp_addition;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		exp_addition = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(exp_addition);
		return writer.data;
	}

}
