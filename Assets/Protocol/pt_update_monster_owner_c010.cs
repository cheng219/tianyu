using System.Collections;
using System.Collections.Generic;

public class pt_update_monster_owner_c010 : st.net.NetBase.Pt {
	public pt_update_monster_owner_c010()
	{
		Id = 0xC010;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_monster_owner_c010();
	}
	public uint monster_id;
	public uint owner_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		monster_id = reader.Read_uint();
		owner_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(monster_id);
		writer.write_int(owner_id);
		return writer.data;
	}

}
