﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VolleyManagement.Services.ServiceResources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VolleyManagement.Services.ServiceResources.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Дивизионы не могут иметь одинаковые названия.
        /// </summary>
        internal static string DivisionsAreNotUniq {
            get {
                return ResourceManager.GetString("DivisionsAreNotUniq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Список дивизионов пуст.
        /// </summary>
        internal static string DivisionsListIsEmpty {
            get {
                return ResourceManager.GetString("DivisionsListIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Релзультат игры по указаному id не был найден.
        /// </summary>
        internal static string GameResultsNotFound {
            get {
                return ResourceManager.GetString("GameResultsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Количество дивизионов в турнире должно быть между {0} и {1}.
        /// </summary>
        internal static string OutOfDivisionsCountRange {
            get {
                return ResourceManager.GetString("OutOfDivisionsCountRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Игрок является капитаном другой команды.
        /// </summary>
        internal static string PlayerIsCaptainOfAnotherTeam {
            get {
                return ResourceManager.GetString("PlayerIsCaptainOfAnotherTeam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Игрок с указанным Id не был найден.
        /// </summary>
        internal static string PlayerNotFound {
            get {
                return ResourceManager.GetString("PlayerNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Команда с указанным Id не была найдена.
        /// </summary>
        internal static string TeamNotFound {
            get {
                return ResourceManager.GetString("TeamNotFound", resourceCulture);
            }
        }
    }
}
