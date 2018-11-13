using System.Collections;
using System.Collections.Generic;

public class pt_copy_pause_d47a : st.net.NetBase.Pt {
	public pt_copy_pause_d47a()
	{
		Id = 0xD47A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_pause_d47a();
	}
	public byte state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(state);
		return writer.data;
	}

}
