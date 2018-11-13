using System.Collections;
using System.Collections.Generic;

public class pt_update_quell_demon_drop_out_c145 : st.net.NetBase.Pt {
	public pt_update_quell_demon_drop_out_c145()
	{
		Id = 0xC145;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_quell_demon_drop_out_c145();
	}
	public int drop_out_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		drop_out_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(drop_out_num);
		return writer.data;
	}

}
