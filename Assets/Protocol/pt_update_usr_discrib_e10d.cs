using System.Collections;
using System.Collections.Generic;

public class pt_update_usr_discrib_e10d : st.net.NetBase.Pt {
	public pt_update_usr_discrib_e10d()
	{
		Id = 0xE10D;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_usr_discrib_e10d();
	}
	public uint uid;
	public ushort sort;
	public uint int_data;
	public string str_data;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
		sort = reader.Read_ushort();
		int_data = reader.Read_uint();
		str_data = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		writer.write_short(sort);
		writer.write_int(int_data);
		writer.write_str(str_data);
		return writer.data;
	}

}
