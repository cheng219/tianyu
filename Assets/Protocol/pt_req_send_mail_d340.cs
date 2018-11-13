using System.Collections;
using System.Collections.Generic;

public class pt_req_send_mail_d340 : st.net.NetBase.Pt {
	public pt_req_send_mail_d340()
	{
		Id = 0xD340;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_send_mail_d340();
	}
	public string receive_name;
	public string title;
	public string content;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		receive_name = reader.Read_str();
		title = reader.Read_str();
		content = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(receive_name);
		writer.write_str(title);
		writer.write_str(content);
		return writer.data;
	}

}
