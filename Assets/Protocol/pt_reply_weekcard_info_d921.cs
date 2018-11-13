using System.Collections;
using System.Collections.Generic;

public class pt_reply_weekcard_info_d921 : st.net.NetBase.Pt {
	public pt_reply_weekcard_info_d921()
	{
		Id = 0xD921;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_weekcard_info_d921();
	}
	public uint rechrage_amount;
	public List<st.net.NetBase.weekcard_info> weekcard_info = new List<st.net.NetBase.weekcard_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rechrage_amount = reader.Read_uint();
		ushort lenweekcard_info = reader.Read_ushort();
		weekcard_info = new List<st.net.NetBase.weekcard_info>();
		for(int i_weekcard_info = 0 ; i_weekcard_info < lenweekcard_info ; i_weekcard_info ++)
		{
			st.net.NetBase.weekcard_info listData = new st.net.NetBase.weekcard_info();
			listData.fromBinary(reader);
			weekcard_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rechrage_amount);
		ushort lenweekcard_info = (ushort)weekcard_info.Count;
		writer.write_short(lenweekcard_info);
		for(int i_weekcard_info = 0 ; i_weekcard_info < lenweekcard_info ; i_weekcard_info ++)
		{
			st.net.NetBase.weekcard_info listData = weekcard_info[i_weekcard_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
