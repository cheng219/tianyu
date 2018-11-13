using System.Collections;
using System.Collections.Generic;

public class pt_spare_propertys_d365 : st.net.NetBase.Pt {
	public pt_spare_propertys_d365()
	{
		Id = 0xD365;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_spare_propertys_d365();
	}
	public int recast_item_id;
	public List<st.net.NetBase.spare_list> single_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> one_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> two_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> three_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> four_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> five_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> six_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> seven_property = new List<st.net.NetBase.spare_list>();
	public List<st.net.NetBase.spare_list> eight_property = new List<st.net.NetBase.spare_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		recast_item_id = reader.Read_int();
		ushort lensingle_property = reader.Read_ushort();
		single_property = new List<st.net.NetBase.spare_list>();
		for(int i_single_property = 0 ; i_single_property < lensingle_property ; i_single_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			single_property.Add(listData);
		}
		ushort lenone_property = reader.Read_ushort();
		one_property = new List<st.net.NetBase.spare_list>();
		for(int i_one_property = 0 ; i_one_property < lenone_property ; i_one_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			one_property.Add(listData);
		}
		ushort lentwo_property = reader.Read_ushort();
		two_property = new List<st.net.NetBase.spare_list>();
		for(int i_two_property = 0 ; i_two_property < lentwo_property ; i_two_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			two_property.Add(listData);
		}
		ushort lenthree_property = reader.Read_ushort();
		three_property = new List<st.net.NetBase.spare_list>();
		for(int i_three_property = 0 ; i_three_property < lenthree_property ; i_three_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			three_property.Add(listData);
		}
		ushort lenfour_property = reader.Read_ushort();
		four_property = new List<st.net.NetBase.spare_list>();
		for(int i_four_property = 0 ; i_four_property < lenfour_property ; i_four_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			four_property.Add(listData);
		}
		ushort lenfive_property = reader.Read_ushort();
		five_property = new List<st.net.NetBase.spare_list>();
		for(int i_five_property = 0 ; i_five_property < lenfive_property ; i_five_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			five_property.Add(listData);
		}
		ushort lensix_property = reader.Read_ushort();
		six_property = new List<st.net.NetBase.spare_list>();
		for(int i_six_property = 0 ; i_six_property < lensix_property ; i_six_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			six_property.Add(listData);
		}
		ushort lenseven_property = reader.Read_ushort();
		seven_property = new List<st.net.NetBase.spare_list>();
		for(int i_seven_property = 0 ; i_seven_property < lenseven_property ; i_seven_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			seven_property.Add(listData);
		}
		ushort leneight_property = reader.Read_ushort();
		eight_property = new List<st.net.NetBase.spare_list>();
		for(int i_eight_property = 0 ; i_eight_property < leneight_property ; i_eight_property ++)
		{
			st.net.NetBase.spare_list listData = new st.net.NetBase.spare_list();
			listData.fromBinary(reader);
			eight_property.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(recast_item_id);
		ushort lensingle_property = (ushort)single_property.Count;
		writer.write_short(lensingle_property);
		for(int i_single_property = 0 ; i_single_property < lensingle_property ; i_single_property ++)
		{
			st.net.NetBase.spare_list listData = single_property[i_single_property];
			listData.toBinary(writer);
		}
		ushort lenone_property = (ushort)one_property.Count;
		writer.write_short(lenone_property);
		for(int i_one_property = 0 ; i_one_property < lenone_property ; i_one_property ++)
		{
			st.net.NetBase.spare_list listData = one_property[i_one_property];
			listData.toBinary(writer);
		}
		ushort lentwo_property = (ushort)two_property.Count;
		writer.write_short(lentwo_property);
		for(int i_two_property = 0 ; i_two_property < lentwo_property ; i_two_property ++)
		{
			st.net.NetBase.spare_list listData = two_property[i_two_property];
			listData.toBinary(writer);
		}
		ushort lenthree_property = (ushort)three_property.Count;
		writer.write_short(lenthree_property);
		for(int i_three_property = 0 ; i_three_property < lenthree_property ; i_three_property ++)
		{
			st.net.NetBase.spare_list listData = three_property[i_three_property];
			listData.toBinary(writer);
		}
		ushort lenfour_property = (ushort)four_property.Count;
		writer.write_short(lenfour_property);
		for(int i_four_property = 0 ; i_four_property < lenfour_property ; i_four_property ++)
		{
			st.net.NetBase.spare_list listData = four_property[i_four_property];
			listData.toBinary(writer);
		}
		ushort lenfive_property = (ushort)five_property.Count;
		writer.write_short(lenfive_property);
		for(int i_five_property = 0 ; i_five_property < lenfive_property ; i_five_property ++)
		{
			st.net.NetBase.spare_list listData = five_property[i_five_property];
			listData.toBinary(writer);
		}
		ushort lensix_property = (ushort)six_property.Count;
		writer.write_short(lensix_property);
		for(int i_six_property = 0 ; i_six_property < lensix_property ; i_six_property ++)
		{
			st.net.NetBase.spare_list listData = six_property[i_six_property];
			listData.toBinary(writer);
		}
		ushort lenseven_property = (ushort)seven_property.Count;
		writer.write_short(lenseven_property);
		for(int i_seven_property = 0 ; i_seven_property < lenseven_property ; i_seven_property ++)
		{
			st.net.NetBase.spare_list listData = seven_property[i_seven_property];
			listData.toBinary(writer);
		}
		ushort leneight_property = (ushort)eight_property.Count;
		writer.write_short(leneight_property);
		for(int i_eight_property = 0 ; i_eight_property < leneight_property ; i_eight_property ++)
		{
			st.net.NetBase.spare_list listData = eight_property[i_eight_property];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
