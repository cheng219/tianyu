using System.Collections;
using System.Collections.Generic;

public class pt_lucky_wheel_record_d963 : st.net.NetBase.Pt {
	public pt_lucky_wheel_record_d963()
	{
		Id = 0xD963;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lucky_wheel_record_d963();
	}
	public List<st.net.NetBase.lucky_wheel_record> record_list = new List<st.net.NetBase.lucky_wheel_record>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrecord_list = reader.Read_ushort();
		record_list = new List<st.net.NetBase.lucky_wheel_record>();
		for(int i_record_list = 0 ; i_record_list < lenrecord_list ; i_record_list ++)
		{
			st.net.NetBase.lucky_wheel_record listData = new st.net.NetBase.lucky_wheel_record();
			listData.fromBinary(reader);
			record_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrecord_list = (ushort)record_list.Count;
		writer.write_short(lenrecord_list);
		for(int i_record_list = 0 ; i_record_list < lenrecord_list ; i_record_list ++)
		{
			st.net.NetBase.lucky_wheel_record listData = record_list[i_record_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
