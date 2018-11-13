using System.Collections;
using System.Collections.Generic;

public class pt_mastery_update_d021 : st.net.NetBase.Pt {
	public pt_mastery_update_d021()
	{
		Id = 0xD021;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mastery_update_d021();
	}
	public uint mastery_id;
	public uint mastery_lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		mastery_id = reader.Read_uint();
		mastery_lev = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(mastery_id);
		writer.write_int(mastery_lev);
		return writer.data;
	}

}
