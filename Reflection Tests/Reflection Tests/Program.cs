using System;
using System.Diagnostics;
using System.Reflection;

namespace Reflection_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassPacketHandlers();
            DelegatePacketHandlers();
            MethodInfoPacketHandlers();
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        static void ClassPacketHandlers()
        {
            var sender = new PacketSender();
            var packet = new ExamplePacket();
            var registry = new ClassPacketHandlerRegistry();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register class handlers.");
            }

            var sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(ClassPacketHandlers)}: {sw.Elapsed}");
        }

        static void DelegatePacketHandlers()
        {
            var sender = new PacketSender();
            var packet = new ExamplePacket();
            var registry = new DelegatePacketHandlerRegistry();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register delegate handlers.");
            }

            var sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(DelegatePacketHandlerRegistry)}: {sw.Elapsed}");
        }

        static void MethodInfoPacketHandlers()
        {
            var sender = new PacketSender();
            var packet = new ExamplePacket();
            var registry = new MethodInfoPacketHandlerRegistry();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register method handlers.");
            }

            var sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(MethodInfoPacketHandlers)}: {sw.Elapsed}");
        }
    }
}