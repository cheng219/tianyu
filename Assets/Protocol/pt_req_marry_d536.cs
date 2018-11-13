using System.Collections;
using System.Collections.Generic;

public class pt_req_marry_d536 : st.net.NetBase.Pt {
	public pt_req_marry_d536()
	{
		Id = 0xD536;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_marry_d536();
	}
	public int keepsake_type;
	public int marry_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		keepsake_type = reader.Read_int();
		marry_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(keepsake_type);
		writer.write_int(marry_type);
		return writer.data;
	}

}
