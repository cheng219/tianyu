using System.Collections;
using System.Collections.Generic;

public class pt_donation_record_list_d11b : st.net.NetBase.Pt {
	public pt_donation_record_list_d11b()
	{
		Id = 0xD11B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_donation_record_list_d11b();
	}
	public List<st.net.NetBase.donation_record_list> donation_record_list = new List<st.net.NetBase.donation_record_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lendonation_record_list = reader.Read_ushort();
		donation_record_list = new List<st.net.NetBase.donation_record_list>();
		for(int i_donation_record_list = 0 ; i_donation_record_list < lendonation_record_list ; i_donation_record_list ++)
		{
			st.net.NetBase.donation_record_list listData = new st.net.NetBase.donation_record_list();
			listData.fromBinary(reader);
			donation_record_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lendonation_record_list = (ushort)donation_record_list.Count;
		writer.write_short(lendonation_record_list);
		for(int i_donation_record_list = 0 ; i_donation_record_list < lendonation_record_list ; i_donation_record_list ++)
		{
			st.net.NetBase.donation_record_list listData = donation_record_list[i_donation_record_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
