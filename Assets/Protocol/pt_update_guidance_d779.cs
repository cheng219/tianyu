using System.Collections;
using System.Collections.Generic;

public class pt_update_guidance_d779 : st.net.NetBase.Pt {
	public pt_update_guidance_d779()
	{
		Id = 0xD779;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_guidance_d779();
	}
	public int guidance_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guidance_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(guidance_id);
		return writer.data;
	}

}
