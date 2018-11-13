using System.Collections;
using System.Collections.Generic;

public class pt_scene_property_c007 : st.net.NetBase.Pt {
	public pt_scene_property_c007()
	{
		Id = 0xC007;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_property_c007();
	}
	public uint oid;
	public List<st.net.NetBase.property> property_list = new List<st.net.NetBase.property>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		ushort lenproperty_list = reader.Read_ushort();
		property_list = new List<st.net.NetBase.property>();
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property listData = new st.net.NetBase.property();
			listData.fromBinary(reader);
			property_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		ushort lenproperty_list = (ushort)property_list.Count;
		writer.write_short(lenproperty_list);
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property listData = property_list[i_property_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
