using System.Collections;
using System.Collections.Generic;

public class pt_friend_relation_list_d705 : st.net.NetBase.Pt {
	public pt_friend_relation_list_d705()
	{
		Id = 0xD705;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_friend_relation_list_d705();
	}
	public int add_or_remove;
	public List<st.net.NetBase.relation_list> relation_list = new List<st.net.NetBase.relation_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		add_or_remove = reader.Read_int();
		ushort lenrelation_list = reader.Read_ushort();
		relation_list = new List<st.net.NetBase.relation_list>();
		for(int i_relation_list = 0 ; i_relation_list < lenrelation_list ; i_relation_list ++)
		{
			st.net.NetBase.relation_list listData = new st.net.NetBase.relation_list();
			listData.fromBinary(reader);
			relation_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(add_or_remove);
		ushort lenrelation_list = (ushort)relation_list.Count;
		writer.write_short(lenrelation_list);
		for(int i_relation_list = 0 ; i_relation_list < lenrelation_list ; i_relation_list ++)
		{
			st.net.NetBase.relation_list listData = relation_list[i_relation_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
