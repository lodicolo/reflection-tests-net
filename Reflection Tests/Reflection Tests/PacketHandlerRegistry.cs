using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection_Tests
{
    public abstract class PacketHandlerRegistry<THandler, THandlerMetaType>
    {
        private Dictionary<Type, THandler> RegisteredHandlers { get; }

        protected PacketHandlerRegistry()
        {
            RegisteredHandlers = new Dictionary<Type, THandler>();
        }

        protected abstract IEnumerable<THandlerMetaType> FindHandlers(Assembly assembly);

        protected abstract KeyValuePair<Type, THandler> ExtractHandler(THandlerMetaType handlerMetaType);

        public virtual bool TryRegisterHandlers(Assembly assembly)
        {
            var handlerMetaTypes = FindHandlers(assembly);
            foreach (var handlerMetaType in handlerMetaTypes)
            {
                var handler = ExtractHandler(handlerMetaType);

                if (RegisteredHandlers.ContainsKey(handler.Key))
                {
                    throw new Exception();
                }

                RegisteredHandlers[handler.Key] = handler.Value;
            }

            return RegisteredHandlers.Count != 0;
        }

        protected abstract bool Invoke(THandler handler, PacketSender packetSender, IPacket packet);

        public virtual bool Handle(PacketSender packetSender, IPacket packet)
        {
            var packetType = packet.GetType();
            if (!RegisteredHandlers.TryGetValue(packetType, out var handler))
            {
                throw new Exception();
                // return false;
            }

            return Invoke(handler, packetSender, packet);
        }
    }
}