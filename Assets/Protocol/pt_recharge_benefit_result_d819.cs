using System.Collections;
using System.Collections.Generic;

public class pt_recharge_benefit_result_d819 : st.net.NetBase.Pt {
	public pt_recharge_benefit_result_d819()
	{
		Id = 0xD819;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_recharge_benefit_result_d819();
	}
	public int condition;
	public int diamo;
	public int vipexp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		condition = reader.Read_int();
		diamo = reader.Read_int();
		vipexp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(condition);
		writer.write_int(diamo);
		writer.write_int(vipexp);
		return writer.data;
	}

}
