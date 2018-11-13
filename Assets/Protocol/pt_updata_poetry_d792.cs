using System.Collections;
using System.Collections.Generic;

public class pt_updata_poetry_d792 : st.net.NetBase.Pt {
	public pt_updata_poetry_d792()
	{
		Id = 0xD792;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_updata_poetry_d792();
	}
	public string poetry;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		poetry = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(poetry);
		return writer.data;
	}

}
