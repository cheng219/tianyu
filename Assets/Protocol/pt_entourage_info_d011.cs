using System.Collections;
using System.Collections.Generic;

public class pt_entourage_info_d011 : st.net.NetBase.Pt {
	public pt_entourage_info_d011()
	{
		Id = 0xD011;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_entourage_info_d011();
	}
	public uint eType;
	public uint fighting;
	public uint lev;
	public uint exp;
	public uint hp;
	public uint itemType;
	public uint itemVal;
	public uint eStar;
	public uint fightType;
	public uint played;
	public List<st.net.NetBase.skill_list> skill_list = new List<st.net.NetBase.skill_list>();
	public List<st.net.NetBase.equip_list> equip_list = new List<st.net.NetBase.equip_list>();
	public List<st.net.NetBase.property_list> property_list = new List<st.net.NetBase.property_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		eType = reader.Read_uint();
		fighting = reader.Read_uint();
		lev = reader.Read_uint();
		exp = reader.Read_uint();
		hp = reader.Read_uint();
		itemType = reader.Read_uint();
		itemVal = reader.Read_uint();
		eStar = reader.Read_uint();
		fightType = reader.Read_uint();
		played = reader.Read_uint();
		ushort lenskill_list = reader.Read_ushort();
		skill_list = new List<st.net.NetBase.skill_list>();
		for(int i_skill_list = 0 ; i_skill_list < lenskill_list ; i_skill_list ++)
		{
			st.net.NetBase.skill_list listData = new st.net.NetBase.skill_list();
			listData.fromBinary(reader);
			skill_list.Add(listData);
		}
		ushort lenequip_list = reader.Read_ushort();
		equip_list = new List<st.net.NetBase.equip_list>();
		for(int i_equip_list = 0 ; i_equip_list < lenequip_list ; i_equip_list ++)
		{
			st.net.NetBase.equip_list listData = new st.net.NetBase.equip_list();
			listData.fromBinary(reader);
			equip_list.Add(listData);
		}
		ushort lenproperty_list = reader.Read_ushort();
		property_list = new List<st.net.NetBase.property_list>();
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property_list listData = new st.net.NetBase.property_list();
			listData.fromBinary(reader);
			property_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(eType);
		writer.write_int(fighting);
		writer.write_int(lev);
		writer.write_int(exp);
		writer.write_int(hp);
		writer.write_int(itemType);
		writer.write_int(itemVal);
		writer.write_int(eStar);
		writer.write_int(fightType);
		writer.write_int(played);
		ushort lenskill_list = (ushort)skill_list.Count;
		writer.write_short(lenskill_list);
		for(int i_skill_list = 0 ; i_skill_list < lenskill_list ; i_skill_list ++)
		{
			st.net.NetBase.skill_list listData = skill_list[i_skill_list];
			listData.toBinary(writer);
		}
		ushort lenequip_list = (ushort)equip_list.Count;
		writer.write_short(lenequip_list);
		for(int i_equip_list = 0 ; i_equip_list < lenequip_list ; i_equip_list ++)
		{
			st.net.NetBase.equip_list listData = equip_list[i_equip_list];
			listData.toBinary(writer);
		}
		ushort lenproperty_list = (ushort)property_list.Count;
		writer.write_short(lenproperty_list);
		for(int i_property_list = 0 ; i_property_list < lenproperty_list ; i_property_list ++)
		{
			st.net.NetBase.property_list listData = property_list[i_property_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
