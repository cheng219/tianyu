using System.Collections;
using System.Collections.Generic;

public class pt_recommend_friends_list_d109 : st.net.NetBase.Pt {
	public pt_recommend_friends_list_d109()
	{
		Id = 0xD109;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_recommend_friends_list_d109();
	}
	public List<st.net.NetBase.relation_info> relation_info = new List<st.net.NetBase.relation_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrelation_info = reader.Read_ushort();
		relation_info = new List<st.net.NetBase.relation_info>();
		for(int i_relation_info = 0 ; i_relation_info < lenrelation_info ; i_relation_info ++)
		{
			st.net.NetBase.relation_info listData = new st.net.NetBase.relation_info();
			listData.fromBinary(reader);
			relation_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrelation_info = (ushort)relation_info.Count;
		writer.write_short(lenrelation_info);
		for(int i_relation_info = 0 ; i_relation_info < lenrelation_info ; i_relation_info ++)
		{
			st.net.NetBase.relation_info listData = relation_info[i_relation_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
