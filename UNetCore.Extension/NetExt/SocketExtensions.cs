using System.Net.Sockets;

public static class SocketExtensions
{
    /// <summary>
    /// 是否连接
    /// </summary>
    /// <param name="socket"></param>
    /// <returns></returns>
    public static bool IsConnected(this Socket socket)
    {
        var part1 = socket.Poll(1000, SelectMode.SelectRead);
        var part2 = (socket.Available == 0);

        return part1 & part2;
    }
}