using System.Collections;
using System.Collections.Generic;

public class pt_update_bonfire_exp_d768 : st.net.NetBase.Pt {
	public pt_update_bonfire_exp_d768()
	{
		Id = 0xD768;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_bonfire_exp_d768();
	}
	public int amount_exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		amount_exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(amount_exp);
		return writer.data;
	}

}
