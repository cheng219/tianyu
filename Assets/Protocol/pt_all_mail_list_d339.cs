using System.Collections;
using System.Collections.Generic;

public class pt_all_mail_list_d339 : st.net.NetBase.Pt {
	public pt_all_mail_list_d339()
	{
		Id = 0xD339;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_all_mail_list_d339();
	}
	public List<st.net.NetBase.base_mail_list> mails = new List<st.net.NetBase.base_mail_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmails = reader.Read_ushort();
		mails = new List<st.net.NetBase.base_mail_list>();
		for(int i_mails = 0 ; i_mails < lenmails ; i_mails ++)
		{
			st.net.NetBase.base_mail_list listData = new st.net.NetBase.base_mail_list();
			listData.fromBinary(reader);
			mails.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmails = (ushort)mails.Count;
		writer.write_short(lenmails);
		for(int i_mails = 0 ; i_mails < lenmails ; i_mails ++)
		{
			st.net.NetBase.base_mail_list listData = mails[i_mails];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
