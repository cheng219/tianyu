using System.Collections;
using System.Collections.Generic;

public class pt_update_new_name_c122 : st.net.NetBase.Pt {
	public pt_update_new_name_c122()
	{
		Id = 0xC122;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_new_name_c122();
	}
	public string new_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		new_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(new_name);
		return writer.data;
	}

}
