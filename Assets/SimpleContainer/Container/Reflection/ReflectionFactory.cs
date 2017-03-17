using System;
using System.Collections.Generic;
using System.Reflection;
using Utils;

namespace SimpleContainer.Container
{
    public class ReflectionFactory : IReflectionFactory
    {
        /// <summary>
        /// 返回一个储存了指定类型的反射信息的 ReflectInfo 类
        /// </summary>
        virtual public ReflectionInfo Create(Type type)
        {
            // 新建一个 ReflectionInfo 类
            var reflectionInfo = new ReflectionInfo();

            // 获取 ReflectInfo 类的信息
            reflectionInfo.type = type;
            var constructor = this.GetPreferredConstructor(type);

            // 由于带参数和不带参数的构造函数委托类型不同，将筛选构造函数的过程封装为一个独立的方法将带来不必要的装、拆箱步骤，所以直接在方法内处理
            if (constructor != null)
            {
                if (constructor.GetParameters().Length == 0)
                {
                    reflectionInfo.constructor = MethodUtils.CreateConstructor(type, constructor);
                }
                else {
                    reflectionInfo.paramsConstructor = MethodUtils.CreateConstructorWithParams(type, constructor); 
                }
            }

            reflectionInfo.constructorParameters = GetConstructorParameters(constructor);
            reflectionInfo.methods = GetMethods(type);
            reflectionInfo.properties = GetProperties(type);
            reflectionInfo.fields = GetFields(type);

            return reflectionInfo;
        }

        /// <summary>
        /// 获取首选构造函数信息，优先带有[Inject]特性的构造函数，如果没有，就选择参数最少的构造函数
        /// </summary>
        virtual protected ConstructorInfo GetPreferredConstructor(Type type)
        {
            // 获取类型参数带有指定 BindingFlags 的构造函数
            var constructors = type.GetConstructors(
                BindingFlags.FlattenHierarchy |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.InvokeMethod);

            // 如果没有获取到返回空
            if (constructors.Length == 0) { return null; }
            // 如果只有1个构造函数直接返回该构造函数
            if (constructors.Length == 1) { return constructors[0]; }

            // 如果构造函数多于1个，遍历它们寻找最合适的构造函数
            ConstructorInfo preferredConstructor = null;
            for (int i = 0, length = 0, shortestLength = int.MaxValue; i < constructors.Length; i++)
            {
                var constructor = constructors[i];
                // 获取附着于当前构造函数的[Inject]特性，如果获取到就直接返回当前构造函数
                var attributes = constructor.GetCustomAttributes(typeof(Inject), true);



                if (attributes.Length > 0) { return constructor; }
                
                // 如果没有获取到就比较参数的长度
                length = constructor.GetParameters().Length;
                if (length < shortestLength)
                {
                    shortestLength = length;
                    preferredConstructor = constructor;
                }
            }
            // 返回参数最短的构造函数
            return preferredConstructor;
        }

        /// <summary>
        /// 获取构造函数参数信息类
        /// </summary>
        virtual protected ParameterInfo[] GetConstructorParameters(ConstructorInfo constructor)
        {
            // 如果参数 constructor 为空直接返回
            if (constructor == null) { return null; }

            // 获取构造函数参数
            var parameters = constructor.GetParameters();
            var constructorParameters = new ParameterInfo[parameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++)
            {
                //constructorParameters[i] = parameters[i].ParameterType;

                object id = null;
                var parameter = parameters[i];

                // 获取当前参数所附着的[Inject]特性，如果获取成功，将该特性的 id 作为自己的 id
                var attributes = parameter.GetCustomAttributes(typeof(Inject), true);
                if (attributes.Length > 0) { id = (attributes[0] as Inject).id; }

                constructorParameters[i] = new ParameterInfo(parameter.ParameterType, parameter.Name, id);
            }

            return constructorParameters;
        }

        /// <summary>
        /// 获取指定类型注入后需要执行的方法的信息类
        /// </summary>
        virtual protected MethodInfo[] GetMethods(Type type)
        {
            var parameterlessMethods = new List<MethodInfo>();
            // 获取参数 type 中的方法
            var methods = type.GetMethods (
                BindingFlags.FlattenHierarchy |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);

            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];

                // 获取方法是否附着有[Inject]特性
                var attributes = method.GetCustomAttributes(typeof(Inject), true);
                if (attributes.Length > 0)
                {
                    // 如果获取成功，继续获取方法的参数，并获取它们的 id，用它们构造 ParameterInfo 类
                    var parameters = method.GetParameters();
                    var methodParameters = new ParameterInfo[parameters.Length];
                    for (int n = 0; n < methodParameters.Length; n++)
                    {
                        object id = null;
                        var parameter = parameters[n];

                        var parameterAttributes = parameter.GetCustomAttributes(typeof(Inject), true);
                        if (parameterAttributes.Length > 0)
                        {
                            id = (parameterAttributes[0] as Inject).id;
                        }

                        methodParameters[n] = new ParameterInfo(parameter.ParameterType, parameter.Name, id);
                    }

                    var parameterlessMethod = new MethodInfo(method.Name, methodParameters);

                    // 根据参数个数创建方法委托
                    if (methodParameters.Length == 0)
                    {
                        parameterlessMethod.method = MethodUtils.CreateParameterlessMethod(type, method);
                    }
                    else
                    {
                        parameterlessMethod.paramsMethod = MethodUtils.CreateParameterizedMethod(type, method);
                    }

                    // 将方法委托加入数组
                    parameterlessMethods.Add(parameterlessMethod);
                }
            }

            // 以数组形式返回
            return parameterlessMethods.ToArray();
        }

        /// <summary>
        /// 返回接受注入的属性信息 : type 为 key，object 为 id， PropertyInfo 为值
        /// </summary>
        virtual protected AcessorInfo[] GetProperties(Type type)
        {
            var setters = new List<AcessorInfo>();

            // 获取参数 type 的属性
            var properties = type.GetProperties (
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.NonPublic |
                BindingFlags.Public);

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i] as PropertyInfo;
                var attributes = property.GetCustomAttributes(typeof(Inject), true);

                if (attributes.Length > 0)
                {
                    var attribute = attributes[0] as Inject;
                    var getter = MethodUtils.CreatePropertyGetter(type, property);
                    var setter = MethodUtils.CreatePropertySetter(type, property);
                    var info = new AcessorInfo(property.PropertyType, property.Name, attribute.id, getter, setter);
                    setters.Add(info);
            }
            }

            return setters.ToArray();
        }

        /// <summary>
        /// 返回接受注入的字段信息 : type 为 key，object 为 id， PropertyInfo 为值
        /// </summary>
        virtual protected AcessorInfo[] GetFields(Type type)
        {
            var setters = new List<AcessorInfo>();

            // 获取参数 type 的属性
            var fields = type.GetFields (
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.NonPublic |
                BindingFlags.Public);

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i] as FieldInfo;
                var attributes = field.GetCustomAttributes(typeof(Inject), true);

                if (attributes.Length > 0)
                {
                    var attribute = attributes[0] as Inject;
                    var getter = MethodUtils.CreateFieldGetter(type, field);
                    var setter = MethodUtils.CreateFieldSetter(type, field);
                    var info = new AcessorInfo(field.FieldType, field.Name, attribute.id, getter, setter);
                    setters.Add(info);
                }
            }

            return setters.ToArray();
        }
    }
}