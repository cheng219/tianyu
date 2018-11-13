using System.Collections;
using System.Collections.Generic;

public class pt_req_chat_to_world_d318 : st.net.NetBase.Pt {
	public pt_req_chat_to_world_d318()
	{
		Id = 0xD318;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_chat_to_world_d318();
	}
	public uint type;
	public string target_name;
	public int scene;
	public int x;
	public int y;
	public int z;
	public int item_id;
	public int item_type;
	public string content;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_uint();
		target_name = reader.Read_str();
		scene = reader.Read_int();
		x = reader.Read_int();
		y = reader.Read_int();
		z = reader.Read_int();
		item_id = reader.Read_int();
		item_type = reader.Read_int();
		content = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_str(target_name);
		writer.write_int(scene);
		writer.write_int(x);
		writer.write_int(y);
		writer.write_int(z);
		writer.write_int(item_id);
		writer.write_int(item_type);
		writer.write_str(content);
		return writer.data;
	}

}
