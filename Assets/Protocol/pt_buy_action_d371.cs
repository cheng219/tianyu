using System.Collections;
using System.Collections.Generic;

public class pt_buy_action_d371 : st.net.NetBase.Pt {
	public pt_buy_action_d371()
	{
		Id = 0xD371;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_buy_action_d371();
	}
	public int id;
	public uint num;
	public int action;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		num = reader.Read_uint();
		action = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(num);
		writer.write_int(action);
		return writer.data;
	}

}
