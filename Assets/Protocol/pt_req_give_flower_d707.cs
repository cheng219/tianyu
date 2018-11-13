using System.Collections;
using System.Collections.Generic;

public class pt_req_give_flower_d707 : st.net.NetBase.Pt {
	public pt_req_give_flower_d707()
	{
		Id = 0xD707;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_give_flower_d707();
	}
	public int flower_type;
	public int oth_uid;
	public int source;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		flower_type = reader.Read_int();
		oth_uid = reader.Read_int();
		source = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(flower_type);
		writer.write_int(oth_uid);
		writer.write_int(source);
		return writer.data;
	}

}
