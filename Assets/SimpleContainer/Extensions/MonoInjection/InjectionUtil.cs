using SimpleContainer.Container;

namespace SimpleContainer
{
	public static class InjectionUtil {
        /// <summary>
        /// Injects into a specified object using container details.
        /// 获取参数 obj 上附着的 InjectFromContainer，用特性的 id 执行注入
        /// </summary>
        public static void Inject(object obj)
        {
            // 获取传人参数的特性
			var attributes = obj.GetType().GetCustomAttributes(true);

            // 如果没有获取到，就用空 id 进行注入
            if (attributes.Length == 0) { Inject(obj, null); }
            else
            {
				var containInjectFromContainer = false;
				
				for (var i = 0; i < attributes.Length; i++)
                {
                    var attribute = attributes[i];
                    // 如需根据容器的 id 进行匹配，将 InjectFromContainer 的 id 作为要对照的 id 传入
                    if (attribute is InjectFromContainer)
                    {
                        Inject(obj, (attribute as InjectFromContainer).id);
						containInjectFromContainer = true;
					}
				}
                
                //如果到最后都没有获取到 InjectFromContainer 特性，用空 id 进行注入
                if (!containInjectFromContainer) { Inject(obj, null); }
			}
		}

        /// <summary>
        /// 如果参数 obj 不是单例 binding 的值，为 ContextRoot 的 containers List 中每个
        /// 元素的 container id 与参数 id 相等的容器注入 obj。
        /// 如参数 id 为空，只要 obj 不是单例就对每一个容器进行注入，否则只对容器 id 相等的容器注入
        /// </summary>
        public static void Inject(object obj, object id)
        {
            // 获取 ContextRoot 中的 containers List
            var containers = ContextRoot.containers;

            for (int i = 0; i < containers.Count; i++)
            {
                // 遍历 list，如果容器 id 不为空且和参数 id 相等，injectOnContainer 为真
                var injectOnContainer = (containers[i].id != null && containers[i].id.Equals(id));

                // 如参数 id 为空或 id 与容器 id 相等，且参数 obj 不与容器中任何 binding 的值重复，
                // 就为当前容器注入 obj(避免重复注入)
                if ((id == null || injectOnContainer) && 
                    !IsExistOnContainer(obj, containers[i]))
                {
                    containers[i].Inject(obj);
				}
			}
		}
		
		/// <summary>
		/// 返回指定容器中的 object 是否已经存在
		/// </summary>
		public static bool IsExistOnContainer(object obj, IInjectionContainer container)
        {
			var isExist = false;
			var bindings = container.GetTypes(obj.GetType());

            if (bindings == null) { return false; }
			
			for (var i = 0; i < bindings.Count; i++)
            {
                int length = bindings[i].valueList.Count;
                for (int n = 0; n < length; n++)
                {
                    if (bindings[i].valueList[n] == obj)
                    {
                        isExist = true;
                        return isExist;
                    }
                }
			}
			
			return isExist;
        }

        /// <summary>
        /// 返回指定 binder 中的 object 是否已经存在
        /// </summary>
        public static bool IsExistOnBinder(object obj, IBinder binder)
        {
            var isExist = false;
            var bindings = binder.GetTypes(obj.GetType());

            if (bindings == null) { return false; }

            for (var i = 0; i < bindings.Count; i++)
            {
                int length = bindings[i].valueList.Count;
                for (int n = 0; n < length; n++)
                {
                    if (bindings[i].valueList[n] == obj)
                    {
                        isExist = true;
                        return isExist;
                    }
                }
            }

            return isExist;
        }
    }
}