using System.Collections;
using System.Collections.Generic;

public class pt_download_reward_result_d822 : st.net.NetBase.Pt {
	public pt_download_reward_result_d822()
	{
		Id = 0xD822;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_download_reward_result_d822();
	}
	public int result;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		result = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(result);
		return writer.data;
	}

}
