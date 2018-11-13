using System.Collections;
using System.Collections.Generic;

public class pt_del_mail_d20d : st.net.NetBase.Pt {
	public pt_del_mail_d20d()
	{
		Id = 0xD20D;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_del_mail_d20d();
	}
	public List<st.net.NetBase.id_list> mails = new List<st.net.NetBase.id_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmails = reader.Read_ushort();
		mails = new List<st.net.NetBase.id_list>();
		for(int i_mails = 0 ; i_mails < lenmails ; i_mails ++)
		{
			st.net.NetBase.id_list listData = new st.net.NetBase.id_list();
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
			st.net.NetBase.id_list listData = mails[i_mails];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
