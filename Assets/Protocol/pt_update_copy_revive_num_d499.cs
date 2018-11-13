using System.Collections;
using System.Collections.Generic;

public class pt_update_copy_revive_num_d499 : st.net.NetBase.Pt {
	public pt_update_copy_revive_num_d499()
	{
		Id = 0xD499;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_copy_revive_num_d499();
	}
	public int revive_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		revive_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(revive_num);
		return writer.data;
	}

}
