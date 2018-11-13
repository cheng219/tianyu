using System.Collections;
using System.Collections.Generic;

public class pt_ret_trials_info_e115 : st.net.NetBase.Pt {
	public pt_ret_trials_info_e115()
	{
		Id = 0xE115;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_trials_info_e115();
	}
	public List<st.net.NetBase.trial_nums> trials = new List<st.net.NetBase.trial_nums>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentrials = reader.Read_ushort();
		trials = new List<st.net.NetBase.trial_nums>();
		for(int i_trials = 0 ; i_trials < lentrials ; i_trials ++)
		{
			st.net.NetBase.trial_nums listData = new st.net.NetBase.trial_nums();
			listData.fromBinary(reader);
			trials.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentrials = (ushort)trials.Count;
		writer.write_short(lentrials);
		for(int i_trials = 0 ; i_trials < lentrials ; i_trials ++)
		{
			st.net.NetBase.trial_nums listData = trials[i_trials];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
