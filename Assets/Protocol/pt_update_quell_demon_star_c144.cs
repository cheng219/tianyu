using System.Collections;
using System.Collections.Generic;

public class pt_update_quell_demon_star_c144 : st.net.NetBase.Pt {
	public pt_update_quell_demon_star_c144()
	{
		Id = 0xC144;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_quell_demon_star_c144();
	}
	public int star_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		star_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(star_num);
		return writer.data;
	}

}
