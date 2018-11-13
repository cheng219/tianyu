using System.Collections;
using System.Collections.Generic;

public class pt_req_mail_d20a : st.net.NetBase.Pt {
	public pt_req_mail_d20a()
	{
		Id = 0xD20A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_mail_d20a();
	}
	public List<st.net.NetBase.mail_list> mails = new List<st.net.NetBase.mail_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmails = reader.Read_ushort();
		mails = new List<st.net.NetBase.mail_list>();
		for(int i_mails = 0 ; i_mails < lenmails ; i_mails ++)
		{
			st.net.NetBase.mail_list listData = new st.net.NetBase.mail_list();
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
			st.net.NetBase.mail_list listData = mails[i_mails];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
