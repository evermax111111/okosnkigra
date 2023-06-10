using ExitGames.Client.Photon;
using System.Runtime.CompilerServices;

public static class CustomStream
{
	public static readonly byte[] memDamageInfo = new byte[4];

	[CompilerGenerated]
	private static SerializeStreamMethod _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static DeserializeStreamMethod _003C_003Ef__mg_0024cache1;

	public static void Register()
	{
		PhotonPeer.RegisterType(typeof(GamePlayer), 90, SerializeDamageInfo, DeserializeDamageInfo);
	}

	private static short SerializeDamageInfo(StreamBuffer outStream, object customObject)
	{
		GamePlayer gamePlayer = (GamePlayer)customObject;
		lock (memDamageInfo)
		{
			int targetOffset = 0;
			byte[] array = memDamageInfo;
			Protocol.Serialize(gamePlayer.ID, array, ref targetOffset);
			outStream.Write(array, 0, memDamageInfo.Length);
		}
		return (short)memDamageInfo.Length;
	}

	private static object DeserializeDamageInfo(StreamBuffer inStream, short length)
	{
		int value = 0;
		lock (memDamageInfo)
		{
			int offset = 0;
			inStream.Read(memDamageInfo, 0, memDamageInfo.Length);
			Protocol.Deserialize(out value, memDamageInfo, ref offset);
		}
		return new GamePlayer(value);
	}
}
