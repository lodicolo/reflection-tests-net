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
            ClassDelegatePacketHandlers();
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

            var sw = Stopwatch.StartNew();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register class handlers.");
            }

            sw.Stop();

            Console.WriteLine($"{nameof(ClassPacketHandlers)}: Registration took {sw.Elapsed.TotalMilliseconds}ms");

            sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(ClassPacketHandlers)}: Invocation took {sw.Elapsed.TotalMilliseconds}ms");
        }

        static void ClassDelegatePacketHandlers()
        {
            var sender = new PacketSender();
            var packet = new ExamplePacket();
            var registry = new ClassDelegatePacketHandlerRegistry();

            var sw = Stopwatch.StartNew();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register class delegate handlers.");
            }

            sw.Stop();

            Console.WriteLine($"{nameof(ClassDelegatePacketHandlers)}: Registration took {sw.Elapsed.TotalMilliseconds}ms");

            sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(ClassDelegatePacketHandlers)}: Invocation took {sw.Elapsed.TotalMilliseconds}ms");
        }

        static void DelegatePacketHandlers()
        {
            var sender = new PacketSender();
            var packet = new ExamplePacket();

            var sw = Stopwatch.StartNew();
            var registry = new DelegatePacketHandlerRegistry();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register delegate handlers.");
            }

            sw.Stop();

            Console.WriteLine($"{nameof(DelegatePacketHandlers)}: Registration took {sw.Elapsed.TotalMilliseconds}ms");

            sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(DelegatePacketHandlers)}: Invocation took {sw.Elapsed.TotalMilliseconds}ms");
        }

        static void MethodInfoPacketHandlers()
        {
            var sender = new PacketSender();
            var packet = new ExamplePacket();

            var sw = Stopwatch.StartNew();
            var registry = new MethodInfoPacketHandlerRegistry();
            if (!registry.TryRegisterHandlers(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("Failed to register method handlers.");
            }

            sw.Stop();

            Console.WriteLine($"{nameof(MethodInfoPacketHandlers)}: Registration took {sw.Elapsed.TotalMilliseconds}ms");

            sw = Stopwatch.StartNew();
            for (var itr = 0; itr < 10000000; ++itr)
            {
                registry.Handle(sender, packet);
            }

            sw.Stop();

            Console.WriteLine($"{nameof(MethodInfoPacketHandlers)}: Invocation took {sw.Elapsed.TotalMilliseconds}ms");
        }
    }
}