using System.Collections;
using System.Collections.Generic;

public class pt_update_battleground_id_c115 : st.net.NetBase.Pt {
	public pt_update_battleground_id_c115()
	{
		Id = 0xC115;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_battleground_id_c115();
	}
	public int battleground_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		battleground_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(battleground_id);
		return writer.data;
	}

}
