using System.Collections;
using System.Collections.Generic;

public class pt_scene_add_c002 : st.net.NetBase.Pt {
	public pt_scene_add_c002()
	{
		Id = 0xC002;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_add_c002();
	}
	public List<st.net.NetBase.scene_ply> ply_list = new List<st.net.NetBase.scene_ply>();
	public List<st.net.NetBase.scene_monster> monster_list = new List<st.net.NetBase.scene_monster>();
	public List<st.net.NetBase.scene_item> item_list = new List<st.net.NetBase.scene_item>();
	public List<st.net.NetBase.scene_entourage> entourage_list = new List<st.net.NetBase.scene_entourage>();
	public List<st.net.NetBase.usr_equip_list> usr_equip_list = new List<st.net.NetBase.usr_equip_list>();
	public List<st.net.NetBase.drop_des> drop_item_list = new List<st.net.NetBase.drop_des>();
	public List<st.net.NetBase.scene_model> model_list = new List<st.net.NetBase.scene_model>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenply_list = reader.Read_ushort();
		ply_list = new List<st.net.NetBase.scene_ply>();
		for(int i_ply_list = 0 ; i_ply_list < lenply_list ; i_ply_list ++)
		{
			st.net.NetBase.scene_ply listData = new st.net.NetBase.scene_ply();
			listData.fromBinary(reader);
			ply_list.Add(listData);
		}
		ushort lenmonster_list = reader.Read_ushort();
		monster_list = new List<st.net.NetBase.scene_monster>();
		for(int i_monster_list = 0 ; i_monster_list < lenmonster_list ; i_monster_list ++)
		{
			st.net.NetBase.scene_monster listData = new st.net.NetBase.scene_monster();
			listData.fromBinary(reader);
			monster_list.Add(listData);
		}
		ushort lenitem_list = reader.Read_ushort();
		item_list = new List<st.net.NetBase.scene_item>();
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.scene_item listData = new st.net.NetBase.scene_item();
			listData.fromBinary(reader);
			item_list.Add(listData);
		}
		ushort lenentourage_list = reader.Read_ushort();
		entourage_list = new List<st.net.NetBase.scene_entourage>();
		for(int i_entourage_list = 0 ; i_entourage_list < lenentourage_list ; i_entourage_list ++)
		{
			st.net.NetBase.scene_entourage listData = new st.net.NetBase.scene_entourage();
			listData.fromBinary(reader);
			entourage_list.Add(listData);
		}
		ushort lenusr_equip_list = reader.Read_ushort();
		usr_equip_list = new List<st.net.NetBase.usr_equip_list>();
		for(int i_usr_equip_list = 0 ; i_usr_equip_list < lenusr_equip_list ; i_usr_equip_list ++)
		{
			st.net.NetBase.usr_equip_list listData = new st.net.NetBase.usr_equip_list();
			listData.fromBinary(reader);
			usr_equip_list.Add(listData);
		}
		ushort lendrop_item_list = reader.Read_ushort();
		drop_item_list = new List<st.net.NetBase.drop_des>();
		for(int i_drop_item_list = 0 ; i_drop_item_list < lendrop_item_list ; i_drop_item_list ++)
		{
			st.net.NetBase.drop_des listData = new st.net.NetBase.drop_des();
			listData.fromBinary(reader);
			drop_item_list.Add(listData);
		}
		ushort lenmodel_list = reader.Read_ushort();
		model_list = new List<st.net.NetBase.scene_model>();
		for(int i_model_list = 0 ; i_model_list < lenmodel_list ; i_model_list ++)
		{
			st.net.NetBase.scene_model listData = new st.net.NetBase.scene_model();
			listData.fromBinary(reader);
			model_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenply_list = (ushort)ply_list.Count;
		writer.write_short(lenply_list);
		for(int i_ply_list = 0 ; i_ply_list < lenply_list ; i_ply_list ++)
		{
			st.net.NetBase.scene_ply listData = ply_list[i_ply_list];
			listData.toBinary(writer);
		}
		ushort lenmonster_list = (ushort)monster_list.Count;
		writer.write_short(lenmonster_list);
		for(int i_monster_list = 0 ; i_monster_list < lenmonster_list ; i_monster_list ++)
		{
			st.net.NetBase.scene_monster listData = monster_list[i_monster_list];
			listData.toBinary(writer);
		}
		ushort lenitem_list = (ushort)item_list.Count;
		writer.write_short(lenitem_list);
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.scene_item listData = item_list[i_item_list];
			listData.toBinary(writer);
		}
		ushort lenentourage_list = (ushort)entourage_list.Count;
		writer.write_short(lenentourage_list);
		for(int i_entourage_list = 0 ; i_entourage_list < lenentourage_list ; i_entourage_list ++)
		{
			st.net.NetBase.scene_entourage listData = entourage_list[i_entourage_list];
			listData.toBinary(writer);
		}
		ushort lenusr_equip_list = (ushort)usr_equip_list.Count;
		writer.write_short(lenusr_equip_list);
		for(int i_usr_equip_list = 0 ; i_usr_equip_list < lenusr_equip_list ; i_usr_equip_list ++)
		{
			st.net.NetBase.usr_equip_list listData = usr_equip_list[i_usr_equip_list];
			listData.toBinary(writer);
		}
		ushort lendrop_item_list = (ushort)drop_item_list.Count;
		writer.write_short(lendrop_item_list);
		for(int i_drop_item_list = 0 ; i_drop_item_list < lendrop_item_list ; i_drop_item_list ++)
		{
			st.net.NetBase.drop_des listData = drop_item_list[i_drop_item_list];
			listData.toBinary(writer);
		}
		ushort lenmodel_list = (ushort)model_list.Count;
		writer.write_short(lenmodel_list);
		for(int i_model_list = 0 ; i_model_list < lenmodel_list ; i_model_list ++)
		{
			st.net.NetBase.scene_model listData = model_list[i_model_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
