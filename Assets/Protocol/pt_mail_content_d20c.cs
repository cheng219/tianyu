using System.Collections;
using System.Collections.Generic;

public class pt_mail_content_d20c : st.net.NetBase.Pt {
	public pt_mail_content_d20c()
	{
		Id = 0xD20C;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mail_content_d20c();
	}
	public uint mail_id;
	public string title;
	public string content;
	public List<st.net.NetBase.item_list> items = new List<st.net.NetBase.item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		mail_id = reader.Read_uint();
		title = reader.Read_str();
		content = reader.Read_str();
		ushort lenitems = reader.Read_ushort();
		items = new List<st.net.NetBase.item_list>();
		for(int i_items = 0 ; i_items < lenitems ; i_items ++)
		{
			st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
			listData.fromBinary(reader);
			items.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(mail_id);
		writer.write_str(title);
		writer.write_str(content);
		ushort lenitems = (ushort)items.Count;
		writer.write_short(lenitems);
		for(int i_items = 0 ; i_items < lenitems ; i_items ++)
		{
			st.net.NetBase.item_list listData = items[i_items];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
