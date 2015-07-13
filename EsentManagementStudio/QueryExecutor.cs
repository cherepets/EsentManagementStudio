using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using FourToolkit.Esent;

namespace EsentManagementStudio
{
    internal static class QueryExecutor
    {
        public static object Execute(string expression, EsentDatabase db, out bool success)
        {
            try
            {
                success = true;
                var provider = CodeDomProvider.CreateProvider("CSharp");
                var parameters = new CompilerParameters
                {

                    CompilerOptions = "/t:library",
                    GenerateInMemory = true
                };
                var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => a.Location);
                parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
                var code = @"
using FourToolkit.Esent;
using System;
using System.Data;
using System.Linq;
using System.Xml;
namespace TempAssembly
{
    public class CodeRunner
    {
        public object Execute(EsentDatabase db)
        {
            return " + expression + @";
        }
    }
}";
                var compilationResult = provider.CompileAssemblyFromSource(parameters, code);
                if (compilationResult.Errors.HasErrors)
                    throw new Exception(compilationResult.Errors[0].ErrorText);
                var assembly = compilationResult.CompiledAssembly;
                var instance = assembly.CreateInstance("TempAssembly.CodeRunner");
                if (instance != null)
                {
                    var type = instance.GetType();
                    var method = type.GetMethod("Execute");
                    return method.Invoke(instance, new object[] { db });
                }
                return null;
            }
            catch (TargetInvocationException targetException)
            {
                success = false;
                return $"Exception: {targetException.InnerException?.Message} {targetException.InnerException?.StackTrace}";
            }
            catch (Exception exception)
            {
                success = false;
                return $"Exception: {exception.Message} {exception.StackTrace}";
            }
        }
    }
}