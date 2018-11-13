using System.Collections;
using System.Collections.Generic;

public class pt_team_ask_cancle_d031 : st.net.NetBase.Pt {
	public pt_team_ask_cancle_d031()
	{
		Id = 0xD031;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_ask_cancle_d031();
	}
	public uint ask_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ask_uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ask_uid);
		return writer.data;
	}

}
