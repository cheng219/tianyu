using System.Collections;
using System.Collections.Generic;

public class pt_usr_list_a102 : st.net.NetBase.Pt {
	public pt_usr_list_a102()
	{
		Id = 0xA102;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_usr_list_a102();
	}
	public List<st.net.NetBase.create_usr_info> usr_list = new List<st.net.NetBase.create_usr_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenusr_list = reader.Read_ushort();
		usr_list = new List<st.net.NetBase.create_usr_info>();
		for(int i_usr_list = 0 ; i_usr_list < lenusr_list ; i_usr_list ++)
		{
			st.net.NetBase.create_usr_info listData = new st.net.NetBase.create_usr_info();
			listData.fromBinary(reader);
			usr_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenusr_list = (ushort)usr_list.Count;
		writer.write_short(lenusr_list);
		for(int i_usr_list = 0 ; i_usr_list < lenusr_list ; i_usr_list ++)
		{
			st.net.NetBase.create_usr_info listData = usr_list[i_usr_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
