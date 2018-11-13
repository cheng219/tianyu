using System.Collections;
using System.Collections.Generic;

public class pt_set_pk_mode_c013 : st.net.NetBase.Pt {
	public pt_set_pk_mode_c013()
	{
		Id = 0xC013;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_set_pk_mode_c013();
	}
	public byte mode;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		mode = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(mode);
		return writer.data;
	}

}
