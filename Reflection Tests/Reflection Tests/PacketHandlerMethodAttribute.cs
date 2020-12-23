using System;
using System.Linq;
using System.Reflection;

namespace Reflection_Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketHandlerMethodAttribute : Attribute
    {
        public Type PacketType { get; }

        public PacketHandlerMethodAttribute(Type packetType)
        {
            PacketType = packetType;
        }

        public static Type GetPacketParameterType(MethodInfo methodInfo) => methodInfo.GetParameters()[1].ParameterType;

        public static bool IsValidPacketHandlerMethod(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType != typeof(bool))
            {
                return false;
            }

            var parameterTypes = methodInfo.GetParameters().Select(parameterInfo => parameterInfo.ParameterType)
                .ToArray();

            if (parameterTypes.Length != 2)
            {
                return false;
            }

            return (typeof(PacketSender) == parameterTypes[0]) && typeof(IPacket).IsAssignableFrom(parameterTypes[1]);
        }
    }
}