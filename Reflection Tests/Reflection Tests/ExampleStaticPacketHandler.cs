namespace Reflection_Tests
{
    public class ExampleStaticPacketHandler
    {
        [PacketHandlerMethod(typeof(ExamplePacket))]
        public static bool Handle(PacketSender packetSender, ExamplePacket packet) => true;
    }
}