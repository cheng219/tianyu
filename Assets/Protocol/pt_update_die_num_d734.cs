using System.Collections;
using System.Collections.Generic;

public class pt_update_die_num_d734 : st.net.NetBase.Pt {
	public pt_update_die_num_d734()
	{
		Id = 0xD734;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_die_num_d734();
	}
	public int cur_die_num;
	public int max_die_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		cur_die_num = reader.Read_int();
		max_die_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(cur_die_num);
		writer.write_int(max_die_num);
		return writer.data;
	}

}
