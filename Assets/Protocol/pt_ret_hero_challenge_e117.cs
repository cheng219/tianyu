using System.Collections;
using System.Collections.Generic;

public class pt_ret_hero_challenge_e117 : st.net.NetBase.Pt {
	public pt_ret_hero_challenge_e117()
	{
		Id = 0xE117;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_hero_challenge_e117();
	}
	public uint mission;
	public uint hp;
	public uint mp;
	public byte times;
	public List<st.net.NetBase.entourage> entourages = new List<st.net.NetBase.entourage>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		mission = reader.Read_uint();
		hp = reader.Read_uint();
		mp = reader.Read_uint();
		times = reader.Read_byte();
		ushort lenentourages = reader.Read_ushort();
		entourages = new List<st.net.NetBase.entourage>();
		for(int i_entourages = 0 ; i_entourages < lenentourages ; i_entourages ++)
		{
			st.net.NetBase.entourage listData = new st.net.NetBase.entourage();
			listData.fromBinary(reader);
			entourages.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(mission);
		writer.write_int(hp);
		writer.write_int(mp);
		writer.write_byte(times);
		ushort lenentourages = (ushort)entourages.Count;
		writer.write_short(lenentourages);
		for(int i_entourages = 0 ; i_entourages < lenentourages ; i_entourages ++)
		{
			st.net.NetBase.entourage listData = entourages[i_entourages];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
