using System.Collections;
using System.Collections.Generic;

public class pt_req_shelve_items_d549 : st.net.NetBase.Pt {
	public pt_req_shelve_items_d549()
	{
		Id = 0xD549;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_shelve_items_d549();
	}
	public List<int> sort = new List<int>();
	public uint lev;
	public byte color;
	public byte currency;
	public byte prof;
	public byte index;
	public uint page;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lensort = reader.Read_ushort();
		sort = new List<int>();
		for(int i_sort = 0 ; i_sort < lensort ; i_sort ++)
		{
			int listData = reader.Read_int();
			sort.Add(listData);
		}
		lev = reader.Read_uint();
		color = reader.Read_byte();
		currency = reader.Read_byte();
		prof = reader.Read_byte();
		index = reader.Read_byte();
		page = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lensort = (ushort)sort.Count;
		writer.write_short(lensort);
		for(int i_sort = 0 ; i_sort < lensort ; i_sort ++)
		{
			int listData = sort[i_sort];
			writer.write_int(listData);
		}
		writer.write_int(lev);
		writer.write_byte(color);
		writer.write_byte(currency);
		writer.write_byte(prof);
		writer.write_byte(index);
		writer.write_int(page);
		return writer.data;
	}

}
