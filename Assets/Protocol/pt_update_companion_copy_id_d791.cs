using System.Collections;
using System.Collections.Generic;

public class pt_update_companion_copy_id_d791 : st.net.NetBase.Pt {
	public pt_update_companion_copy_id_d791()
	{
		Id = 0xD791;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_companion_copy_id_d791();
	}
	public int id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
