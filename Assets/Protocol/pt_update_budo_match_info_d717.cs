using System.Collections;
using System.Collections.Generic;

public class pt_update_budo_match_info_d717 : st.net.NetBase.Pt {
	public pt_update_budo_match_info_d717()
	{
		Id = 0xD717;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_budo_match_info_d717();
	}
	public string name;
	public uint time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		name = reader.Read_str();
		time = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(name);
		writer.write_int(time);
		return writer.data;
	}

}
