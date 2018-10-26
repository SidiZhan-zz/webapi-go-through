using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Xunit;
using Xunit.Abstractions;

namespace WebApplication2.Test
{
    public class IoCFacts
    {
        class DatabaseInProduction : IWithName
        {
            public string Name { get; } = typeof(DatabaseInProduction).Name;
        }

        class DatabaseInTest : IWithName
        {
            public string Name { get; } = typeof(DatabaseInTest).Name;
        }

        [Fact]
        public void should_create_objects()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DatabaseInProduction>().As<IWithName>();
            builder.RegisterType<DatabaseInTest>().As<IWithName>();

            var container = builder.Build();
            var withNames = container.Resolve<IEnumerable<IWithName>>().ToArray();
            
            Assert.Equal(2, withNames.Length);
            Assert.True(withNames.Any(item => item.Name == "DatabaseInProduction"));
            Assert.True(withNames.Any(item => item.Name == "DatabaseInTest"));
        }

        class Disposable : IDisposable
        {
            public bool IsDisposed { get; set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        [Fact]
        public void should_manage_life_cycle()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Disposable>();

            var container = builder.Build();

            var disposable = container.Resolve<Disposable>();

            Assert.False(disposable.IsDisposed);

            container.Dispose();

            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void should_create_per_dependency()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<object>();

            var container = builder.Build();

            var instance = container.Resolve<object>();
            var anotherInstance = container.Resolve<object>();
            
            Assert.NotSame(instance, anotherInstance);
        }

        [Fact]
        public void should_create_single_instance()
        {
            var builder = new ContainerBuilder();
            var anotherBuilder = new ContainerBuilder();

            builder.RegisterType<object>().SingleInstance();
            anotherBuilder.RegisterType<object>();

            var container = builder.Build();
            var anotherContainer = anotherBuilder.Build();

            var instance = container.Resolve<object>();
            var anotherInstance = container.Resolve<object>();

            var instanceInAnotherContainer = anotherContainer.Resolve<object>();
            var anotherInstanceInAnotherContainer = anotherContainer.Resolve<object>();

            Assert.Same(instance, anotherInstance);
            Assert.NotSame(instanceInAnotherContainer, anotherInstanceInAnotherContainer);
            Assert.NotSame(instanceInAnotherContainer, instance);
        }

        [Fact]
        public void should_create_lifetimes_scoped_insance()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Disposable>().InstancePerLifetimeScope();

            var container = builder.Build();

            var lifetimeScope = container.BeginLifetimeScope();

            var disposable = lifetimeScope.Resolve<Disposable>();
            var anotherDisposable = lifetimeScope.Resolve<Disposable>();

            Assert.Same(disposable, anotherDisposable);

            var anotherScope = container.BeginLifetimeScope();

            var disposableInOtherScope = anotherScope.Resolve<Disposable>();
            var anotherDisposableInOtherScope = anotherScope.Resolve<Disposable>();

            Assert.Same(anotherDisposableInOtherScope, disposableInOtherScope);
            Assert.NotSame(disposable, disposableInOtherScope);
        }

        [Fact]
        public void should_store_single_instance_object_to_container()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Disposable>().SingleInstance();

            var container = builder.Build();

            var scope = container.BeginLifetimeScope();

            var objectResolvedByScope = scope.Resolve<Disposable>();
            var objectResovledByContainer = container.Resolve<Disposable>();

            Assert.Same(objectResovledByContainer, objectResolvedByScope);

            scope.Dispose();

            Assert.False(objectResolvedByScope.IsDisposed);
        }

        [Fact]
        public void should_answer_the_question()
        {
            var builder = new ContainerBuilder();
            builder.Register(_ => new MyConfig()).As<MyConfig>().SingleInstance();
            builder.Register(_ => new Foo()).As<Foo>().InstancePerDependency();
            builder.RegisterType<Dependence>().InstancePerLifetimeScope();
            builder.RegisterType<WithDependence>().InstancePerDependency();
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();
            var withDependence1 = scope.Resolve<WithDependence>();
            var withDependence2 = scope.Resolve<WithDependence>();
            
            Assert.NotSame(withDependence1, withDependence2);
            Assert.Same(withDependence1.Dependence, withDependence2.Dependence);
            Assert.Same(withDependence1.Dependence.Foo, withDependence2.Dependence.Foo);
            Assert.Same(withDependence1.Dependence.MyConfig, withDependence2.Dependence.MyConfig);
        }
    }

    class MyConfig
    {

    }

    class Foo
    {

    }

    class Dependence
    {
        public MyConfig MyConfig { get; }
        public Foo Foo { get; }
        public Dependence(MyConfig myConfig, Foo foo)
        {
            MyConfig = myConfig;
            Foo = foo;
        }
    }

    class WithDependence
    {
        public Dependence Dependence { get; }

        public WithDependence(Dependence dependence)
        {
            Dependence = dependence;
        }
    }


    interface IWithName
    {
        string Name { get; }
    }
}

