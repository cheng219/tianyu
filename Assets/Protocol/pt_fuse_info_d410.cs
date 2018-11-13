using System.Collections;
using System.Collections.Generic;

public class pt_fuse_info_d410 : st.net.NetBase.Pt {
	public pt_fuse_info_d410()
	{
		Id = 0xD410;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_fuse_info_d410();
	}
	public int pet_type;
	public int grow_up_lev;
	public int aptitude_lev;
	public int tian_soul;
	public int di_soul;
	public int life_soul;
	public int aptitude_exp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		pet_type = reader.Read_int();
		grow_up_lev = reader.Read_int();
		aptitude_lev = reader.Read_int();
		tian_soul = reader.Read_int();
		di_soul = reader.Read_int();
		life_soul = reader.Read_int();
		aptitude_exp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(pet_type);
		writer.write_int(grow_up_lev);
		writer.write_int(aptitude_lev);
		writer.write_int(tian_soul);
		writer.write_int(di_soul);
		writer.write_int(life_soul);
		writer.write_int(aptitude_exp);
		return writer.data;
	}

}
