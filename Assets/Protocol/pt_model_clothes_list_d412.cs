using System.Collections;
using System.Collections.Generic;

public class pt_model_clothes_list_d412 : st.net.NetBase.Pt {
	public pt_model_clothes_list_d412()
	{
		Id = 0xD412;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_model_clothes_list_d412();
	}
	public List<st.net.NetBase.model_clothes_list> model_list = new List<st.net.NetBase.model_clothes_list>();
	public int model_lev;
	public int model_exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmodel_list = reader.Read_ushort();
		model_list = new List<st.net.NetBase.model_clothes_list>();
		for(int i_model_list = 0 ; i_model_list < lenmodel_list ; i_model_list ++)
		{
			st.net.NetBase.model_clothes_list listData = new st.net.NetBase.model_clothes_list();
			listData.fromBinary(reader);
			model_list.Add(listData);
		}
		model_lev = reader.Read_int();
		model_exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmodel_list = (ushort)model_list.Count;
		writer.write_short(lenmodel_list);
		for(int i_model_list = 0 ; i_model_list < lenmodel_list ; i_model_list ++)
		{
			st.net.NetBase.model_clothes_list listData = model_list[i_model_list];
			listData.toBinary(writer);
		}
		writer.write_int(model_lev);
		writer.write_int(model_exp);
		return writer.data;
	}

}
