using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace FW.Tool
{
    public class WSHelper
    {
        /// < summary>           
        /// 动态调用web服务         
        /// < /summary>          
        /// < param name="url">WSDL服务地址< /param> 
        /// < param name="methodname">方法名< /param>           
        /// < param name="args">参数< /param>           
        /// < returns>< /returns>          
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WSHelper.InvokeWebService(url, null, methodname, args);
        }

        /// < summary>          
        /// 动态调用web服务(参数为基本类型)
        /// < /summary>          
        /// < param name="url">WSDL服务地址< /param>
        /// < param name="classname">类名< /param>  
        /// < param name="methodname">方法名< /param>  
        /// < param name="args">参数< /param> 
        /// < returns>< /returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = WSHelper.GetWsClassName(url);
            }
            try
            {
                //获取WSDL   
                System.Net.WebClient web = new System.Net.WebClient();
                Stream stream = web.OpenRead(url + "?WSDL");
                //创建和格式化 WSDL 文档
                ServiceDescription description = ServiceDescription.Read(stream);
                //创建客户端代理代理类
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

                importer.ProtocolName = "Soap"; // 指定访问协议。
                importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档

                //使用 CodeDom 编译客户端代理类
                CodeNamespace cn = new CodeNamespace(@namespace);
                //为代理类添加命名空间，缺省为全局空间        
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);

                ServiceDescriptionImportWarnings warning = importer.Import(cn, ccu);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

                //importer.Import(cn, ccu);
                //CSharpCodeProvider icc = new CSharpCodeProvider();

                //设定编译参数                 
                CompilerParameters parameter = new CompilerParameters();
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = true;
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                //编译代理类                 
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, ccu);

                if (true == result.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in result.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法   
                Assembly assembly = result.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
                // PropertyInfo propertyInfo = type.GetProperty(propertyname);     
                //return propertyInfo.GetValue(obj, null); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        /// < summary>          
        /// (未完成,暂时不能用)动态调用web服务(参数是对象)
        /// < /summary>          
        /// < param name="url">WSDL服务地址< /param>
        /// < param name="classname">类名< /param>  
        /// < param name="methodname">方法名< /param>  
        /// < param name="args">参数< /param> 
        /// < returns>< /returns>
        public static object InvokeWebServiceByObject(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((classname == null) || (classname == ""))
            {
                classname = WSHelper.GetWsClassName(url);
            }
            try
            {
                //获取WSDL   
                System.Net.WebClient web = new System.Net.WebClient();
                Stream stream = web.OpenRead(url + "?WSDL");
                //创建和格式化 WSDL 文档
                ServiceDescription description = ServiceDescription.Read(stream);
                //创建客户端代理代理类
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

                importer.ProtocolName = "Soap"; // 指定访问协议。
                importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档

                //使用 CodeDom 编译客户端代理类
                CodeNamespace cn = new CodeNamespace(@namespace);
                //为代理类添加命名空间，缺省为全局空间        
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);

                ServiceDescriptionImportWarnings warning = importer.Import(cn, ccu);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

                //importer.Import(cn, ccu);
                //CSharpCodeProvider icc = new CSharpCodeProvider();

                //设定编译参数                 
                CompilerParameters parameter = new CompilerParameters();
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = true;
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");
                //编译代理类                 
                CompilerResults result = provider.CompileAssemblyFromDom(parameter, ccu);

                if (true == result.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in result.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法   
                Assembly assembly = result.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                var aa = assembly.CreateInstance("EnterpriseServerBase.WebService.DynamicWebCalling.PriceStationDefine", false);
                var bb = new { Id = Guid.Empty, StationName = "aaa", IsUsing = true };
                //var cc = 
                args = new object[1];
                args[0] = aa;
                return mi.Invoke(obj, args);
                // PropertyInfo propertyInfo = type.GetProperty(propertyname);     
                //return propertyInfo.GetValue(obj, null); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}
