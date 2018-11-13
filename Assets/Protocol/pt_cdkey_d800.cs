using System.Collections;
using System.Collections.Generic;

public class pt_cdkey_d800 : st.net.NetBase.Pt {
	public pt_cdkey_d800()
	{
		Id = 0xD800;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_cdkey_d800();
	}
	public string cdkey;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		cdkey = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(cdkey);
		return writer.data;
	}

}
