using System.Collections;
using System.Collections.Generic;

public class pt_magic_weapons_state_d309 : st.net.NetBase.Pt {
	public pt_magic_weapons_state_d309()
	{
		Id = 0xD309;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_magic_weapons_state_d309();
	}
	public List<st.net.NetBase.magic_weapons_state> magic_weapons = new List<st.net.NetBase.magic_weapons_state>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmagic_weapons = reader.Read_ushort();
		magic_weapons = new List<st.net.NetBase.magic_weapons_state>();
		for(int i_magic_weapons = 0 ; i_magic_weapons < lenmagic_weapons ; i_magic_weapons ++)
		{
			st.net.NetBase.magic_weapons_state listData = new st.net.NetBase.magic_weapons_state();
			listData.fromBinary(reader);
			magic_weapons.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmagic_weapons = (ushort)magic_weapons.Count;
		writer.write_short(lenmagic_weapons);
		for(int i_magic_weapons = 0 ; i_magic_weapons < lenmagic_weapons ; i_magic_weapons ++)
		{
			st.net.NetBase.magic_weapons_state listData = magic_weapons[i_magic_weapons];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
