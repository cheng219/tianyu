using System.Collections;
using System.Collections.Generic;

public class pt_copy_win_d20e : st.net.NetBase.Pt {
	public pt_copy_win_d20e()
	{
		Id = 0xD20E;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_win_d20e();
	}
	public int winorlose;
	public int coin;
	public int exp;
	public int damage;
	public int totalexp;
	public int totalcoin;
	public List<st.net.NetBase.item_list> items = new List<st.net.NetBase.item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		winorlose = reader.Read_int();
		coin = reader.Read_int();
		exp = reader.Read_int();
		damage = reader.Read_int();
		totalexp = reader.Read_int();
		totalcoin = reader.Read_int();
		ushort lenitems = reader.Read_ushort();
		items = new List<st.net.NetBase.item_list>();
		for(int i_items = 0 ; i_items < lenitems ; i_items ++)
		{
			st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
			listData.fromBinary(reader);
			items.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(winorlose);
		writer.write_int(coin);
		writer.write_int(exp);
		writer.write_int(damage);
		writer.write_int(totalexp);
		writer.write_int(totalcoin);
		ushort lenitems = (ushort)items.Count;
		writer.write_short(lenitems);
		for(int i_items = 0 ; i_items < lenitems ; i_items ++)
		{
			st.net.NetBase.item_list listData = items[i_items];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
