using System.Collections;
using System.Collections.Generic;

public class pt_single_many_copy_info_d453 : st.net.NetBase.Pt {
	public pt_single_many_copy_info_d453()
	{
		Id = 0xD453;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_single_many_copy_info_d453();
	}
	public List<st.net.NetBase.single_many_list> single_many_list = new List<st.net.NetBase.single_many_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lensingle_many_list = reader.Read_ushort();
		single_many_list = new List<st.net.NetBase.single_many_list>();
		for(int i_single_many_list = 0 ; i_single_many_list < lensingle_many_list ; i_single_many_list ++)
		{
			st.net.NetBase.single_many_list listData = new st.net.NetBase.single_many_list();
			listData.fromBinary(reader);
			single_many_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lensingle_many_list = (ushort)single_many_list.Count;
		writer.write_short(lensingle_many_list);
		for(int i_single_many_list = 0 ; i_single_many_list < lensingle_many_list ; i_single_many_list ++)
		{
			st.net.NetBase.single_many_list listData = single_many_list[i_single_many_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
