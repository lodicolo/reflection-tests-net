using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection_Tests
{
    public class DelegatePacketHandlerRegistry : PacketHandlerRegistry<HandlePacketGeneric, MethodInfo>
    {
        #region Overrides of PacketHandlerRegistry<Delegate,MethodInfo>

        protected override IEnumerable<MethodInfo> FindHandlers(Assembly assembly)
        {
            var assemblyTypes = assembly.GetTypes();
            var methodInfos = assemblyTypes.SelectMany(assemblyType => assemblyType.GetMethods().Where(methodInfo =>
                Attribute.IsDefined(methodInfo, typeof(PacketHandlerMethodAttribute)) &&
                PacketHandlerMethodAttribute.IsValidPacketHandlerMethod(methodInfo)));

            return methodInfos;
        }

        #endregion

        #region Overrides of PacketHandlerRegistry<object,MethodInfo>

        private static HandlePacketGeneric CreateHandlerDelegate<TPacket>(MethodInfo methodInfo)
            where TPacket : IPacket
        {
            var stronglyTyped =
                Delegate.CreateDelegate(typeof(HandlePacket<TPacket>), methodInfo) as HandlePacket<TPacket>;

            return (PacketSender packetSender, IPacket packet) => stronglyTyped(packetSender, (TPacket) packet);
        }

        protected override KeyValuePair<Type, HandlePacketGeneric> ExtractHandler(MethodInfo handlerMetaType)
        {
            var packetHandlerMethodAttribute =
                Attribute.GetCustomAttribute(handlerMetaType, typeof(PacketHandlerMethodAttribute)) as
                    PacketHandlerMethodAttribute;

            var packetType = packetHandlerMethodAttribute.PacketType;
            var delegateType = typeof(HandlePacket<>).MakeGenericType(packetType);
            var genericDelegateFactory = typeof(DelegatePacketHandlerRegistry).GetMethod(nameof(CreateHandlerDelegate),
                BindingFlags.NonPublic | BindingFlags.Static);

            var typedDelegateFactory = genericDelegateFactory.MakeGenericMethod(packetType);
            var packetHandlerDelegate =
                typedDelegateFactory.Invoke(null, new object[] {handlerMetaType}) as HandlePacketGeneric;

            return new KeyValuePair<Type, HandlePacketGeneric>(packetType, packetHandlerDelegate);
        }

        #endregion

        #region Overrides of PacketHandlerRegistry<object,MethodInfo>

        protected override bool Invoke(HandlePacketGeneric handler, PacketSender packetSender, IPacket packet) =>
            handler(packetSender, packet);

        #endregion
    }
}