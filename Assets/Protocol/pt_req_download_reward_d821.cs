using System.Collections;
using System.Collections.Generic;

public class pt_req_download_reward_d821 : st.net.NetBase.Pt {
	public pt_req_download_reward_d821()
	{
		Id = 0xD821;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_download_reward_d821();
	}
	public string sdktoken;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		sdktoken = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(sdktoken);
		return writer.data;
	}

}
