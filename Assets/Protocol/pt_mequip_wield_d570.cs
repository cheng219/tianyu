using System.Collections;
using System.Collections.Generic;

public class pt_mequip_wield_d570 : st.net.NetBase.Pt {
	public pt_mequip_wield_d570()
	{
		Id = 0xD570;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mequip_wield_d570();
	}
	public uint item_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		item_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(item_id);
		return writer.data;
	}

}
