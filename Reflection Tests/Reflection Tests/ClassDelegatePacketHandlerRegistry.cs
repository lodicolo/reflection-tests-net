using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection_Tests
{
    public class ClassDelegatePacketHandlerRegistry : PacketHandlerRegistry<HandlePacketGeneric, Type>
    {
        #region Overrides of PacketHandlerRegistry<HandlePacketGeneric,Type>

        protected override IEnumerable<Type> FindHandlers(Assembly assembly)
        {
            var assemblyTypes = assembly.GetTypes();
            var packetHandlerTypes = assemblyTypes.Where(type =>
                Attribute.IsDefined(type, typeof(PacketHandlerClassAttribute)) &&
                PacketHandlerClassAttribute.IsValidPacketHandlerClass(type));

            return packetHandlerTypes;
        }

        private static HandlePacketGeneric CreateHandlerDelegate<TPacket>(IPacketHandler packetHandlerGeneric)
            where TPacket : IPacket
        {
            var stronglyTypedPacketHandler = packetHandlerGeneric as IPacketHandler<TPacket>;
            return (PacketSender packetSender, IPacket packet) =>
                stronglyTypedPacketHandler.Handle(packetSender, (TPacket) packet);
        }

        protected override KeyValuePair<Type, HandlePacketGeneric> ExtractHandler(Type handlerMetaType)
        {
            var packetType = PacketHandlerClassAttribute.FindPacketHandlerGenericInterface(handlerMetaType)
                .GenericTypeArguments[0];

            var genericDelegateFactory = GetType().GetMethod(nameof(CreateHandlerDelegate),
                BindingFlags.NonPublic | BindingFlags.Static);

            var typedDelegateFactory = genericDelegateFactory.MakeGenericMethod(packetType);
            var packetHandler = Activator.CreateInstance(handlerMetaType) as IPacketHandler;
            var instance = typedDelegateFactory.Invoke(null, new object[] {packetHandler});
            var packetHandlerDelegate = instance as HandlePacketGeneric;

            return new KeyValuePair<Type, HandlePacketGeneric>(packetType, packetHandlerDelegate);
        }

        protected override bool Invoke(HandlePacketGeneric handler, PacketSender packetSender, IPacket packet) =>
            handler(packetSender, packet);

        #endregion
    }
}