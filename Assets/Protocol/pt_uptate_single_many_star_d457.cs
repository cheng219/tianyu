using System.Collections;
using System.Collections.Generic;

public class pt_uptate_single_many_star_d457 : st.net.NetBase.Pt {
	public pt_uptate_single_many_star_d457()
	{
		Id = 0xD457;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_uptate_single_many_star_d457();
	}
	public int copy_id;
	public int scene_type;
	public int star_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		copy_id = reader.Read_int();
		scene_type = reader.Read_int();
		star_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(copy_id);
		writer.write_int(scene_type);
		writer.write_int(star_num);
		return writer.data;
	}

}
