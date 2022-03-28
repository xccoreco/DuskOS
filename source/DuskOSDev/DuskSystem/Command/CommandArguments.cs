using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command
{
    public sealed class CommandArguments
    {
        //TODO: Add support for a valued argument of either type of: key:value or key=value.
        //Also add support for a valued variable argument variable=value.

        private List<string> arguments;

        /// <summary>
        /// Creates a new instance of the <see cref="CommandArguments"/> class.
        /// </summary>
        /// <param name="strx">The strings that identify the arguments.</param>
        public CommandArguments(string[] strx)
        {
            arguments = new List<string>();
            foreach (string s in strx)
                arguments.Add(s);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CommandArguments"/> class.
        /// </summary>
        /// <param name="strx">The strings that identify the arguments.</param>
        public CommandArguments(List<string> strx)
        {
            arguments = strx;
        }

        /// <summary>
        /// Gets the number of arguments present.
        /// </summary>
        public int Count => arguments.Count;
        /// <summary>
        /// Denotes whether arguments are present.
        /// </summary>
        public bool IsEmpty
            => (Count <= 0);
        /// <summary>
        /// Gets a command at the specified argument position as it was processed.
        /// </summary>
        /// <param name="position">The position of an argument.</param>
        /// <returns>The argument at the specified position.</returns>
        public string GetArgumentAtPosition(int position)
            => (!IsEmpty) ? arguments[position] : "";

        /// <summary>
        /// Gets a list of all argumetns passed to the specified command.
        /// </summary>
        /// <returns></returns>
        public string[] GetArguments() => arguments.ToArray();
        /// <summary>
        /// Checks if an argument was passed to a specific command.
        /// </summary>
        /// <param name="arg">The argument to verify.</param>
        /// <returns>True, if the argument was found.</returns>
        public bool ContainsArgument(string arg)
        {
            if (!(IsEmpty))
            {
                foreach (string s in arguments)
                {
                    if (s.Equals(arg))
                        return true;
                    else
                        continue;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a 'long named switch <c>--help</c>' was passed to a command.
        /// </summary>
        /// <param name="s">The switch value.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool ContainsSwitch(string s)
            => !IsEmpty && ContainsArgument($"--{s}");
        /// <summary>
        /// Checks if a 'short named switch <c>-h</c>' was passed to a command.
        /// </summary>
        /// <param name="c">The switch value.</param>
        /// <returns>True, if the switch was found.</returns>
        public bool ContainsSwitch(char c)
            => !IsEmpty && ContainsArgument($"-{c}");
        /// <summary>
        /// Checks if a variable '$variable or %variable' was passed to a command.
        /// </summary>
        /// <param name="v">The variable.</param>
        /// <returns>True, if the variable was found.</returns>
        public bool ContainsVariable(Variable v)
            => !IsEmpty && ContainsArgument(v.ToString());
        /// <summary>
        /// Checks if an argument is at the begining of the argument array.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>True, if the argument is the first argument in the argument array.</returns>
        public bool StartsWith(string arg)
            => !IsEmpty && GetArgumentAtPosition(0).Equals(arg);

        /// <summary>
        /// Checks if an argument is at the end of the argument array.
        /// </summary>
        /// <param name="arg">The argument</param>
        /// <returns>True, if the argument is the last argument in the argument array.</returns>
        public bool EndsWith(string arg)
        {
            int index = Count - 1;
            if (index < 0)
                index = 0;

            return !IsEmpty && GetArgumentAtPosition(index).Equals(arg);
        }

        /// <summary>
        /// Checks if a 'long named switch <c>--help</c>' is at the begining of the argument array.
        /// </summary>
        /// <param name="s">The switch.</param>
        /// <returns>True, if the switch is the first argument in the argument array.</returns>
        public bool StartsWithSwitch(string s)
            => !IsEmpty && StartsWith($"--{s}");
        /// <summary>
        /// Checks if a 'short named switch <c>-h</c>' is at the begining of the argument array.
        /// </summary>
        /// <param name="c">The switch.</param>
        /// <returns>True, if the argument is at the begining of the array.</returns>
        public bool StartsWithSwitch(char c)
            => !IsEmpty && StartsWith($"-{c}");

        /// <summary>
        /// Checks if a variable '$VARIABLE or %variable%' is at the begining of the argument array.
        /// </summary>
        /// <param name="v">The variable.</param>
        /// <returns>True, if the variable is the first argument in the argument array.</returns>
        public bool StartsWithVariable(Variable v)
            => !IsEmpty && StartsWith(v.ToString());
        /// <summary>
        /// Checks if a 'long named swith <c>--help</c>' is at the end of the argument array.
        /// </summary>
        /// <param name="s">The value of the switch.</param>
        /// <returns>True, if the switch is the last argument in the argument array.</returns>
        public bool EndsWithSwitch(string s)
            => !IsEmpty && EndsWith($"--{s}");
        /// <summary>
        /// Checks if a 'short named switch <c>-c</c>' is at the end of the argument array.
        /// </summary>
        /// <param name="c">The switch.</param>
        /// <returns>True, if the switch is at the end of the argument array.</returns>
        public bool EndsWithSwitch(char c)
            => !IsEmpty && EndsWith($"-{c}");
        /// <summary>
        /// Checks if a variable '$VARIABLE or %variable%' is at the end of the argument array.
        /// </summary>
        /// <param name="v">The variable.</param>
        /// <returns>True, if the variable is at the end of the argument array.</returns>
        public bool EndsWithVariable(Variable v)
            => !IsEmpty && EndsWith(v.ToString());
        /// <summary>
        /// Gets the index (position) of the argument within the argument array.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>A value based on the argumets position in the argument array if the argument was found.</returns>
        public int IndexOfArgument(string arg)
        {
            if (!IsEmpty)
            {
                for (int i = 0; i < Count; i++)
                {
                    var x = GetArgumentAtPosition(i);
                    if (x.Equals(arg))
                        return i;
                    else
                        continue;
                }
            }
            return -1;
        }
        /// <summary>
        /// Gets the index (position) of a 'long named switch <c>--help</c>' within the argument array.
        /// </summary>
        /// <param name="s">The switch value.</param>
        /// <returns>A value based on the switch's position within the argument array if the switch was found.</returns>
        public int IndexOfSwitch(string s)
            => !IsEmpty ? IndexOfArgument($"--{s}") : -1;
        /// <summary>
        /// Gets the index (position) of a 'short named switch <c>-c</c>' within the argument array.
        /// </summary>
        /// <param name="c">The switch value.</param>
        /// <returns>A value based on the switch's position within the argument array if the switch was found.</returns>
        public int IndexOfSwitch(char c)
            => !IsEmpty ? IndexOfArgument($"-{c}") : -1;
        /// <summary>
        /// Gets the index (position) of a variable '$VARIABLE or %variable%' within the argument array.
        /// </summary>
        /// <param name="v">The variable</param>
        /// <returns>A value based on the variable's position within the argument array if the variable was found.</returns>
        public int IndexOfVariable(Variable v)
            => !IsEmpty ? IndexOfArgument(v.ToString()) : -1;
        /// <summary>
        /// Gets an argument that comes after the specified 'long named switch <c>--help</c>' in the argument array.
        /// </summary>
        /// <param name="s">The switch.</param>
        /// <returns>An argument that is found at the position after the specified switch.</returns>
        public string GetArgumentAfterSwitch(string s)
        {
            int index = IndexOfSwitch(s);
            if (index == -1)
                index = 0;
            index += 1; //increase by one.
            if (index >= Count)
                index = Count;
            return GetArgumentAtPosition(index);
        }
        /// <summary>
        /// Gets an argument that comes after the specified 'short named switch <c>-h</c>' in the argument array.
        /// </summary>
        /// <param name="c">The switch.</param>
        /// <returns>An argument that is found at the position after the specified switch.</returns>
        public string GetArgumentAfterSwitch(char c)
        {
            int index = IndexOfSwitch(c);
            index++;
            if (index >= Count)
                index = Count;
            return GetArgumentAtPosition(index);
        }
    }

    /// <summary>
    /// Defines a variable.
    /// </summary>
    public sealed class Variable
    {
        private VariableType variableType;
        private string variable = "var";

        /// <summary>
        /// Creates a new instance of a variable.
        /// </summary>
        /// <param name="variableType">The defining type of variable.</param>
        /// <param name="variable">The name of the variable.</param>
        public Variable(VariableType variableType, string variable)
        {
            this.variableType = variableType;
            this.variable = variable;
        }
        /// <summary>
        /// The pysical name of the variable.
        /// </summary>
        /// <returns>The name of the variable.</returns>
        public string GetVariableString() => variable;
        /// <summary>
        /// The entire variable.
        /// </summary>
        /// <returns>The entire variable.</returns>
        public override string ToString()
        {
            if (variableType == VariableType.WIN_VARIABLE)
                return $"%{variable}%";
            else
                return "${" + variable + "}";
        }

        /// <summary>
        /// Represents the type of variables that can be used.
        /// </summary>
        public enum VariableType
        {
            /// <summary>
            /// Represents a windows-like variable. <c>%variable%</c>
            /// </summary>
            WIN_VARIABLE,
            /// <summary>
            /// Represents a *nix-like variable. <c>$VARIABLE</c>
            /// </summary>
            LINUX_VARIABLE
        }
    }

    public sealed class ArgumentValue
    {
        public enum ArgumentType
        {
            SHORT_SWITCH, //-s
            LONG_SWITCH, //--switch
            VALUED_SWITCH, //-v:value or -v=value
            QUOTE, //"Some Value"
            VARIABLE,
            VALUED_VARIABLE, //$VARIABLE=value
            TEXT //Defines a standard argument type.
        }
    }
}
