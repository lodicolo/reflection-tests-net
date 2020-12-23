using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection_Tests
{
    public class MethodInfoPacketHandlerRegistry : PacketHandlerRegistry<MethodInfo, MethodInfo>
    {
        #region Overrides of PacketHandlerRegistry<MethodInfo,MethodInfo>

        protected override IEnumerable<MethodInfo> FindHandlers(Assembly assembly)
        {
            var assemblyTypes = assembly.GetTypes();
            var methodInfos = assemblyTypes.SelectMany(assemblyType =>
                assemblyType.GetMethods().Where(methodInfo =>
                    Attribute.IsDefined(methodInfo, typeof(PacketHandlerMethodAttribute)) && PacketHandlerMethodAttribute.IsValidPacketHandlerMethod(methodInfo)));
            return methodInfos;
        }

        #endregion

        #region Overrides of PacketHandlerRegistry<MethodInfo,MethodInfo>

        protected override KeyValuePair<Type, MethodInfo> ExtractHandler(MethodInfo handlerMetaType)
        {
            var packetType = PacketHandlerMethodAttribute.GetPacketParameterType(handlerMetaType);
            return new KeyValuePair<Type, MethodInfo>(packetType, handlerMetaType);
        }

        #endregion

        #region Overrides of PacketHandlerRegistry<MethodInfo,MethodInfo>

        protected override bool Invoke(MethodInfo handler, PacketSender packetSender, IPacket packet) => (bool)handler.Invoke(null, new object[] { packetSender, packet });

        #endregion
    }
}