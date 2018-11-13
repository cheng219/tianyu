using System.Collections;
using System.Collections.Generic;

public class pt_daily_recharge_benifit_succ_d991 : st.net.NetBase.Pt {
	public pt_daily_recharge_benifit_succ_d991()
	{
		Id = 0xD991;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_daily_recharge_benifit_succ_d991();
	}
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		return writer.data;
	}

}
