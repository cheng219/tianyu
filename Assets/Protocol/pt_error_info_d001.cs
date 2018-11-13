using System.Collections;
using System.Collections.Generic;

public class pt_error_info_d001 : st.net.NetBase.Pt {
	public pt_error_info_d001()
	{
		Id = 0xD001;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_error_info_d001();
	}
	public int error;
	public List<st.net.NetBase.normal_info> msg = new List<st.net.NetBase.normal_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		error = reader.Read_int();
		ushort lenmsg = reader.Read_ushort();
		msg = new List<st.net.NetBase.normal_info>();
		for(int i_msg = 0 ; i_msg < lenmsg ; i_msg ++)
		{
			st.net.NetBase.normal_info listData = new st.net.NetBase.normal_info();
			listData.fromBinary(reader);
			msg.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(error);
		ushort lenmsg = (ushort)msg.Count;
		writer.write_short(lenmsg);
		for(int i_msg = 0 ; i_msg < lenmsg ; i_msg ++)
		{
			st.net.NetBase.normal_info listData = msg[i_msg];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
