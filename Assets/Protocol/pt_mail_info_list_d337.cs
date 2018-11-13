using System.Collections;
using System.Collections.Generic;

public class pt_mail_info_list_d337 : st.net.NetBase.Pt {
	public pt_mail_info_list_d337()
	{
		Id = 0xD337;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mail_info_list_d337();
	}
	public List<st.net.NetBase.mail_info_list> infos = new List<st.net.NetBase.mail_info_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort leninfos = reader.Read_ushort();
		infos = new List<st.net.NetBase.mail_info_list>();
		for(int i_infos = 0 ; i_infos < leninfos ; i_infos ++)
		{
			st.net.NetBase.mail_info_list listData = new st.net.NetBase.mail_info_list();
			listData.fromBinary(reader);
			infos.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort leninfos = (ushort)infos.Count;
		writer.write_short(leninfos);
		for(int i_infos = 0 ; i_infos < leninfos ; i_infos ++)
		{
			st.net.NetBase.mail_info_list listData = infos[i_infos];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
