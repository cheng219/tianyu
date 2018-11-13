using System.Collections;
using System.Collections.Generic;

public class pt_revive_d205 : st.net.NetBase.Pt {
	public pt_revive_d205()
	{
		Id = 0xD205;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_revive_d205();
	}
	public uint revive_uid;
	public uint revive_sort;
	public float x;
	public float y;
	public float z;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		revive_uid = reader.Read_uint();
		revive_sort = reader.Read_uint();
		x = reader.Read_float();
		y = reader.Read_float();
		z = reader.Read_float();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(revive_uid);
		writer.write_int(revive_sort);
		writer.write_float(x);
		writer.write_float(y);
		writer.write_float(z);
		return writer.data;
	}

}
