using System.Collections;
using System.Collections.Generic;

public class pt_update_monster_wave_num_d493 : st.net.NetBase.Pt {
	public pt_update_monster_wave_num_d493()
	{
		Id = 0xD493;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_monster_wave_num_d493();
	}
	public int wave_num;
	public int max_wave;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		wave_num = reader.Read_int();
		max_wave = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(wave_num);
		writer.write_int(max_wave);
		return writer.data;
	}

}
