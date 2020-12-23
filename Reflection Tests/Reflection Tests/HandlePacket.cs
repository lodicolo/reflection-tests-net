namespace Reflection_Tests
{
    public delegate bool HandlePacket<TPacket>(PacketSender packetSender, TPacket packet) where TPacket : IPacket;

    public delegate bool HandlePacketGeneric(PacketSender packetSender, IPacket packet);
}