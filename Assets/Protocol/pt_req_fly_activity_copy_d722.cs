using System.Collections;
using System.Collections.Generic;

public class pt_req_fly_activity_copy_d722 : st.net.NetBase.Pt {
	public pt_req_fly_activity_copy_d722()
	{
		Id = 0xD722;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_fly_activity_copy_d722();
	}
	public int actid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		actid = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(actid);
		return writer.data;
	}

}
