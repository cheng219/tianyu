using System.Collections;
using System.Collections.Generic;

public class pt_give_flower_allserver_inform_d797 : st.net.NetBase.Pt {
	public pt_give_flower_allserver_inform_d797()
	{
		Id = 0xD797;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_give_flower_allserver_inform_d797();
	}
	public string give_flower_name;
	public string receive_flower_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		give_flower_name = reader.Read_str();
		receive_flower_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(give_flower_name);
		writer.write_str(receive_flower_name);
		return writer.data;
	}

}
