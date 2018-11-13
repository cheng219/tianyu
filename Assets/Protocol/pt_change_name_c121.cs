using System.Collections;
using System.Collections.Generic;

public class pt_change_name_c121 : st.net.NetBase.Pt {
	public pt_change_name_c121()
	{
		Id = 0xC121;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_change_name_c121();
	}
	public int id;
	public string new_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		new_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_str(new_name);
		return writer.data;
	}

}
