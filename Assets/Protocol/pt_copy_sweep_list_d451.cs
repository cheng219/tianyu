using System.Collections;
using System.Collections.Generic;

public class pt_copy_sweep_list_d451 : st.net.NetBase.Pt {
	public pt_copy_sweep_list_d451()
	{
		Id = 0xD451;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_sweep_list_d451();
	}
	public List<st.net.NetBase.copy_sweep_list> copy_sweep = new List<st.net.NetBase.copy_sweep_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lencopy_sweep = reader.Read_ushort();
		copy_sweep = new List<st.net.NetBase.copy_sweep_list>();
		for(int i_copy_sweep = 0 ; i_copy_sweep < lencopy_sweep ; i_copy_sweep ++)
		{
			st.net.NetBase.copy_sweep_list listData = new st.net.NetBase.copy_sweep_list();
			listData.fromBinary(reader);
			copy_sweep.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lencopy_sweep = (ushort)copy_sweep.Count;
		writer.write_short(lencopy_sweep);
		for(int i_copy_sweep = 0 ; i_copy_sweep < lencopy_sweep ; i_copy_sweep ++)
		{
			st.net.NetBase.copy_sweep_list listData = copy_sweep[i_copy_sweep];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
