using System.Collections;
using System.Collections.Generic;

public class pt_update_copy_property_percentage_d777 : st.net.NetBase.Pt {
	public pt_update_copy_property_percentage_d777()
	{
		Id = 0xD777;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_copy_property_percentage_d777();
	}
	public int percentage;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		percentage = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(percentage);
		return writer.data;
	}

}
