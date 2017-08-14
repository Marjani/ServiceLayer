using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using MyApp.Framework.Extensions;

namespace MyApp.Framework.GuardToolkit
{
    public static class Guard
    {
        #region Constants

        private const string AgainstMessage = "Assertion evaluation failed with 'false'.";
        private const string ImplementsMessage = "Type '{0}' must implement type '{1}'.";
        private const string InheritsFromMessage = "Type '{0}' must inherit from type '{1}'.";
        private const string IsTypeOfMessage = "Type '{0}' must be of type '{1}'.";
        private const string IsEqualMessage = "Compared objects must be equal.";
        private const string IsPositiveMessage = "Argument '{0}' must be a positive value. Value: '{1}'.";
        private const string IsTrueMessage = "True expected for '{0}' but the condition was False.";
        private const string NotNegativeMessage = "Argument '{0}' cannot be a negative value. Value: '{1}'.";

        #endregion

        #region Public Methods

        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
            return value;
        }

        public static T? NotNull<T>(T? value, string parameterName) where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException(parameterName);
            return value;
        }

        public static string NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(parameterName);
            return value;
        }

        [DebuggerStepThrough]
        public static void Against<TException>(bool assertion, string message = AgainstMessage)
            where TException : Exception
        {
            if (assertion)
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        [DebuggerStepThrough]
        public static void Against<TException>(Func<bool> assertion, string message = AgainstMessage)
            where TException : Exception
        {
            //Execute the lambda and if it evaluates to true then throw the exception.
            if (assertion())
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull(object arg, string argName)
        {
            if (arg == null)
                throw Error.ArgumentNull(argName);
        }

        [DebuggerStepThrough]
        public static void ArgumentNotEmpty<T>(ICollection<T> arg, string argName)
        {
            if (arg != null && !arg.Any())
                throw Error.Argument(argName, "Collection cannot be null and must have at least one item.");
        }

        [DebuggerStepThrough]
        public static void ArgumentNotEmpty(Guid arg, string argName)
        {
            if (arg == Guid.Empty)
                throw Error.Argument(argName, "Argument '{0}' cannot be an empty guid.", argName);
        }

        [DebuggerStepThrough]
        public static void ArgumentInRange<T>(T arg, T min, T max, string argName) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(min) < 0 || arg.CompareTo(max) > 0)
                throw Error.ArgumentOutOfRange(argName, "The argument '{0}' must be between '{1}' and '{2}'.", argName,
                    min, max);
        }

        [DebuggerStepThrough]
        public static void ArgumentNotOutOfLength(string arg, int maxLength, string argName)
        {
            if (arg.Trim().Length > maxLength)
                throw Error.Argument(argName, "Argument '{0}' cannot be more than {1} characters long.", argName,
                    maxLength);
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNegative<T>(T arg, string argName, string message = NotNegativeMessage)
            where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 0)
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName, arg));
        }

        [DebuggerStepThrough]
        public static void ArgumentNotZero<T>(T arg, string argName) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) == 0)
                throw Error.ArgumentOutOfRange(argName,
                    "Argument '{0}' must be greater or less than zero. Value: '{1}'.", argName, arg);
        }

        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type)
        {
            InheritsFrom<TBase>(type, InheritsFromMessage.FormatInvariant(type.FullName, typeof(TBase).FullName));
        }

        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type, string message)
        {
            if (type.BaseType != typeof(TBase))
                throw new InvalidOperationException(message);
        }

        [DebuggerStepThrough]
        public static void Implements<TInterface>(Type type, string message = ImplementsMessage)
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
                throw new InvalidOperationException(
                    message.FormatInvariant(type.FullName, typeof(TInterface).FullName));
        }

        [DebuggerStepThrough]
        public static void IsSubclassOf<TBase>(Type type)
        {
            var baseType = typeof(TBase);
            if (!baseType.IsSubClass(type))
                throw new InvalidOperationException(
                    "Type '{0}' must be a subclass of type '{1}'.".FormatInvariant(type.FullName, baseType.FullName));
        }

        [DebuggerStepThrough]
        public static void IsTypeOf<TType>(object instance)
        {
            IsTypeOf<TType>(instance, IsTypeOfMessage.FormatInvariant(instance.GetType().Name, typeof(TType).FullName));
        }

        [DebuggerStepThrough]
        public static void IsTypeOf<TType>(object instance, string message)
        {
            if (!(instance is TType))
                throw new InvalidOperationException(message);
        }

        [DebuggerStepThrough]
        public static void IsEqual<TException>(object compare, object instance, string message = IsEqualMessage)
            where TException : Exception
        {
            if (!compare.Equals(instance))
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        [DebuggerStepThrough]
        public static void HasDefaultConstructor<T>()
        {
            HasDefaultConstructor(typeof(T));
        }

        [DebuggerStepThrough]
        public static void HasDefaultConstructor(Type t)
        {
            if (!t.HasDefaultConstructor())
                throw Error.InvalidOperation("The type '{0}' must have a default parameterless constructor.",
                    t.FullName);
        }

        [DebuggerStepThrough]
        public static void ArgumentIsPositive<T>(T arg, string argName, string message = IsPositiveMessage)
            where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 1)
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName));
        }

        [DebuggerStepThrough]
        public static void ArgumentIsTrue(bool arg, string argName, string message = IsTrueMessage)
        {
            if (!arg)
                throw Error.Argument(argName, message.FormatInvariant(argName));
        }


        [DebuggerStepThrough]
        public static void ArgumentIsEnumType(Type arg, string argName)
        {
            ArgumentNotNull(arg, argName);
            if (!arg.IsEnum)
                throw Error.Argument(argName, "Type '{0}' must be a valid Enum type.", arg.FullName);
        }

        [DebuggerStepThrough]
        public static void ArgumentIsEnumType(Type enumType, object arg, string argName)
        {
            ArgumentNotNull(arg, argName);
            if (!Enum.IsDefined(enumType, arg))
                throw Error.ArgumentOutOfRange(argName,
                    "The value of the argument '{0}' provided for the enumeration '{1}' is invalid.", argName,
                    enumType.FullName);
        }


        [DebuggerStepThrough]
        public static void PagingArgsValid(int indexArg, long sizeArg, string indexArgName, string sizeArgName)
        {
            ArgumentNotNegative(indexArg, indexArgName, "PageIndex cannot be below 0");
            if (indexArg > 0)
                ArgumentIsPositive(sizeArg, sizeArgName,
                    "PageSize cannot be below 1 if a PageIndex greater 0 was provided.");
            else
                ArgumentNotNegative(sizeArg, sizeArgName);
        }

        [DebuggerStepThrough]
        public static T ArgumentNotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                throw Error.ArgumentNull(parameterName);
            return value;
        }

        [DebuggerStepThrough]
        public static T? ArgumentNotNull<T>(T? value, string parameterName) where T : struct
        {
            if (!value.HasValue)
                throw Error.ArgumentNull(parameterName);
            return value;
        }

        [DebuggerStepThrough]
        public static string ArgumentNotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw Error.ArgumentNullOrEmpty(parameterName);
            return value;
        }

        [DebuggerStepThrough]
        public static string ArgumentNotWhiteSpace(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw Error.ArgumentNullOrWhiteSpace(parameterName);
            return value;
        }

        //public static bool ContainsNumber(this string inputText)
        //{
        //    return !string.IsNullOrWhiteSpace(inputText) && inputText.ToEnglishNumbers().Any(char.IsDigit);
        //}

        [DebuggerStepThrough]
        public static bool HasConsecutiveChars(string inputText, int sequenceLength = 3)
        {
            var charEnumerator = StringInfo.GetTextElementEnumerator(inputText);
            var currentElement = string.Empty;
            var count = 1;
            while (charEnumerator.MoveNext())
                if (currentElement == charEnumerator.GetTextElement())
                {
                    if (++count >= sequenceLength)
                        return true;
                }
                else
                {
                    count = 1;
                    currentElement = charEnumerator.GetTextElement();
                }
            return false;
        }

        [DebuggerStepThrough]
        public static bool IsEmailAddress(string inputText)
        {
            return !string.IsNullOrWhiteSpace(inputText) && new EmailAddressAttribute().IsValid(inputText);
        }

        //public static bool IsNumeric(this string inputText)
        //{
        //    if (string.IsNullOrWhiteSpace(inputText)) return false;

        //    long inputNumber;
        //    return long.TryParse(inputText.ToEnglishNumbers(), out inputNumber);
        //}

        #endregion
    }
}