using System.Collections;
using System.Collections.Generic;

public class pt_reply_operation_activity_info_d903 : st.net.NetBase.Pt {
	public pt_reply_operation_activity_info_d903()
	{
		Id = 0xD903;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_operation_activity_info_d903();
	}
	public uint id;
	public byte type;
	public uint rest_time;
	public string desc;
	public uint counter_value;
	public List<st.net.NetBase.operation_activity_detail_info> details = new List<st.net.NetBase.operation_activity_detail_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		type = reader.Read_byte();
		rest_time = reader.Read_uint();
		desc = reader.Read_str();
		counter_value = reader.Read_uint();
		ushort lendetails = reader.Read_ushort();
		details = new List<st.net.NetBase.operation_activity_detail_info>();
		for(int i_details = 0 ; i_details < lendetails ; i_details ++)
		{
			st.net.NetBase.operation_activity_detail_info listData = new st.net.NetBase.operation_activity_detail_info();
			listData.fromBinary(reader);
			details.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_byte(type);
		writer.write_int(rest_time);
		writer.write_str(desc);
		writer.write_int(counter_value);
		ushort lendetails = (ushort)details.Count;
		writer.write_short(lendetails);
		for(int i_details = 0 ; i_details < lendetails ; i_details ++)
		{
			st.net.NetBase.operation_activity_detail_info listData = details[i_details];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
