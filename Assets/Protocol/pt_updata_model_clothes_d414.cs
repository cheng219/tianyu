using System.Collections;
using System.Collections.Generic;

public class pt_updata_model_clothes_d414 : st.net.NetBase.Pt {
	public pt_updata_model_clothes_d414()
	{
		Id = 0xD414;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_updata_model_clothes_d414();
	}
	public int model_id;
	public int put_state;
	public int time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		model_id = reader.Read_int();
		put_state = reader.Read_int();
		time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(model_id);
		writer.write_int(put_state);
		writer.write_int(time);
		return writer.data;
	}

}
