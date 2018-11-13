using System.Collections;
using System.Collections.Generic;

public class pt_pet_d201 : st.net.NetBase.Pt {
	public pt_pet_d201()
	{
		Id = 0xD201;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pet_d201();
	}
	public List<st.net.NetBase.pet_info> pets = new List<st.net.NetBase.pet_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenpets = reader.Read_ushort();
		pets = new List<st.net.NetBase.pet_info>();
		for(int i_pets = 0 ; i_pets < lenpets ; i_pets ++)
		{
			st.net.NetBase.pet_info listData = new st.net.NetBase.pet_info();
			listData.fromBinary(reader);
			pets.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenpets = (ushort)pets.Count;
		writer.write_short(lenpets);
		for(int i_pets = 0 ; i_pets < lenpets ; i_pets ++)
		{
			st.net.NetBase.pet_info listData = pets[i_pets];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
