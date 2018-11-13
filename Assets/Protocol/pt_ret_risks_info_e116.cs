using System.Collections;
using System.Collections.Generic;

public class pt_ret_risks_info_e116 : st.net.NetBase.Pt {
	public pt_ret_risks_info_e116()
	{
		Id = 0xE116;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_risks_info_e116();
	}
	public List<st.net.NetBase.trial_nums> risks = new List<st.net.NetBase.trial_nums>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrisks = reader.Read_ushort();
		risks = new List<st.net.NetBase.trial_nums>();
		for(int i_risks = 0 ; i_risks < lenrisks ; i_risks ++)
		{
			st.net.NetBase.trial_nums listData = new st.net.NetBase.trial_nums();
			listData.fromBinary(reader);
			risks.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrisks = (ushort)risks.Count;
		writer.write_short(lenrisks);
		for(int i_risks = 0 ; i_risks < lenrisks ; i_risks ++)
		{
			st.net.NetBase.trial_nums listData = risks[i_risks];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
