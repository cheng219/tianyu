using System.Collections;
using System.Collections.Generic;

public class pt_change_many_copy_difficulty_d462 : st.net.NetBase.Pt {
	public pt_change_many_copy_difficulty_d462()
	{
		Id = 0xD462;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_change_many_copy_difficulty_d462();
	}
	public uint copy_id;
	public uint copy_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_uint();
		copy_type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(copy_type);
		return writer.data;
	}

}
