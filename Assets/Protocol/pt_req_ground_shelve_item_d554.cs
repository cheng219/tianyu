using System.Collections;
using System.Collections.Generic;

public class pt_req_ground_shelve_item_d554 : st.net.NetBase.Pt {
	public pt_req_ground_shelve_item_d554()
	{
		Id = 0xD554;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_ground_shelve_item_d554();
	}
	public uint id;
	public byte resource;
	public uint price;
	public uint num;
	public byte broadcast;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		resource = reader.Read_byte();
		price = reader.Read_uint();
		num = reader.Read_uint();
		broadcast = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_byte(resource);
		writer.write_int(price);
		writer.write_int(num);
		writer.write_byte(broadcast);
		return writer.data;
	}

}
