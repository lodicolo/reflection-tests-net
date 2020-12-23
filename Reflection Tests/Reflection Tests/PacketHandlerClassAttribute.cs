using System;
using System.Linq;

namespace Reflection_Tests
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketHandlerClassAttribute : Attribute
    {
        public static Type PacketHandlerInterfaceType = typeof(IPacketHandler<>);

        public Type PacketType { get; }

        public PacketHandlerClassAttribute(Type packetType)
        {
            PacketType = packetType;
        }

        public static Type FindPacketHandlerGenericInterface(Type implementationType) => implementationType
            .GetInterfaces().FirstOrDefault(interfaceType =>
                interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == PacketHandlerInterfaceType);

        public static bool IsValidPacketHandlerClass(Type packetHandlerClassType)
        {
            if (packetHandlerClassType.IsInterface || packetHandlerClassType.IsAbstract ||
                packetHandlerClassType.IsGenericType)
            {
                return false;
            }

            return FindPacketHandlerGenericInterface(packetHandlerClassType) != default;
        }
    }
}