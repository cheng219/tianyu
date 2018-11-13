using System.Collections;
using System.Collections.Generic;

public class pt_scene_move_c001 : st.net.NetBase.Pt {
	public pt_scene_move_c001()
	{
		Id = 0xC001;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_move_c001();
	}
	public uint oid;
	public uint obj_sort;
	public float dir;
	public byte is_path_move;
	public List<st.net.NetBase.point3> point_list = new List<st.net.NetBase.point3>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oid = reader.Read_uint();
		obj_sort = reader.Read_uint();
		dir = reader.Read_float();
		is_path_move = reader.Read_byte();
		ushort lenpoint_list = reader.Read_ushort();
		point_list = new List<st.net.NetBase.point3>();
		for(int i_point_list = 0 ; i_point_list < lenpoint_list ; i_point_list ++)
		{
			st.net.NetBase.point3 listData = new st.net.NetBase.point3();
			listData.fromBinary(reader);
			point_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(oid);
		writer.write_int(obj_sort);
		writer.write_float(dir);
		writer.write_byte(is_path_move);
		ushort lenpoint_list = (ushort)point_list.Count;
		writer.write_short(lenpoint_list);
		for(int i_point_list = 0 ; i_point_list < lenpoint_list ; i_point_list ++)
		{
			st.net.NetBase.point3 listData = point_list[i_point_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
