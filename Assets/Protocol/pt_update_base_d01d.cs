using System.Collections;
using System.Collections.Generic;

public class pt_update_base_d01d : st.net.NetBase.Pt {
	public pt_update_base_d01d()
	{
		Id = 0xD01D;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_base_d01d();
	}
	public uint uid;
	public List<st.net.NetBase.property> property_list = new List<st.net.NetBase.property>();
	public List<st.net.NetBase.property64> property64_list = new List<st.net.NetBase.property64>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		ushort lenproperty_list = reader.Read_ushort();
		property_list = new List<st.net.NetBase.property>();
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property listData = new st.net.NetBase.property();
			listData.fromBinary(reader);
			property_list.Add(listData);
		}
		ushort lenproperty64_list = reader.Read_ushort();
		property64_list = new List<st.net.NetBase.property64>();
		for(int i_property64_list = 0 ; i_property64_list < lenproperty64_list ; i_property64_list ++)
		{
			st.net.NetBase.property64 listData = new st.net.NetBase.property64();
			listData.fromBinary(reader);
			property64_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		ushort lenproperty_list = (ushort)property_list.Count;
		writer.write_short(lenproperty_list);
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property listData = property_list[i_property_list];
			listData.toBinary(writer);
		}
		ushort lenproperty64_list = (ushort)property64_list.Count;
		writer.write_short(lenproperty64_list);
		for(int i_property64_list = 0 ; i_property64_list < lenproperty64_list ; i_property64_list ++)
		{
			st.net.NetBase.property64 listData = property64_list[i_property64_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
