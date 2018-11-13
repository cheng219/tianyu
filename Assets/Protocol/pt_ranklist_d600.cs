using System.Collections;
using System.Collections.Generic;

public class pt_ranklist_d600 : st.net.NetBase.Pt {
	public pt_ranklist_d600()
	{
		Id = 0xD600;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ranklist_d600();
	}
	public byte type;
	public byte page;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_byte();
		page = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(type);
		writer.write_byte(page);
		return writer.data;
	}

}
