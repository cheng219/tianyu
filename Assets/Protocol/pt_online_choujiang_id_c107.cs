using System.Collections;
using System.Collections.Generic;

public class pt_online_choujiang_id_c107 : st.net.NetBase.Pt {
	public pt_online_choujiang_id_c107()
	{
		Id = 0xC107;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_online_choujiang_id_c107();
	}
	public int reward_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_id);
		return writer.data;
	}

}
