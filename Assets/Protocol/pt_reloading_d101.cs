using System.Collections;
using System.Collections.Generic;

public class pt_reloading_d101 : st.net.NetBase.Pt {
	public pt_reloading_d101()
	{
		Id = 0xD101;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reloading_d101();
	}
	public int uid;
	public List<st.net.NetBase.equip_id_state_list> equip_id_state_list = new List<st.net.NetBase.equip_id_state_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		ushort lenequip_id_state_list = reader.Read_ushort();
		equip_id_state_list = new List<st.net.NetBase.equip_id_state_list>();
		for(int i_equip_id_state_list = 0 ; i_equip_id_state_list < lenequip_id_state_list ; i_equip_id_state_list ++)
		{
			st.net.NetBase.equip_id_state_list listData = new st.net.NetBase.equip_id_state_list();
			listData.fromBinary(reader);
			equip_id_state_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		ushort lenequip_id_state_list = (ushort)equip_id_state_list.Count;
		writer.write_short(lenequip_id_state_list);
		for(int i_equip_id_state_list = 0 ; i_equip_id_state_list < lenequip_id_state_list ; i_equip_id_state_list ++)
		{
			st.net.NetBase.equip_id_state_list listData = equip_id_state_list[i_equip_id_state_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
