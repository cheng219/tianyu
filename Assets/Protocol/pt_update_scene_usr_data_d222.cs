using System.Collections;
using System.Collections.Generic;

public class pt_update_scene_usr_data_d222 : st.net.NetBase.Pt {
	public pt_update_scene_usr_data_d222()
	{
		Id = 0xD222;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_scene_usr_data_d222();
	}
	public uint uid;
	public uint sort;
	public string sdata;
	public uint idata;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		sort = reader.Read_uint();
		sdata = reader.Read_str();
		idata = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_int(sort);
		writer.write_str(sdata);
		writer.write_int(idata);
		return writer.data;
	}

}
