/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace uMVVMCS.DIContainer
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
            reflectionInfo.postConstructors = GetPostConstructors(type);
            reflectionInfo.properties = GetProperties(type);
            reflectionInfo.fields = GetFields(type);

            return reflectionInfo;
        }

        /// <summary>
        /// 获取首选构造函数信息，优先带有[Construct]特性的构造函数，如果没有，就选择参数最少的构造函数
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
                // 获取附着于当前构造函数的[Construct]特性，如果获取到就直接返回当前构造函数
                object[] attributes = constructor.GetCustomAttributes(typeof(Construct), true);
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

                constructorParameters[i] = new ParameterInfo(parameter.ParameterType, id);
            }

            return constructorParameters;
        }

        /// <summary>
        /// 获取指定类型注入后需要执行的方法的信息类
        /// </summary>
        virtual protected PostConstructorInfo[] GetPostConstructors(Type type)
        {
            var postConstructors = new List<PostConstructorInfo>();
            // 获取参数 type 中的方法
            var methods = type.GetMethods (
                BindingFlags.FlattenHierarchy |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);

            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];

                // 获取方法是否附着有[PostConstruct]特性
                var attributes = method.GetCustomAttributes(typeof(PostConstruct), true);
                if (attributes.Length > 0)
                {
                    // 如果获取成功，继续获取方法的参数，并获取它们的 id，用它们构造 ParameterInfo 类
                    var parameters = method.GetParameters();
                    var postConstructorParameters = new ParameterInfo[parameters.Length];
                    for (int n = 0; n < postConstructorParameters.Length; n++)
                    {
                        object id = null;
                        var parameter = parameters[n];

                        var parameterAttributes = parameter.GetCustomAttributes(typeof(Inject), true);
                        if (parameterAttributes.Length > 0)
                        {
                            id = (parameterAttributes[0] as Inject).id;
                        }

                        postConstructorParameters[n] = new ParameterInfo(parameter.ParameterType, id);
                    }

                    var postConstructor = new PostConstructorInfo(postConstructorParameters);
                    // 根据参数个数创建方法委托
                    if (postConstructorParameters.Length == 0)
                    {
                        postConstructor.postConstructor = MethodUtils.CreateParameterlessMethod(type, method);
                    }
                    else {
                        postConstructor.paramsPostConstructor = MethodUtils.CreateParameterizedMethod(type, method);
                    }

                    // 将方法委托加入数组
                    postConstructors.Add(postConstructor);
                }
            }

            // 以数组形式返回
            return postConstructors.ToArray();
        }

        /// <summary>
        /// 返回接受注入的属性信息 : type 为 key，object 为 id， PropertyInfo 为值
        /// </summary>
        virtual protected SetterInfo[] GetProperties(Type type)
        {
            var setters = new List<SetterInfo>();

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
                    var method = MethodUtils.CreatePropertySetter(type, property);
                    var info = new SetterInfo(property.PropertyType, attribute.id, method);
                    setters.Add(info);
            }
            }

            return setters.ToArray();
        }

        /// <summary>
        /// 返回接受注入的字段信息 : type 为 key，object 为 id， PropertyInfo 为值
        /// </summary>
        virtual protected SetterInfo[] GetFields(Type type)
        {
            var setters = new List<SetterInfo>();

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
                    var method = MethodUtils.CreateFieldSetter(type, field);
                    var info = new SetterInfo(field.FieldType, attribute.id, method);
                    setters.Add(info);
                }
            }

            return setters.ToArray();
        }
    }
}