using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCoding.Function.Command
{
    public class UndefinedCommand : Command
    {

        public string CommandName = "example";

        private string _format;
        private Dictionary<string, int> _commandArgs = new Dictionary<string, int>();
        private List<Type> _commandArgTypes = new List<Type>();
        private Delegate _command;


        public UndefinedCommand() { }

        public void Define(string format, Delegate command)
        {
            _commandArgs = new Dictionary<string, int>();
            _commandArgTypes = new List<Type>();
            _format = format;
            var temp = format.Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            string lastKey = null;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Length >= 2 && temp[i][1].Equals('-'))
                {
                    lastKey = temp[i].Substring(1);
                    _commandArgTypes.Add(TypeOf(temp[i][0]));
                    _commandArgs.Add(lastKey, 0);
                }
                else if (lastKey != null)
                {
                    _commandArgs[lastKey] += 1;
                }
            }
            _command = command;
            if (format.IndexOf(' ') != -1)
            {
                CommandName = format.Substring(0, format.IndexOf(' '));
            }
            else
            {
                CommandName = format;
            }
        }

        public void Run(string[] argsIn)
        {
            /*List<string> argsIn = new List<string>();
			for(int i = 0; i < _commandArgs.Keys.Count; i++) {
				if(parameters[i].GetType().Equals(typeof(string)) && ((string) parameters[i]).StartsWith("-")) {
					argsIn.Add((string) parameters[i]);
				}
			}*/
            List<object> argsOut = new List<object>();
            List<object> firstelements = new List<object>();
            for (int i = 1; i < argsIn.Length; i++)
            {
                if (argsIn[i].StartsWith("-"))
                {
                    break;
                }
                else
                {
                    firstelements.Add(argsIn[i]);
                }
            }
            if (firstelements.Count == 0)
            {
                return;
            }
            else if (firstelements.Count == 1)
            {
                argsOut.Add((string)firstelements[0]);
            }
            else
            {
                argsOut.Add(firstelements.ToArray());
            }

            int index = 1;
            foreach (var key in _commandArgs.Keys)
            {
                if (key.Length < 1 || !key.StartsWith("-"))
                    continue;
                for (int j = 0; j < argsIn.Length; j++)
                {
                    if (argsIn[j] == key)
                    {
                        if (_commandArgs[key] > 1)
                        {
                            object[] elements = new object[_commandArgs[key]];
                            for (int i = 0; i < _commandArgs[key]; i++)
                            {
                                elements[i] = argsIn[j + i + 1];
                            }
                            argsOut.Add(elements);
                        }
                        else
                        {
                            argsOut.Add(Convert.ChangeType(argsIn[j + 1], _commandArgTypes[index - firstelements.Count]));
                        }
                        break;
                    }
                    if (j + 1 == argsIn.Length)
                    {
                        argsOut.Add(null);
                    }
                }
                index++;
            }
            object[] output = argsOut.ToArray();
            _command.DynamicInvoke(output);
        }

        public void SetCommand(string commandName)
        {

        }

        public void AddParameter(string parameterName, bool overwrites)
        {

        }

        public Type TypeOf(char typeIdentifier)
        {
            switch (char.ToLower(typeIdentifier))
            {
                case 'b':
                    return typeof(bool);
                case 's':
                    return typeof(string);
                case 'i':
                    return typeof(int);
                case 'l':
                    return typeof(long);
                case 'f':
                    return typeof(float);
                case 'd':
                    return typeof(double);
                default:
                    throw new KeyNotFoundException("Unknown Type");
            }
        }
    }
}
