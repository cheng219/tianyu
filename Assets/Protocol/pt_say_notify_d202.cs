using System.Collections;
using System.Collections.Generic;

public class pt_say_notify_d202 : st.net.NetBase.Pt {
	public pt_say_notify_d202()
	{
		Id = 0xD202;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_say_notify_d202();
	}
	public uint target_id;
	public uint say_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		target_id = reader.Read_uint();
		say_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(target_id);
		writer.write_int(say_id);
		return writer.data;
	}

}
