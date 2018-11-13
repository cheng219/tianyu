using System.Collections;
using System.Collections.Generic;

public class pt_scene_remove_buff_c009 : st.net.NetBase.Pt {
	public pt_scene_remove_buff_c009()
	{
		Id = 0xC009;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_remove_buff_c009();
	}
	public uint oid;
	public uint obj_sort;
	public uint buff_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		obj_sort = reader.Read_uint();
		buff_type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_int(obj_sort);
		writer.write_int(buff_type);
		return writer.data;
	}

}
