using System.Collections;
using System.Collections.Generic;

public class pt_req_recast_d366 : st.net.NetBase.Pt {
	public pt_req_recast_d366()
	{
		Id = 0xD366;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_recast_d366();
	}
	public int equip_id;
	public int action;
	public List<int> locking_list = new List<int>();
	public int quik_buy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		equip_id = reader.Read_int();
		action = reader.Read_int();
		ushort lenlocking_list = reader.Read_ushort();
		locking_list = new List<int>();
		for(int i_locking_list = 0 ; i_locking_list < lenlocking_list ; i_locking_list ++)
		{
			int listData = reader.Read_int();
			locking_list.Add(listData);
		}
		quik_buy = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(equip_id);
		writer.write_int(action);
		ushort lenlocking_list = (ushort)locking_list.Count;
		writer.write_short(lenlocking_list);
		for(int i_locking_list = 0 ; i_locking_list < lenlocking_list ; i_locking_list ++)
		{
			int listData = locking_list[i_locking_list];
			writer.write_int(listData);
		}
		writer.write_int(quik_buy);
		return writer.data;
	}

}
