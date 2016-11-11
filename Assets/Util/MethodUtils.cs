using System;
using System.Reflection;
using System.Reflection.Emit;
using SimpleContainer.Container;

namespace Utils
{
    public class MethodUtils
    {
        /// <summary>
        /// type "object"
        /// </summary>		
#pragma warning disable 0414
        private static Type OBJECT_TYPE = typeof(object);

        /// <summary>
        /// 返回一个没有参数的构造函数委托
        /// </summary>
        public static Constructor CreateConstructor(Type type, ConstructorInfo constructor)
        {
            // 如果是移动平台，直接使用 Invoke 方法调用
#if UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL

            return () => {
				return constructor.Invoke(null);
			};

#else

            // 否则新建一个动态方法,采用了 DynamicMethod(String, Type, Type[], Type) 形式的构造函数
            // 第一个参数为方法名称，第二个参数为返回类型，第三个参数为参数对象数组，第四个参数为可访问类型
            var method = new DynamicMethod(type.Name, type, null, type);
            // 从新建的动态方法中获取MSIL 生成器，默认 MSIL 流大小为 64 个字节
            ILGenerator generator = method.GetILGenerator();
            // 将指定的指令和字符参数放在 MSIL 指令流上
            // 第一个参数为要放到流上的 MSIL 指令，第二个参数为紧接着该指令推到流中的字符参数
            // OpCodes.Newobj 字段 : 创建一个值类型的新对象或新实例，并将对象引用（O 类型）推送到计算堆栈上
            generator.Emit(OpCodes.Newobj, constructor);
            // OpCodes.Ret 字段 : 从当前方法返回，并将返回值（如果存在）从被调用方的计算堆栈推送到调用方的计算堆栈上
            generator.Emit(OpCodes.Ret);
            // 完成动态方法并创建、返回一个可用于执行该方法的委托
            return (Constructor)method.CreateDelegate(typeof(Constructor));

#endif
        }

        /// <summary>
        /// 返回一个有参数的构造函数委托
        /// </summary>
        public static ParamsConstructor CreateConstructorWithParams(Type type, ConstructorInfo constructor)
        {
            // 如果是移动平台，直接使用 Invoke 方法调用
#if UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL

            return (object[] parameters) => {
				return constructor.Invoke(parameters);
			};
			
#else
            // 从构造函数中获取参数
            var parameters = constructor.GetParameters();

            Type[] parametersTypes = new Type[] { typeof(object[]) };
            // 创建动态方法 
            var method = new DynamicMethod(type.Name, type, parametersTypes, type);
            // 从新建的动态方法中获取 MSIL 生成器
            ILGenerator generator = method.GetILGenerator();

            for (int i = 0; i < parameters.Length; i++)
            {
                // OpCodes.Ldarg_0 : 将索引为 0 的参数加载到计算堆栈上 （设置参数）
                generator.Emit(OpCodes.Ldarg_0);

                // OpCodes.Ldc_I4_0 : 将整数值 0 作为 int32 推送到计算堆栈上，依次类推 （移动指针）
                switch (i)
                {
                    case 0: generator.Emit(OpCodes.Ldc_I4_0); break;
                    case 1: generator.Emit(OpCodes.Ldc_I4_1); break;
                    case 2: generator.Emit(OpCodes.Ldc_I4_2); break;
                    case 3: generator.Emit(OpCodes.Ldc_I4_3); break;
                    case 4: generator.Emit(OpCodes.Ldc_I4_4); break;
                    case 5: generator.Emit(OpCodes.Ldc_I4_5); break;
                    case 6: generator.Emit(OpCodes.Ldc_I4_6); break;
                    case 7: generator.Emit(OpCodes.Ldc_I4_7); break;
                    case 8: generator.Emit(OpCodes.Ldc_I4_8); break;
                    default: generator.Emit(OpCodes.Ldc_I4, i); break;
                }

                // OpCodes.Ldelem_Ref : 将位于指定数组索引处的包含对象引用的元素作为 O 类型（对象引用）加载到计算堆栈的顶部
                generator.Emit(OpCodes.Ldelem_Ref);
                Type paramType = parameters[i].ParameterType;
                // 将指定的指令放到 MSIL 流上，后跟给定类型的元数据标记
                // OpCodes.Unbox_Any : 将指令中指定类型的已装箱的表示形式转换成未装箱形式
                // OpCodes.Castclass : 尝试将引用传递的对象转换为指定的类
                generator.Emit(paramType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, paramType);
            }

            // 创建方法和委托并返回
            generator.Emit(OpCodes.Newobj, constructor);
            generator.Emit(OpCodes.Ret);
            return (ParamsConstructor)method.CreateDelegate(typeof(ParamsConstructor));

#endif
        }

        /// <summary>
        /// 返回字段设值器方法委托
        /// </summary>
        public static Setter CreateFieldSetter(Type type, FieldInfo fieldInfo)
        {
            // 如果是移动平台，直接使用 Invoke 方法调用
#if UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL

            return (object instance, object value) => fieldInfo.SetValue(instance, value);
			
#else

            var parametersTypes = new[] { OBJECT_TYPE, OBJECT_TYPE };
            DynamicMethod setMethod = new DynamicMethod(fieldInfo.Name, typeof(void), parametersTypes, true);
            ILGenerator generator = setMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            // OpCodes.Stfld : 用新值替换在对象引用或指针的字段中存储的值
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Emit(OpCodes.Ret);

            return (Setter)setMethod.CreateDelegate(typeof(Setter));

#endif
        }

        /// <summary>
        /// 返回属性设置器方法委托
        /// </summary>
        public static Setter CreatePropertySetter(Type type, PropertyInfo propertyInfo)
        {
            // 如果是移动平台，直接使用 Invoke 方法调用
#if UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL

            return (object instance, object value) => propertyInfo.SetValue(instance, value, null);
			
#else

            var propertySetMethod = propertyInfo.GetSetMethod();

            var parametersTypes = new Type[] { OBJECT_TYPE, OBJECT_TYPE };
            DynamicMethod method = new DynamicMethod(propertyInfo.Name, typeof(void), parametersTypes, true);
            ILGenerator generator = method.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            // OpCodes.Callvirt : 对对象调用后期绑定方法，并且将返回值推送到计算堆栈上
            generator.Emit(OpCodes.Callvirt, propertySetMethod);
            generator.Emit(OpCodes.Ret);

            return (Setter)method.CreateDelegate(typeof(Setter));

#endif
        }

        /// <summary>
        /// 返回一个没有参数的方法委托
        /// </summary>
        public static ParameterlessMethod CreateParameterlessMethod(Type type, System.Reflection.MethodInfo methodInfo)
        {
            // 如果是移动平台，直接使用 Invoke 方法调用
#if UNITY_IOS || UNITY_WSA || UNITY_WP8 || UNITY_WP8_1 || UNITY_WEBGL

            return (object instance) => methodInfo.Invoke(instance, null);
			
#else

            var parametersTypes = new Type[] { OBJECT_TYPE };
            DynamicMethod method = new DynamicMethod(
                methodInfo.Name,
                typeof(void),
                parametersTypes, true);
            ILGenerator generator = method.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, methodInfo);
            generator.Emit(OpCodes.Ret);

            return (ParameterlessMethod)method.CreateDelegate(typeof(ParameterlessMethod));

#endif
        }

        /// <summary>
        /// 返回一个有参数的方法委托
        /// </summary>
        public static ParamsMethod CreateParameterizedMethod(Type type, System.Reflection.MethodInfo methodInfo)
        {
            return (object instance, object[] parameters) => methodInfo.Invoke(instance, parameters);
        }

    }
}