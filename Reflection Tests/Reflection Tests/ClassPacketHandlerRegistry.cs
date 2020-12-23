using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection_Tests
{
    public class ClassPacketHandlerRegistry : PacketHandlerRegistry<IPacketHandler, Type>
    {
        #region Overrides of PacketHandlerRegistry<IPacketHandler,Type>

        protected override IEnumerable<Type> FindHandlers(Assembly assembly)
        {
            var assemblyTypes = assembly.GetTypes();
            var packetHandlerTypes = assemblyTypes.Where(type =>
                Attribute.IsDefined(type, typeof(PacketHandlerClassAttribute)) &&
                PacketHandlerClassAttribute.IsValidPacketHandlerClass(type));

            return packetHandlerTypes;
        }

        protected override KeyValuePair<Type, IPacketHandler> ExtractHandler(Type handlerMetaType)
        {
            var packetType = PacketHandlerClassAttribute.FindPacketHandlerGenericInterface(handlerMetaType)
                .GenericTypeArguments[0];

            var packetHandler = Activator.CreateInstance(handlerMetaType) as IPacketHandler;
            return new KeyValuePair<Type, IPacketHandler>(packetType, packetHandler);
        }

        protected override bool Invoke(IPacketHandler handler, PacketSender packetSender, IPacket packet) =>
            handler.Handle(packetSender, packet);

        #endregion
    }
}