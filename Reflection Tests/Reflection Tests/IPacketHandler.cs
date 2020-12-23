namespace Reflection_Tests
{
    public interface IPacketHandler
    {
        bool Handle(PacketSender packetSender, IPacket packet);
    }

    public interface IPacketHandler<TPacket> : IPacketHandler where TPacket : IPacket
    {
        bool Handle(PacketSender packetSender, TPacket packet);
    }
}