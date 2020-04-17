using System;
using System.Linq;
using System.Reflection;

namespace ReflectClassName
{
	class Program
	{
		static void Main(string[] args)
		{
			//Reflect
			var a1 = AppDomain.CurrentDomain.GetAssemblies();
			var a = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault();
			string b = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			string c = b.Replace("\\","/");
			string d = c.Split("/").LastOrDefault();
			string path = c.Replace(d, "ClassLibrary1.dll");
			Assembly ass1 = Assembly.LoadFrom(@path);
			var type = ass1.GetType("ClassLibrary1.Class1");
			var obj = CreateInstance(type);
			var ic= obj as IClassLibrary1.IClass1;
			if (ic==null)
			{
				Console.WriteLine("失败");
			}
			else
			{
				ic.Print();
			}
		}

		//public void GetTypeClassPropertiesAndFunc(string className)
		//{
		//	Type classype = null;
		//	//根据类名获取Type对象
		//	//1.通过完全限定名
		//	string fullType = AAA + "." + className;
		//	classype = Type.GetType(fullType);
		//	//2.如果上面的方式得不到，可以通过遍历程序集查找
		//	//得到所有程序集
		//	Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
		//	Type[] types = null;
		//	foreach (Assembly assembly in assemblys)
		//	{
		//		//得到此程序集中所有类型
		//		types = assembly.GetTypes();
		//		if (types != null)
		//		{
		//			foreach (Type test in types)
		//			{
		//				//判断类型的名字是否与类名一致，忽视大小写
		//				if (test.Name.Equals(className, StringComparison.CurrentCultureIgnoreCase))
		//				{
		//					type = test;
		//					break;
		//				}
		//			}
		//		}
		//		if (type != null)
		//		{
		//			break;
		//		}
		//	}

		//	if (type != null)
		//	{
		//		//属性
		//		//此接口只有类中的字段必须封装才能获得
		//		Properties[] propertys = type.GetProperties();
		//		int i = 0;
		//		foreach (Properties property in propertys)
		//		{
		//			// 获取属性名
		//			Debug.LogError("property name : " + property.Name);
		//			// 给属性赋值
		//			property.SetValue(Tclass, System.Convert.ChangeType(i, property.PropertyType), null);
		//			// 获取属性值
		//			Debug.LogError(property.GetValue(Tclass, null));
		//		}

		//		//方法
		//		MethodInfo method = type.GetMethod("Add");
		//		method.Invoke(object, null);

		//		//字段
		//		FieldInfo[] fieldInfos = type.GetFields();
		//		foreach (FieldInfo fieldInfo in fieldInfos)
		//		{
		//			Debug.Log(fieldInfo.Name);
		//			Debug.Log(fieldInfo.FieldType.Name);
		//		}
		//	} 
		//}

			/// <summary>反射创建指定类型的实例</summary>
			/// <param name="type">类型</param>
			/// <param name="parameters">参数数组</param>
			/// <returns></returns>
			public static Object CreateInstance(Type type, params Object[] parameters)
		{
			try
			{
				if (parameters == null || parameters.Length == 0)
					return Activator.CreateInstance(type, true);
				else
					return Activator.CreateInstance(type, parameters);
			}
			catch (Exception ex)
			{
				//throw new Exception("创建对象失败 type={0} parameters={1}".F(type.FullName, parameters.Join()), ex);
				throw new Exception($"创建对象失败 type={type.FullName} parameters={string.Join(",", parameters)} {ex?.Message}");
			}
		}

		/// <summary>根据名称获取类型。可搜索当前目录DLL，自动加载</summary>
		/// <param name="typeName">类型名</param>
		/// <param name="isLoadAssembly">是否从未加载程序集中获取类型。使用仅反射的方法检查目标类型，如果存在，则进行常规加载</param>
		/// <returns></returns>
		//public static Type GetTypeEx(String typeName, Boolean isLoadAssembly = true)
		//{
		//	if (String.IsNullOrEmpty(typeName)) return null;

		//	var type = Type.GetType(typeName);
		//	if (type != null) return type;

		//	//return GetType(typeName, isLoadAssembly);
		//}


		///// <summary>根据名称获取类型</summary>
		///// <param name="typeName">类型名</param>
		///// <param name="isLoadAssembly">是否从未加载程序集中获取类型。使用仅反射的方法检查目标类型，如果存在，则进行常规加载</param>
		///// <returns></returns>
		//internal static Type GetType(String typeName, Boolean isLoadAssembly)
		//{
		//	var type = Type.GetType(typeName);
		//	if (type != null) return type;

		//	// 数组
		//	if (typeName.EndsWith("[]"))
		//	{
		//		var elemType = GetType(typeName.Substring(0, typeName.Length - 2), isLoadAssembly);
		//		if (elemType == null) return null;

		//		return elemType.MakeArrayType();
		//	}

		//	// 加速基础类型识别，忽略大小写
		//	if (!typeName.Contains("."))
		//	{
		//		foreach (var item in Enum.GetNames(typeof(TypeCode)))
		//		{
		//			if (typeName.EqualIgnoreCase(item))
		//			{
		//				type = Type.GetType("System." + item);
		//				if (type != null) return type;
		//			}
		//		}
		//	}

		//	// 尝试本程序集
		//	var asms = new[] {
		//		Create(Assembly.GetExecutingAssembly()),
		//		Create(Assembly.GetCallingAssembly()),
		//		Create(Assembly.GetEntryAssembly()) };
		//	var loads = new List<AssemblyX>();

		//	foreach (var asm in asms)
		//	{
		//		if (asm == null || loads.Contains(asm)) continue;
		//		loads.Add(asm);

		//		type = asm.GetType(typeName);
		//		if (type != null) return type;
		//	}

		//	// 尝试所有程序集
		//	foreach (var asm in GetAssemblies())
		//	{
		//		if (loads.Contains(asm)) continue;
		//		loads.Add(asm);

		//		type = asm.GetType(typeName);
		//		if (type != null) return type;
		//	}

		//	// 尝试加载只读程序集
		//	if (!isLoadAssembly) return null;

		//	foreach (var asm in ReflectionOnlyGetAssemblies())
		//	{
		//		type = asm.GetType(typeName);
		//		if (type != null)
		//		{
		//			// 真实加载
		//			var file = asm.Asm.Location;
		//			try
		//			{
		//				type = null;
		//				var asm2 = Assembly.LoadFile(file);
		//				var type2 = Create(asm2).GetType(typeName);
		//				if (type2 == null) continue;
		//				type = type2;
		//				if (XTrace.Debug)
		//				{
		//					var root = ".".GetFullPath();
		//					if (file.StartsWithIgnoreCase(root)) file = file.Substring(root.Length).TrimStart("\\");
		//					Console.WriteLine("TypeX.GetType(\"{0}\") => {1}", typeName, file);
		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				Console.WriteLine(ex);
		//			}

		//			return type;
		//		}
		//	}

		//	return null;
		//}

		///// <summary>创建程序集辅助对象</summary>
		///// <param name="asm"></param>
		///// <returns></returns>
		//public static AssemblyX Create(Assembly asm)
		//{
		//	if (asm == null) return null;

		//	return cache.GetOrAdd(asm, key => new AssemblyX(key));
		//}
	}
}
