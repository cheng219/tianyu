using System.Collections;
using System.Collections.Generic;

public class pt_endless_pass_list_d435 : st.net.NetBase.Pt {
	public pt_endless_pass_list_d435()
	{
		Id = 0xD435;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_endless_pass_list_d435();
	}
	public List<st.net.NetBase.endless_list> endless_list = new List<st.net.NetBase.endless_list>();
	public int reset_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenendless_list = reader.Read_ushort();
		endless_list = new List<st.net.NetBase.endless_list>();
		for(int i_endless_list = 0 ; i_endless_list < lenendless_list ; i_endless_list ++)
		{
			st.net.NetBase.endless_list listData = new st.net.NetBase.endless_list();
			listData.fromBinary(reader);
			endless_list.Add(listData);
		}
		reset_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenendless_list = (ushort)endless_list.Count;
		writer.write_short(lenendless_list);
		for(int i_endless_list = 0 ; i_endless_list < lenendless_list ; i_endless_list ++)
		{
			st.net.NetBase.endless_list listData = endless_list[i_endless_list];
			listData.toBinary(writer);
		}
		writer.write_int(reset_num);
		return writer.data;
	}

}
