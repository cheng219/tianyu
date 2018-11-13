using System.Collections;
using System.Collections.Generic;

public class pt_recharge_succ_d80a : st.net.NetBase.Pt {
	public pt_recharge_succ_d80a()
	{
		Id = 0xD80A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_recharge_succ_d80a();
	}
	public uint type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		return writer.data;
	}

}
