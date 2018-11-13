using System.Collections;
using System.Collections.Generic;

public class pt_use_someone_title_e10c : st.net.NetBase.Pt {
	public pt_use_someone_title_e10c()
	{
		Id = 0xE10C;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_use_someone_title_e10c();
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
