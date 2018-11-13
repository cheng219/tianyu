using System.Collections;
using System.Collections.Generic;

public class pt_return_sign_d105 : st.net.NetBase.Pt {
	public pt_return_sign_d105()
	{
		Id = 0xD105;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_return_sign_d105();
	}
	public List<st.net.NetBase.sign_list> sign_list = new List<st.net.NetBase.sign_list>();
	public List<st.net.NetBase.sign_rewards_list> sign_rewards_list = new List<st.net.NetBase.sign_rewards_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lensign_list = reader.Read_ushort();
		sign_list = new List<st.net.NetBase.sign_list>();
		for(int i_sign_list = 0 ; i_sign_list < lensign_list ; i_sign_list ++)
		{
			st.net.NetBase.sign_list listData = new st.net.NetBase.sign_list();
			listData.fromBinary(reader);
			sign_list.Add(listData);
		}
		ushort lensign_rewards_list = reader.Read_ushort();
		sign_rewards_list = new List<st.net.NetBase.sign_rewards_list>();
		for(int i_sign_rewards_list = 0 ; i_sign_rewards_list < lensign_rewards_list ; i_sign_rewards_list ++)
		{
			st.net.NetBase.sign_rewards_list listData = new st.net.NetBase.sign_rewards_list();
			listData.fromBinary(reader);
			sign_rewards_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lensign_list = (ushort)sign_list.Count;
		writer.write_short(lensign_list);
		for(int i_sign_list = 0 ; i_sign_list < lensign_list ; i_sign_list ++)
		{
			st.net.NetBase.sign_list listData = sign_list[i_sign_list];
			listData.toBinary(writer);
		}
		ushort lensign_rewards_list = (ushort)sign_rewards_list.Count;
		writer.write_short(lensign_rewards_list);
		for(int i_sign_rewards_list = 0 ; i_sign_rewards_list < lensign_rewards_list ; i_sign_rewards_list ++)
		{
			st.net.NetBase.sign_rewards_list listData = sign_rewards_list[i_sign_rewards_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
