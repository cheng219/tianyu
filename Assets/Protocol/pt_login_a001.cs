using System.Collections;
using System.Collections.Generic;

public class pt_login_a001 : st.net.NetBase.Pt {
	public pt_login_a001()
	{
		Id = 0xA001;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_login_a001();
	}
	public string account;
	public string password;
	public byte platform;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		account = reader.Read_str();
		password = reader.Read_str();
		platform = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(account);
		writer.write_str(password);
		writer.write_byte(platform);
		return writer.data;
	}

}
