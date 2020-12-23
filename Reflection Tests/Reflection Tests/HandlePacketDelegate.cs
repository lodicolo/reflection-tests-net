namespace Reflection_Tests
{
    public delegate bool HandlePacketDelegate<TPacket>(PacketSender packetSender, TPacket packet) where TPacket : IPacket;
}