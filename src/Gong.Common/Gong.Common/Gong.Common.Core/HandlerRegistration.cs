﻿using Gong.Common.Core.CQRS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gong.Common.Core
{
    public static class HandlerRegistration
    {
        public static void AddHandlers(this IServiceCollection services, Type type)

        {
            List<Type> handlerTypes = type.Assembly.GetTypes()
                .Where(px => px.GetInterfaces().Any(x => IsHandlerInterface(x)))
                .Where(px => px.Name.EndsWith("Handler"))
                .ToList();

            foreach (var item in handlerTypes)
            {
                Addhandler(services, item);
            }
        }
        public static void AddHandlers(this IServiceCollection services)
        
        {
            List<Type> handlerTypes = typeof(HandlerRegistration).Assembly.GetTypes()
                .Where(px => px.GetInterfaces().Any(x => IsHandlerInterface(x)))
                .Where(px => px.Name.EndsWith("Handler"))
                .ToList();

            foreach (var item in handlerTypes)
            {
                Addhandler(services, item);
            }
        }
        private static void Addhandler(IServiceCollection services, Type type)
        {
            object[] attributes = type.GetCustomAttributes(false);
            List<Type> pipeline = attributes
                .Select(x => ToDecorator(x))
                .Concat(new[] { type })
                .Reverse()
                .ToList();

            Type interfaceType = type.GetInterfaces().Single(px => IsHandlerInterface(px));
            Func<IServiceProvider, object> factory = BuildPipline(pipeline, interfaceType);

            services.AddTransient(interfaceType, factory);
                
        }

        private static Func<IServiceProvider, object> BuildPipline(List<Type> pipeline, Type interfaceType)
        {
            List<ConstructorInfo> ctors = pipeline
                .Select(px =>
                {
                    Type type = px.IsGenericType ? px.MakeGenericType(interfaceType.GenericTypeArguments) : px;
                    return type.GetConstructors().Single();
                })
            .ToList();

            Func<IServiceProvider, object> func = provider =>
            {
                object current = null;
                foreach (var ctor in ctors)
                {
                    List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();
                    object[] parameters = GetParameters(parameterInfos, current, provider);

                    current = ctor.Invoke(parameters);
                }
                return current;
            };
            return func;
        }
        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];
            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameters(parameterInfos[i], current, provider);
            }
            return result;
        }

        private static object GetParameters(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            Type parameterType = parameterInfo.ParameterType;
            if (IsHandlerInterface(parameterType))
                return current;

            object service = provider.GetService(parameterType);
            if (service != null) return service;

            throw new ArgumentException($"Type: {parameterType} not found");
        }

        private static Type ToDecorator(object attributes)
        {
            Type type = attributes.GetType();

            throw new Exception();
            //if (type == typeof("")))
        }
        public static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;
            Type typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
        }
    }
}
