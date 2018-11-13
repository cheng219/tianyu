using System.Collections;
using System.Collections.Generic;

public class pt_scene_chg_buff_c008 : st.net.NetBase.Pt {
	public pt_scene_chg_buff_c008()
	{
		Id = 0xC008;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_chg_buff_c008();
	}
	public uint oid;
	public uint obj_sort;
	public uint buff_type;
	public uint buff_power;
	public uint buff_mix_lev;
	public uint buff_len;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		obj_sort = reader.Read_uint();
		buff_type = reader.Read_uint();
		buff_power = reader.Read_uint();
		buff_mix_lev = reader.Read_uint();
		buff_len = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_int(obj_sort);
		writer.write_int(buff_type);
		writer.write_int(buff_power);
		writer.write_int(buff_mix_lev);
		writer.write_int(buff_len);
		return writer.data;
	}

}
