using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection_Tests
{
    using GenericPacketHandler = Func<PacketSender, IPacket, bool>;

    public class DelegatePacketHandlerRegistry : PacketHandlerRegistry<GenericPacketHandler, MethodInfo>
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

        private static GenericPacketHandler CreateHandlerDelegate<TPacket>(MethodInfo methodInfo)
            where TPacket : IPacket
        {
            var stronglyTyped =
                (Func<PacketSender, TPacket, bool>) Delegate.CreateDelegate(typeof(Func<PacketSender, TPacket, bool>),
                    methodInfo);

            return (PacketSender packetSender, IPacket packet) => stronglyTyped(packetSender, (TPacket) packet);
        }

        protected override KeyValuePair<Type, GenericPacketHandler> ExtractHandler(MethodInfo handlerMetaType)
        {
            var packetHandlerMethodAttribute =
                Attribute.GetCustomAttribute(handlerMetaType, typeof(PacketHandlerMethodAttribute)) as
                    PacketHandlerMethodAttribute;

            var packetType = packetHandlerMethodAttribute.PacketType;
            var delegateType = typeof(HandlePacketDelegate<>).MakeGenericType(packetType);
            var genericDelegateFactory = typeof(DelegatePacketHandlerRegistry).GetMethod(nameof(CreateHandlerDelegate),
                BindingFlags.NonPublic | BindingFlags.Static);

            var typedDelegateFactory = genericDelegateFactory.MakeGenericMethod(packetType);
            var packetHandlerDelegate =
                (GenericPacketHandler) typedDelegateFactory.Invoke(null, new object[] {handlerMetaType});

            return new KeyValuePair<Type, GenericPacketHandler>(packetType, packetHandlerDelegate);
        }

        #endregion

        #region Overrides of PacketHandlerRegistry<object,MethodInfo>

        protected override bool Invoke(GenericPacketHandler handler, PacketSender packetSender, IPacket packet) =>
            handler(packetSender, packet);

        #endregion
    }
}