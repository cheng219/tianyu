using System.Collections;
using System.Collections.Generic;

public class pt_update_prepare_state_d466 : st.net.NetBase.Pt {
	public pt_update_prepare_state_d466()
	{
		Id = 0xD466;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_prepare_state_d466();
	}
	public List<st.net.NetBase.update_prepare> update_prepare = new List<st.net.NetBase.update_prepare>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenupdate_prepare = reader.Read_ushort();
		update_prepare = new List<st.net.NetBase.update_prepare>();
		for(int i_update_prepare = 0 ; i_update_prepare < lenupdate_prepare ; i_update_prepare ++)
		{
			st.net.NetBase.update_prepare listData = new st.net.NetBase.update_prepare();
			listData.fromBinary(reader);
			update_prepare.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenupdate_prepare = (ushort)update_prepare.Count;
		writer.write_short(lenupdate_prepare);
		for(int i_update_prepare = 0 ; i_update_prepare < lenupdate_prepare ; i_update_prepare ++)
		{
			st.net.NetBase.update_prepare listData = update_prepare[i_update_prepare];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
