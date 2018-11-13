using System.Collections;
using System.Collections.Generic;

public class pt_boss_challenge_list_d701 : st.net.NetBase.Pt {
	public pt_boss_challenge_list_d701()
	{
		Id = 0xD701;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_boss_challenge_list_d701();
	}
	public List<st.net.NetBase.boss_challenge> underground_palace_boss = new List<st.net.NetBase.boss_challenge>();
	public List<st.net.NetBase.boss_challenge> scene_boss = new List<st.net.NetBase.boss_challenge>();
	public List<st.net.NetBase.boss_challenge> seal_boss = new List<st.net.NetBase.boss_challenge>();
	public List<st.net.NetBase.boss_challenge> smeltters_boss = new List<st.net.NetBase.boss_challenge>();
	public List<st.net.NetBase.boss_challenge> li_smeltters_boss = new List<st.net.NetBase.boss_challenge>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenunderground_palace_boss = reader.Read_ushort();
		underground_palace_boss = new List<st.net.NetBase.boss_challenge>();
		for(int i_underground_palace_boss = 0 ; i_underground_palace_boss < lenunderground_palace_boss ; i_underground_palace_boss ++)
		{
			st.net.NetBase.boss_challenge listData = new st.net.NetBase.boss_challenge();
			listData.fromBinary(reader);
			underground_palace_boss.Add(listData);
		}
		ushort lenscene_boss = reader.Read_ushort();
		scene_boss = new List<st.net.NetBase.boss_challenge>();
		for(int i_scene_boss = 0 ; i_scene_boss < lenscene_boss ; i_scene_boss ++)
		{
			st.net.NetBase.boss_challenge listData = new st.net.NetBase.boss_challenge();
			listData.fromBinary(reader);
			scene_boss.Add(listData);
		}
		ushort lenseal_boss = reader.Read_ushort();
		seal_boss = new List<st.net.NetBase.boss_challenge>();
		for(int i_seal_boss = 0 ; i_seal_boss < lenseal_boss ; i_seal_boss ++)
		{
			st.net.NetBase.boss_challenge listData = new st.net.NetBase.boss_challenge();
			listData.fromBinary(reader);
			seal_boss.Add(listData);
		}
		ushort lensmeltters_boss = reader.Read_ushort();
		smeltters_boss = new List<st.net.NetBase.boss_challenge>();
		for(int i_smeltters_boss = 0 ; i_smeltters_boss < lensmeltters_boss ; i_smeltters_boss ++)
		{
			st.net.NetBase.boss_challenge listData = new st.net.NetBase.boss_challenge();
			listData.fromBinary(reader);
			smeltters_boss.Add(listData);
		}
		ushort lenli_smeltters_boss = reader.Read_ushort();
		li_smeltters_boss = new List<st.net.NetBase.boss_challenge>();
		for(int i_li_smeltters_boss = 0 ; i_li_smeltters_boss < lenli_smeltters_boss ; i_li_smeltters_boss ++)
		{
			st.net.NetBase.boss_challenge listData = new st.net.NetBase.boss_challenge();
			listData.fromBinary(reader);
			li_smeltters_boss.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenunderground_palace_boss = (ushort)underground_palace_boss.Count;
		writer.write_short(lenunderground_palace_boss);
		for(int i_underground_palace_boss = 0 ; i_underground_palace_boss < lenunderground_palace_boss ; i_underground_palace_boss ++)
		{
			st.net.NetBase.boss_challenge listData = underground_palace_boss[i_underground_palace_boss];
			listData.toBinary(writer);
		}
		ushort lenscene_boss = (ushort)scene_boss.Count;
		writer.write_short(lenscene_boss);
		for(int i_scene_boss = 0 ; i_scene_boss < lenscene_boss ; i_scene_boss ++)
		{
			st.net.NetBase.boss_challenge listData = scene_boss[i_scene_boss];
			listData.toBinary(writer);
		}
		ushort lenseal_boss = (ushort)seal_boss.Count;
		writer.write_short(lenseal_boss);
		for(int i_seal_boss = 0 ; i_seal_boss < lenseal_boss ; i_seal_boss ++)
		{
			st.net.NetBase.boss_challenge listData = seal_boss[i_seal_boss];
			listData.toBinary(writer);
		}
		ushort lensmeltters_boss = (ushort)smeltters_boss.Count;
		writer.write_short(lensmeltters_boss);
		for(int i_smeltters_boss = 0 ; i_smeltters_boss < lensmeltters_boss ; i_smeltters_boss ++)
		{
			st.net.NetBase.boss_challenge listData = smeltters_boss[i_smeltters_boss];
			listData.toBinary(writer);
		}
		ushort lenli_smeltters_boss = (ushort)li_smeltters_boss.Count;
		writer.write_short(lenli_smeltters_boss);
		for(int i_li_smeltters_boss = 0 ; i_li_smeltters_boss < lenli_smeltters_boss ; i_li_smeltters_boss ++)
		{
			st.net.NetBase.boss_challenge listData = li_smeltters_boss[i_li_smeltters_boss];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
