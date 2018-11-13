using System.Collections;
using System.Collections.Generic;

public class pt_wsorn_brother_info_d540 : st.net.NetBase.Pt {
	public pt_wsorn_brother_info_d540()
	{
		Id = 0xD540;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_wsorn_brother_info_d540();
	}
	public int uid;
	public int brothers_frendship_lev;
	public int brothers_frendship_num;
	public string brothers_frendship_oath;
	public int brothers_frendship_integer;
	public List<st.net.NetBase.brothers_list> brothers_info = new List<st.net.NetBase.brothers_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_int();
		brothers_frendship_lev = reader.Read_int();
		brothers_frendship_num = reader.Read_int();
		brothers_frendship_oath = reader.Read_str();
		brothers_frendship_integer = reader.Read_int();
		ushort lenbrothers_info = reader.Read_ushort();
		brothers_info = new List<st.net.NetBase.brothers_list>();
		for(int i_brothers_info = 0 ; i_brothers_info < lenbrothers_info ; i_brothers_info ++)
		{
			st.net.NetBase.brothers_list listData = new st.net.NetBase.brothers_list();
			listData.fromBinary(reader);
			brothers_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_int(brothers_frendship_lev);
		writer.write_int(brothers_frendship_num);
		writer.write_str(brothers_frendship_oath);
		writer.write_int(brothers_frendship_integer);
		ushort lenbrothers_info = (ushort)brothers_info.Count;
		writer.write_short(lenbrothers_info);
		for(int i_brothers_info = 0 ; i_brothers_info < lenbrothers_info ; i_brothers_info ++)
		{
			st.net.NetBase.brothers_list listData = brothers_info[i_brothers_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
