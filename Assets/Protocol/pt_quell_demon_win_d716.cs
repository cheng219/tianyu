using System.Collections;
using System.Collections.Generic;

public class pt_quell_demon_win_d716 : st.net.NetBase.Pt {
	public pt_quell_demon_win_d716()
	{
		Id = 0xD716;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_quell_demon_win_d716();
	}
	public int time;
	public int integral;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		time = reader.Read_int();
		integral = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(time);
		writer.write_int(integral);
		return writer.data;
	}

}
