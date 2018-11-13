using System.Collections;
using System.Collections.Generic;

public class pt_mastery_d020 : st.net.NetBase.Pt {
	public pt_mastery_d020()
	{
		Id = 0xD020;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mastery_d020();
	}
	public List<st.net.NetBase.mastery_list> masterys = new List<st.net.NetBase.mastery_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmasterys = reader.Read_ushort();
		masterys = new List<st.net.NetBase.mastery_list>();
		for(int i_masterys = 0 ; i_masterys < lenmasterys ; i_masterys ++)
		{
			st.net.NetBase.mastery_list listData = new st.net.NetBase.mastery_list();
			listData.fromBinary(reader);
			masterys.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmasterys = (ushort)masterys.Count;
		writer.write_short(lenmasterys);
		for(int i_masterys = 0 ; i_masterys < lenmasterys ; i_masterys ++)
		{
			st.net.NetBase.mastery_list listData = masterys[i_masterys];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
