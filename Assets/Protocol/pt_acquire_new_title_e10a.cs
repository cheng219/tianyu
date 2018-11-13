using System.Collections;
using System.Collections.Generic;

public class pt_acquire_new_title_e10a : st.net.NetBase.Pt {
	public pt_acquire_new_title_e10a()
	{
		Id = 0xE10A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_acquire_new_title_e10a();
	}
	public ushort title;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		title = reader.Read_ushort();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_short(title);
		return writer.data;
	}

}
