using System.Collections;
using System.Collections.Generic;

public class pt_monster_affiliation_c01b : st.net.NetBase.Pt {
	public pt_monster_affiliation_c01b()
	{
		Id = 0xC01B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_monster_affiliation_c01b();
	}
	public List<st.net.NetBase.monster_affiliation> monster_affiliation = new List<st.net.NetBase.monster_affiliation>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmonster_affiliation = reader.Read_ushort();
		monster_affiliation = new List<st.net.NetBase.monster_affiliation>();
		for(int i_monster_affiliation = 0 ; i_monster_affiliation < lenmonster_affiliation ; i_monster_affiliation ++)
		{
			st.net.NetBase.monster_affiliation listData = new st.net.NetBase.monster_affiliation();
			listData.fromBinary(reader);
			monster_affiliation.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmonster_affiliation = (ushort)monster_affiliation.Count;
		writer.write_short(lenmonster_affiliation);
		for(int i_monster_affiliation = 0 ; i_monster_affiliation < lenmonster_affiliation ; i_monster_affiliation ++)
		{
			st.net.NetBase.monster_affiliation listData = monster_affiliation[i_monster_affiliation];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
