using System.Collections;
using System.Collections.Generic;

public class pt_update_mail_d211 : st.net.NetBase.Pt {
	public pt_update_mail_d211()
	{
		Id = 0xD211;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_mail_d211();
	}
	public List<st.net.NetBase.update_mails> update_data = new List<st.net.NetBase.update_mails>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenupdate_data = reader.Read_ushort();
		update_data = new List<st.net.NetBase.update_mails>();
		for(int i_update_data = 0 ; i_update_data < lenupdate_data ; i_update_data ++)
		{
			st.net.NetBase.update_mails listData = new st.net.NetBase.update_mails();
			listData.fromBinary(reader);
			update_data.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenupdate_data = (ushort)update_data.Count;
		writer.write_short(lenupdate_data);
		for(int i_update_data = 0 ; i_update_data < lenupdate_data ; i_update_data ++)
		{
			st.net.NetBase.update_mails listData = update_data[i_update_data];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
