using System.Collections;
using System.Collections.Generic;

public class pt_ranklist_d120 : st.net.NetBase.Pt {
	public pt_ranklist_d120()
	{
		Id = 0xD120;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ranklist_d120();
	}
	public uint ranklistId;
	public uint position;
	public List<st.net.NetBase.ranklist> ranklist = new List<st.net.NetBase.ranklist>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ranklistId = reader.Read_uint();
		position = reader.Read_uint();
		ushort lenranklist = reader.Read_ushort();
		ranklist = new List<st.net.NetBase.ranklist>();
		for(int i_ranklist = 0 ; i_ranklist < lenranklist ; i_ranklist ++)
		{
			st.net.NetBase.ranklist listData = new st.net.NetBase.ranklist();
			listData.fromBinary(reader);
			ranklist.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ranklistId);
		writer.write_int(position);
		ushort lenranklist = (ushort)ranklist.Count;
		writer.write_short(lenranklist);
		for(int i_ranklist = 0 ; i_ranklist < lenranklist ; i_ranklist ++)
		{
			st.net.NetBase.ranklist listData = ranklist[i_ranklist];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
