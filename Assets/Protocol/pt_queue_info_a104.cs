using System.Collections;
using System.Collections.Generic;

public class pt_queue_info_a104 : st.net.NetBase.Pt {
	public pt_queue_info_a104()
	{
		Id = 0xA104;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_queue_info_a104();
	}
	public uint max_num;
	public uint cur_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		max_num = reader.Read_uint();
		cur_num = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(max_num);
		writer.write_int(cur_num);
		return writer.data;
	}

}
