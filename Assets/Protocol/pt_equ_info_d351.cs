using System.Collections;
using System.Collections.Generic;

public class pt_equ_info_d351 : st.net.NetBase.Pt {
	public pt_equ_info_d351()
	{
		Id = 0xD351;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_equ_info_d351();
	}
	public List<st.net.NetBase.item_des> equ_list = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenequ_list = reader.Read_ushort();
		equ_list = new List<st.net.NetBase.item_des>();
		for(int i_equ_list = 0 ; i_equ_list < lenequ_list ; i_equ_list ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			equ_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenequ_list = (ushort)equ_list.Count;
		writer.write_short(lenequ_list);
		for(int i_equ_list = 0 ; i_equ_list < lenequ_list ; i_equ_list ++)
		{
			st.net.NetBase.item_des listData = equ_list[i_equ_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
