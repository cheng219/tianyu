using System.Collections;
using System.Collections.Generic;

public class pt_update_title_d423 : st.net.NetBase.Pt {
	public pt_update_title_d423()
	{
		Id = 0xD423;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_title_d423();
	}
	public uint title_id;
	public uint state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		title_id = reader.Read_uint();
		state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(title_id);
		writer.write_int(state);
		return writer.data;
	}

}
