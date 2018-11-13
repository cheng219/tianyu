using System.Collections;
using System.Collections.Generic;

public class pt_look_pet_ride_info_d713 : st.net.NetBase.Pt {
	public pt_look_pet_ride_info_d713()
	{
		Id = 0xD713;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_look_pet_ride_info_d713();
	}
	public List<st.net.NetBase.pet_base_info> target_pet_list = new List<st.net.NetBase.pet_base_info>();
	public int ride_type;
	public int ride_lev;
	public int ride_skin_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentarget_pet_list = reader.Read_ushort();
		target_pet_list = new List<st.net.NetBase.pet_base_info>();
		for(int i_target_pet_list = 0 ; i_target_pet_list < lentarget_pet_list ; i_target_pet_list ++)
		{
			st.net.NetBase.pet_base_info listData = new st.net.NetBase.pet_base_info();
			listData.fromBinary(reader);
			target_pet_list.Add(listData);
		}
		ride_type = reader.Read_int();
		ride_lev = reader.Read_int();
		ride_skin_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentarget_pet_list = (ushort)target_pet_list.Count;
		writer.write_short(lentarget_pet_list);
		for(int i_target_pet_list = 0 ; i_target_pet_list < lentarget_pet_list ; i_target_pet_list ++)
		{
			st.net.NetBase.pet_base_info listData = target_pet_list[i_target_pet_list];
			listData.toBinary(writer);
		}
		writer.write_int(ride_type);
		writer.write_int(ride_lev);
		writer.write_int(ride_skin_id);
		return writer.data;
	}

}
