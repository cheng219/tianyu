using System.Collections;
using System.Collections.Generic;

public class pt_req_put_model_d413 : st.net.NetBase.Pt {
	public pt_req_put_model_d413()
	{
		Id = 0xD413;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_put_model_d413();
	}
	public int model_id;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		model_id = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(model_id);
		writer.write_int(state);
		return writer.data;
	}

}
