using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JuliHelper.Creation
{
    public class Creator
    {
        public const char open = '(';
        public const char close = ')';
        public const char setterSeparator = '_';

        Dictionary<Type, object> autoParameters = new Dictionary<Type, object>();

        Dictionary<string, Type> shortcuts = new Dictionary<string, Type>();

        public Creator(ContentManager content)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes<CreatorShortcutAttribute>(false);
                foreach (var attr in attributes)
                {
                    shortcuts.Add(attr.ShortcutName, type);
                }
            }

            autoParameters.Add(GetType(), this);
            autoParameters.Add(content.GetType(), content);
        }

        public object CreateObject(ScriptReader reader)
        {
            object entity = CreateObject(reader, typeof(object));

            return entity;
        }

        public T CreateObject<T>(ScriptReader reader)
        {
            object entity = CreateObject(reader, typeof(T));
            return (T)entity;
        }

        /// <summary>"Type(ctorArg1)(ctorArg2)_Prop(val)_Method(arg1)(arg2)"</summary>
        private object CreateObject(ScriptReader reader, Type objectBaseType, object[] firstCtorParamValues = null)
        {
            string typeStr = reader.ReadToChar(open);
            reader.Move(-1); // move infront of open again

            Type type;
            if (shortcuts.ContainsKey(typeStr))
            {
                type = shortcuts[typeStr];
            }
            else
            {
                string fullTypeName = "JuliHelper." + typeStr;
                type = Type.GetType(fullTypeName);
                if (type == null)
                    throw new Exception("type " + fullTypeName + " not found");
            }

            if (!objectBaseType.IsAssignableFrom(type))
                throw new Exception("type " + nameof(type)  + " is not assignable to " + objectBaseType);
            
            object obj = CreateObject(type, reader, firstCtorParamValues);

            char? c;

            while ((c = reader.ReadChar()).HasValue)
            {
                if (c != setterSeparator)
                {
                    reader.Move(-1); // move back the wrongly read in char
                    return obj;
                }

                string setterName = reader.ReadToChar(open);

                SetPropertyMethodOrField(type, obj, setterName, reader);
            }

            return obj;
        }

        private void SetPropertyMethodOrField(Type type, object obj, string setterName, ScriptReader reader)
        {
            var prop = type.GetProperty(setterName);
            if (prop != null)
            {
                prop.SetValue(obj, GetParameters(reader.ReadToCharOrEndConsiderOpenCloseBraces(close, open, close), prop.PropertyType));
            }
            else
            {
                var method = type.GetMethod(setterName);
                if (method != null)
                {
                    reader.Move(-1);
                    object[] args = GetParameters(StringSplitConsiderOpenCloseBraces(reader), method.GetParameters().Select(f => f.ParameterType).ToArray());
                    method.Invoke(obj, args);
                }
                else
                {
                    var field = type.GetField(setterName);

                    if (field != null)
                    {
                        field.SetValue(obj, GetParameters(reader.ReadToCharOrEndConsiderOpenCloseBraces(close, open, close), field.FieldType));
                    }
                    else
                    {
                        throw new Exception("couldn't find property, method or field " + setterName);
                    }
                }
            }
        }

        /// <summary>"ctorArg1,ctorArg2"</summary>
        private object CreateObject(Type type, ScriptReader reader, object[] firstCtorParamValues = null)
        {
            object[] args = GetParametersForConstructor(reader, type, firstCtorParamValues);

            return Activator.CreateInstance(type, args);
        }

        /// <summary>"ctorArg1,ctorArg2"</summary>
        private object[] GetParametersForConstructor(ScriptReader reader, Type constructorType, object[] firstCtorParamValues = null)
        {
            firstCtorParamValues ??= new object[0];

            //if (argsStr == "" || argsStr == null)
            //    return firstCtorParamValues;

            var ctors = constructorType.GetConstructors();

            string[] split = StringSplitConsiderOpenCloseBraces(reader);

            ConstructorInfo ctorInfo = GetMatchingConstructor(firstCtorParamValues.Length, ctors, split);
            if (ctorInfo == null)
                throw new Exception("no matching constructor not found");

            var parameterInfos = ctorInfo.GetParameters().Skip(firstCtorParamValues.Length).ToArray();

            return firstCtorParamValues.Concat(GetParameters(split, parameterInfos.Select(f => f.ParameterType).ToArray())).ToArray();
        }

        private ConstructorInfo GetMatchingConstructor(int firstCtorParamValuesCount, ConstructorInfo[] ctors, string[] split)
        {
            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                int parametersForSplitArray = 0;
                for (int i = firstCtorParamValuesCount; i < parameters.Length; i++)
                {
                    if (!autoParameters.ContainsKey(parameters[i].ParameterType))
                        parametersForSplitArray++;
                }

                if (parametersForSplitArray == split.Length)
                    return ctor;
            }

            return null;
        }

        /// <summary>{"ctorArg1", "ctorArg2"}</summary>
        private object[] GetParameters(string[] split, Type[] expectedTypes)
        {
            if (split == null)
                split = new string[0];

            object[] output = new object[expectedTypes.Length];

            int splitIndex = 0;
            for (int i = 0; i < expectedTypes.Length; i++)
            {
                if (autoParameters.ContainsKey(expectedTypes[i]))
                    output[i] = autoParameters[expectedTypes[i]];
                else
                    output[i] = GetParameters(split[splitIndex++], expectedTypes[i]);
            }

            return output;
        }

        /// <summary>"ctorArg1"</summary>
        private object GetParameters(string argStr, Type expectedType)
        {
            if (expectedType == typeof(string))
                return argStr;
            else if (argStr.Contains(open))
            {
                ScriptReader reader = new ScriptReader(argStr);
                return CreateObject(reader, expectedType);
            }
            else
                return Convert.ChangeType(argStr, expectedType, CultureInfo.InvariantCulture);
        }

        private string[] StringSplitConsiderOpenCloseBraces(ScriptReader reader)
        {
            List<string> splits = new List<string>();

            char? peek;
            while ((peek = reader.Peek()) == open)
            {
                reader.ReadChar(); // skip open
                splits.Add(reader.ReadToCharOrEndConsiderOpenCloseBraces(close, open, close));
            }

            return splits.ToArray();
        }
    }
}
