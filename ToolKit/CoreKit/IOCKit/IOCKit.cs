using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace CMFramework
{
    /// <summary>
    /// 由注入容器来决定是注入属性 | 字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public InjectAttribute()
        {
        }
    }

    public interface ICMFrameworkContainer
    {
        /// <summary>
        /// 清空所有 类型映射 和 实例
        /// </summary>
        void Clear();

        /// <summary>
        /// 将注册的 类型/映射 注入到对象中
        /// </summary>
        /// <param name="obj"></param>
        void Inject(object obj);

        /// <summary>
        /// 注入所有
        /// </summary>
        void InjectAll();

        /// <summary>
        /// 注册一个类型映射
        /// </summary>
        /// <typeparam name="TSource">The base type.</typeparam>
        /// <typeparam name="TTarget">The concrete type</typeparam>
        void Register<TSource, TTarget>(string name = null);

        /// <summary>
        /// 注册依赖关系
        /// </summary>
        /// <typeparam name="TFor"></typeparam>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        void RegisterRelation<TFor, TBase, TConcrete>();

        /// <summary>
        /// 注册类型的实例
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="default"></param>
        /// <param name="injectNow"></param>
        /// <returns></returns>
        void RegisterInstance<TBase>(TBase @default, bool injectNow) where TBase : class;

        /// <summary>
        /// 注册类型的实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="default"></param>
        /// <param name="injectNow"></param>
        /// <returns></returns>
        void RegisterInstance(Type type, object @default, bool injectNow);

        /// <summary>
        /// 注册已命名实例
        /// </summary>
        /// <param name="baseType">要注册的类型</param>
        /// <param name="instance">The instance that will be resolved be the name</param>
        /// <param name="name">The name for the instance to be resolved.</param>
        /// <param name="injectNow">Perform the injection immediately</param>
        void RegisterInstance(Type baseType, object instance = null, string name = null, bool injectNow = true);

        void RegisterInstance<TBase>(TBase instance, string name, bool injectNow = true) where TBase : class;

        void RegisterInstance<TBase>(TBase instance) where TBase : class;

        /// <summary>
        ///  如果 T 的实例存在就返回该实例，不然创建一个新的实例
        /// </summary>
        /// <typeparam name="T">要解析的实例类型</typeparam>
        /// <returns>T 的实例</returns>
        T Resolve<T>(string name = null, bool requireInstance = false, params object[] args) where T : class;

        TBase ResolveRelation<TBase>(Type tFor, params object[] arg);

        TBase ResolveRelation<TFor, TBase>(params object[] arg);

        /// <summary>
        /// Resolves all instances of TType or subclasses of TType.  Either named or not.
        /// </summary>
        /// <typeparam name="TType">The Type to resolve</typeparam>
        /// <returns>List of objects.</returns>
        IEnumerable<TType> ResolveAll<TType>();

        //IEnumerable<object> ResolveAll(Type type);
        void Register(Type source, Type target, string name = null);

        /// <summary>
        /// Resolves all instances of TType or subclasses of TType.  Either named or not.
        /// </summary>
        /// <param name="type">The Type to resolve</param>
        /// <returns>List of objects.</returns>
        IEnumerable<object> ResolveAll(Type type);

        TypeMappingCollection Mappings { get; set; }
        TypeInstanceCollection Instances { get; set; }
        TypeRelationCollection RelationshipMappings { get; set; }

        /// <summary>
        /// If an instance of instanceType exist then it will return that instance otherwise it will create a new one based off mappings.
        /// </summary>
        /// <param name="baseType">要解析的实例类型</param>
        /// <param name="name">要解析的实例类型</param>
        /// <param name="requireInstance">如果为 true 或者实例未注册，返回null</param>
        /// <param name="constructorArgs">构造函数参数</param>
        /// <returns>实例类型的实例</returns>
        object Resolve(Type baseType, string name = null, bool requireInstance = false,
            params object[] constructorArgs);

        object ResolveRelation(Type tFor, Type tBase, params object[] arg);
        void RegisterRelation(Type tFor, Type tBase, Type tConcrete);
        object CreateInstance(Type type, params object[] args);
    }

    /// <summary>
    /// ViewModel 容器以及控制器和命令的工厂
    /// </summary>
    public class CMFrameworkContainer : ICMFrameworkContainer
    {
        private TypeInstanceCollection _instances;
        private TypeMappingCollection _mappings;


        public TypeMappingCollection Mappings
        {
            get => _mappings ??= new TypeMappingCollection();
            set => _mappings = value;
        }

        public TypeInstanceCollection Instances
        {
            get => _instances ??= new TypeInstanceCollection();
            set => _instances = value;
        }

        public TypeRelationCollection RelationshipMappings
        {
            get => _relationshipMappings;
            set => _relationshipMappings = value;
        }

        public IEnumerable<TType> ResolveAll<TType>()
        {
            foreach (var obj in ResolveAll(typeof(TType)))
            {
                yield return (TType)obj;
            }
        }

        /// <summary>
        /// Resolves all instances of TType or subclasses of TType.  Either named or not.
        /// </summary>
        /// <param name="type">The Type to resolve</param>
        /// <returns>List of objects.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            // KeyValuePair是用来存放键值对，常用于临时存放键值对，是一个结构体。
            // 从类型实例集合中遍历所有实例
            foreach (KeyValuePair<Tuple<Type, string>, object> kv in Instances)
            {
                // 如果 实例的 Type 和 Type 相等，并且实例名称不为空，就返回值
                if (kv.Key.Item1 == type && !string.IsNullOrEmpty(kv.Key.Item2))
                    yield return kv.Value;
            }

            // 从类型映射集合中遍历所有映射类型
            foreach (KeyValuePair<Tuple<Type, string>, Type> kv in Mappings)
            {
                // 如果映射名称不为空
                if (!string.IsNullOrEmpty(kv.Key.Item2))
                {
#if NETFX_CORE
                    var condition = type.GetTypeInfo().IsSubclassOf(mapping.From);
#else
                    // 判断映射的类型是否可以分配给传入的类型
                    bool condition = type.IsAssignableFrom(kv.Key.Item1);
#endif
                    if (condition)
                    {
                        // 反射创建实例
                        object item = Activator.CreateInstance(kv.Value);
                        Inject(item);
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Clears all type-mappings and instances.
        /// </summary>
        public void Clear()
        {
            Instances.Clear();
            Mappings.Clear();
            RelationshipMappings.Clear();
        }

        /// <summary>
        /// 注入注册的 类型/映射 到对象中
        /// <para>
        /// Injects registered types/mappings into an object
        /// </para>
        /// </summary>
        /// <param name="obj"></param>
        public void Inject(object obj)
        {
            if (obj == null) return;
#if !NETFX_CORE
            // 通过反射获取类所有的成员
            MemberInfo[] members = obj.GetType().GetMembers();
#else
            var members = obj.GetType().GetTypeInfo().DeclaredMembers;
#endif
            // 遍历所有的成员
            foreach (var memberInfo in members)
            {
                // 获取当前成员使用 Inject 特性的成员，因为是MemberInfo，所以只会有一个成员。
                // 获取第一个成员，转换成 InjectAttribute，因为只有一个成员，所以如果没有用 Inject 特性，就是空数组，就会为false，进入下一次循环
                if (memberInfo.GetCustomAttributes(typeof(InjectAttribute), true).FirstOrDefault() is InjectAttribute
                    injectAttribute)
                {
                    // 如果该成员是属性
                    if (memberInfo is PropertyInfo propertyInfo)
                    {
                        propertyInfo.SetValue(obj, Resolve(propertyInfo.PropertyType, injectAttribute.Name), null);
                    }
                    // 如果该成员是字段(成员变量)
                    else if (memberInfo is FieldInfo fieldInfo)
                    {
                        fieldInfo.SetValue(obj, Resolve(fieldInfo.FieldType, injectAttribute.Name));
                    }
                }
            }
        }

        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <typeparam name="TSource">The base type.</typeparam>
        public void Register<TSource>(string name = null)
        {
            Mappings[typeof(TSource), name] = typeof(TSource);
        }


        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <typeparam name="TSource">The base type.</typeparam>
        /// <typeparam name="TTarget">The concrete type</typeparam>
        public void Register<TSource, TTarget>(string name = null)
        {
            Mappings[typeof(TSource), name] = typeof(TTarget);
        }

        /// <summary>
        /// 注册类型映射
        /// </summary>
        /// <param name="source">基类</param>
        /// <param name="target">具体类型</param>
        /// <param name="name">类型名称</param>
        public void Register(Type source, Type target, string name = null)
        {
            Mappings[source, name] = target;
        }

        /// <summary>
        /// 注册实例
        /// </summary>
        /// <param name="baseType">The type to register the instance for.</param>        
        /// <param name="instance">The instance that will be resolved be the name</param>
        /// <param name="injectNow">Perform the injection immediately</param>
        public void RegisterInstance(Type baseType, object instance = null, bool injectNow = true)
        {
            RegisterInstance(baseType, instance, null, injectNow);
        }

        /// <summary>
        /// 注册命名实例
        /// </summary>
        /// <param name="baseType">The type to register the instance for.</param>
        /// <param name="name">The name for the instance to be resolved.</param>
        /// <param name="instance">The instance that will be resolved be the name</param>
        /// <param name="injectNow">是否立即进行注射</param>
        public virtual void RegisterInstance(Type baseType, object instance = null, string name = null,
            bool injectNow = true)
        {
            Instances[baseType, name] = instance;
            if (injectNow)
            {
                Inject(instance);
            }
        }

        public void RegisterInstance<TBase>(TBase instance) where TBase : class
        {
            RegisterInstance(instance, true);
        }

        public void RegisterInstance<TBase>(TBase instance, bool injectNow) where TBase : class
        {
            RegisterInstance(instance, null, injectNow);
        }

        public void RegisterInstance<TBase>(TBase instance, string name, bool injectNow = true) where TBase : class
        {
            RegisterInstance(typeof(TBase), instance, name, injectNow);
        }

        /// <summary>
        ///  If an instance of T exist then it will return that instance otherwise it will create a new one based off mappings.
        /// </summary>
        /// <typeparam name="T">The type of instance to resolve</typeparam>
        /// <returns>The/An instance of 'instanceType'</returns>
        public T Resolve<T>(string name = null, bool requireInstance = false, params object[] args) where T : class
        {
            return (T)Resolve(typeof(T), name, requireInstance, args);
        }

        /// <summary>
        /// 如果 instanceType 的实例存在，那么它将返回该实例，否则它将根据映射创建一个新实例
        /// </summary>
        /// <param name="baseType">要解析的实例类型</param>
        /// <param name="name">要解析的实例类型</param>
        /// <param name="requireInstance">If true will return null if an instance isn't registered.</param>
        /// <param name="constructorArgs">The arguments to pass to the constructor if any.</param>
        /// <returns>The/An instance of 'instanceType'</returns>
        public object Resolve(Type baseType, string name = null, bool requireInstance = false,
            params object[] constructorArgs)
        {
            // Look for an instance first
            var item = Instances[baseType, name];
            if (item != null)
            {
                return item;
            }

            if (requireInstance)
                return null;

            // 检查是否存在该类型的映射
            Type namedMapping = Mappings[baseType, name];

            if (namedMapping == null) return null;

            var obj = CreateInstance(namedMapping, constructorArgs);
            //Inject(obj);
            return obj;
        }

        public object CreateInstance(Type type, params object[] constructorArgs)
        {
            // 模式匹配
            if (constructorArgs is { Length: > 0 })
            {
                //return Activator.CreateInstance(type,BindingFlags.Public | BindingFlags.Instance,Type.DefaultBinder, constructorArgs,CultureInfo.CurrentCulture);
                var obj2 = Activator.CreateInstance(type, constructorArgs);
                Inject(obj2);
                return obj2;
            }
#if !NETFX_CORE
            ConstructorInfo[] constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
#else
        ConstructorInfo[] constructor = type.GetTypeInfo().DeclaredConstructors.ToArray();
#endif

            if (constructor.Length < 1)
            {
                var obj2 = Activator.CreateInstance(type);
                Inject(obj2);
                return obj2;
            }

            var maxParameters = constructor.First().GetParameters();

            foreach (var c in constructor)
            {
                var parameters = c.GetParameters();
                if (parameters.Length > maxParameters.Length)
                {
                    maxParameters = parameters;
                }
            }

            var args = maxParameters.Select(p =>
            {
                if (p.ParameterType.IsArray)
                {
                    return ResolveAll(p.ParameterType);
                }

                return Resolve(p.ParameterType) ?? Resolve(p.ParameterType, p.Name);
            }).ToArray();

            var obj = Activator.CreateInstance(type, args);
            Inject(obj);
            return obj;
        }

        public TBase ResolveRelation<TBase>(Type tFor, params object[] args)
        {
            try
            {
                return (TBase)ResolveRelation(tFor, typeof(TBase), args);
            }
            catch (InvalidCastException castIssue)
            {
                throw new Exception(
                    $"Resolve Relation couldn't cast  to {typeof(TBase).Name} from {tFor.Name}",
                    castIssue);
            }
        }

        public void InjectAll()
        {
            foreach (object instance in Instances.Values)
            {
                Inject(instance);
            }
        }

        private TypeRelationCollection _relationshipMappings = new TypeRelationCollection();

        public void RegisterRelation<TFor, TBase, TConcrete>()
        {
            RelationshipMappings[typeof(TFor), typeof(TBase)] = typeof(TConcrete);
        }

        public void RegisterRelation(Type tFor, Type tBase, Type tConcrete)
        {
            RelationshipMappings[tFor, tBase] = tConcrete;
        }

        public object ResolveRelation(Type tFor, Type tBase, params object[] args)
        {
            var concreteType = RelationshipMappings[tFor, tBase];

            if (concreteType == null)
            {
                return null;
            }

            var result = CreateInstance(concreteType, args);
            //Inject(result);
            return result;
        }

        public TBase ResolveRelation<TFor, TBase>(params object[] arg)
        {
            return (TBase)ResolveRelation(typeof(TFor), typeof(TBase), arg);
        }
    }

    // http://stackoverflow.com/questions/1171812/multi-key-dictionary-in-c
    public class Tuple<T1, T2> //FUCKING Unity: struct is not supported in Mono
    {
        public readonly T1 Item1;
        public readonly T2 Item2;

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return false;

            Tuple<T1, T2> p = obj as Tuple<T1, T2>;
            Debug.LogWarning($"{typeof(Tuple)} 的转换异常");
            if (p == null) return false;

            if (Item1 == null)
            {
                if (p.Item1 != null) return false;
            }
            else
            {
                if (p.Item1 == null || !Item1.Equals(p.Item1)) return false;
            }

            if (Item2 == null)
            {
                if (p.Item2 != null) return false;
            }
            else
            {
                if (p.Item2 == null || !Item2.Equals(p.Item2)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (Item1 != null)
                hash ^= Item1.GetHashCode();
            if (Item2 != null)
                hash ^= Item2.GetHashCode();
            return hash;
        }
    }

    // 使用字典，不要使用列表！
    public class TypeMappingCollection : Dictionary<Tuple<Type, string>, Type>
    {
        public Type this[Type from, string name = null]
        {
            get
            {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                return this.GetValueOrDefault(key);
            }
            set
            {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                this[key] = value;
            }
        }
    }

    public class TypeInstanceCollection : Dictionary<Tuple<Type, string>, object>
    {
        public object this[Type from, string name = null]
        {
            get
            {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                return this.GetValueOrDefault(key);
            }
            set
            {
                Tuple<Type, string> key = new Tuple<Type, string>(from, name);
                this[key] = value;
            }
        }
    }

    public class TypeRelationCollection : Dictionary<Tuple<Type, Type>, Type>
    {
        public Type this[Type from, Type to]
        {
            get
            {
                Tuple<Type, Type> key = new Tuple<Type, Type>(from, to);
                return this.GetValueOrDefault(key);
            }
            set
            {
                Tuple<Type, Type> key = new Tuple<Type, Type>(from, to);
                this[key] = value;
            }
        }
    }
}