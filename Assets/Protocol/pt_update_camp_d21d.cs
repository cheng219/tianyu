using System.Collections;
using System.Collections.Generic;

public class pt_update_camp_d21d : st.net.NetBase.Pt {
	public pt_update_camp_d21d()
	{
		Id = 0xD21D;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_camp_d21d();
	}
	public int uid;
	public int camp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		camp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_int(camp);
		return writer.data;
	}

}
