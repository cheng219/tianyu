using System.Collections;
using System.Collections.Generic;

public class pt_update_auto_revive_d785 : st.net.NetBase.Pt {
	public pt_update_auto_revive_d785()
	{
		Id = 0xD785;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_auto_revive_d785();
	}
	public int auto_revive;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		auto_revive = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(auto_revive);
		return writer.data;
	}

}
