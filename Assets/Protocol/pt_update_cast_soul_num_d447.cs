using System.Collections;
using System.Collections.Generic;

public class pt_update_cast_soul_num_d447 : st.net.NetBase.Pt {
	public pt_update_cast_soul_num_d447()
	{
		Id = 0xD447;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_cast_soul_num_d447();
	}
	public int state;
	public int soul_id;
	public int surplus_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		soul_id = reader.Read_int();
		surplus_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(soul_id);
		writer.write_int(surplus_num);
		return writer.data;
	}

}
