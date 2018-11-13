using System.Collections;
using System.Collections.Generic;

public class pt_req_look_rank_usrinfo_d773 : st.net.NetBase.Pt {
	public pt_req_look_rank_usrinfo_d773()
	{
		Id = 0xD773;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_look_rank_usrinfo_d773();
	}
	public int uid;
	public string name;
	public int lev;
	public int prof;
	public List<int> model_clothes_id = new List<int>();
	public int wing_id;
	public int wing_lev;
	public List<st.net.NetBase.item_des> target_equip_list = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		name = reader.Read_str();
		lev = reader.Read_int();
		prof = reader.Read_int();
		ushort lenmodel_clothes_id = reader.Read_ushort();
		model_clothes_id = new List<int>();
		for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
		{
			int listData = reader.Read_int();
			model_clothes_id.Add(listData);
		}
		wing_id = reader.Read_int();
		wing_lev = reader.Read_int();
		ushort lentarget_equip_list = reader.Read_ushort();
		target_equip_list = new List<st.net.NetBase.item_des>();
		for(int i_target_equip_list = 0 ; i_target_equip_list < lentarget_equip_list ; i_target_equip_list ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			target_equip_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_str(name);
		writer.write_int(lev);
		writer.write_int(prof);
		ushort lenmodel_clothes_id = (ushort)model_clothes_id.Count;
		writer.write_short(lenmodel_clothes_id);
		for(int i_model_clothes_id = 0 ; i_model_clothes_id < lenmodel_clothes_id ; i_model_clothes_id ++)
		{
			int listData = model_clothes_id[i_model_clothes_id];
			writer.write_int(listData);
		}
		writer.write_int(wing_id);
		writer.write_int(wing_lev);
		ushort lentarget_equip_list = (ushort)target_equip_list.Count;
		writer.write_short(lentarget_equip_list);
		for(int i_target_equip_list = 0 ; i_target_equip_list < lentarget_equip_list ; i_target_equip_list ++)
		{
			st.net.NetBase.item_des listData = target_equip_list[i_target_equip_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
