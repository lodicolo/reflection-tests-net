namespace Reflection_Tests
{
    [PacketHandlerClass(typeof(ExamplePacket))]
    public class ExamplePacketHandler : IPacketHandler<ExamplePacket>
    {
        #region Implementation of IPacketHandler<ExamplePacket>

        public bool Handle(PacketSender packetSender, ExamplePacket packet) => true;

        #endregion

        #region Implementation of IPacketHandler

        bool IPacketHandler.Handle(PacketSender packetSender, IPacket packet) =>
            Handle(packetSender, packet as ExamplePacket);

        #endregion
    }
}