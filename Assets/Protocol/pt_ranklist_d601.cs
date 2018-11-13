using System.Collections;
using System.Collections.Generic;

public class pt_ranklist_d601 : st.net.NetBase.Pt {
	public pt_ranklist_d601()
	{
		Id = 0xD601;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ranklist_d601();
	}
	public byte type;
	public byte page;
	public List<st.net.NetBase.rank_info_base> ranklist = new List<st.net.NetBase.rank_info_base>();
	public ushort rank;
	public int value1;
	public int value2;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_byte();
		page = reader.Read_byte();
		ushort lenranklist = reader.Read_ushort();
		ranklist = new List<st.net.NetBase.rank_info_base>();
		for(int i_ranklist = 0 ; i_ranklist < lenranklist ; i_ranklist ++)
		{
			st.net.NetBase.rank_info_base listData = new st.net.NetBase.rank_info_base();
			listData.fromBinary(reader);
			ranklist.Add(listData);
		}
		rank = reader.Read_ushort();
		value1 = reader.Read_int();
		value2 = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(type);
		writer.write_byte(page);
		ushort lenranklist = (ushort)ranklist.Count;
		writer.write_short(lenranklist);
		for(int i_ranklist = 0 ; i_ranklist < lenranklist ; i_ranklist ++)
		{
			st.net.NetBase.rank_info_base listData = ranklist[i_ranklist];
			listData.toBinary(writer);
		}
		writer.write_short(rank);
		writer.write_int(value1);
		writer.write_int(value2);
		return writer.data;
	}

}
