using System.Collections;
using System.Collections.Generic;

public class pt_update_monster_die_num_d496 : st.net.NetBase.Pt {
	public pt_update_monster_die_num_d496()
	{
		Id = 0xD496;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_monster_die_num_d496();
	}
	public int monster_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		monster_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(monster_num);
		return writer.data;
	}

}
