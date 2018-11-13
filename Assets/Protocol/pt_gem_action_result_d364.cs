using System.Collections;
using System.Collections.Generic;

public class pt_gem_action_result_d364 : st.net.NetBase.Pt {
	public pt_gem_action_result_d364()
	{
		Id = 0xD364;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_gem_action_result_d364();
	}
	public uint id;
	public int action;
	public int result;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
		action = reader.Read_int();
		result = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(action);
		writer.write_int(result);
		return writer.data;
	}

}
