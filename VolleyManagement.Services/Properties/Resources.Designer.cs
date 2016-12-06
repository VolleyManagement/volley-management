﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VolleyManagement.Services.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VolleyManagement.Services.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Gmail message is invalid..
        /// </summary>
        internal static string ArgumentExceptionInvalidGmailMessage {
            get {
                return ResourceManager.GetString("ArgumentExceptionInvalidGmailMessage", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Gmail address is invalid..
        /// </summary>
        internal static string ArgumentNullExceptionInvalidGmailAddress {
            get {
                return ResourceManager.GetString("ArgumentNullExceptionInvalidGmailAddress", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Gmail password is invalid..
        /// </summary>
        internal static string ArgumentNullExceptionInvalidGmailPassword {
            get {
                return ResourceManager.GetString("ArgumentNullExceptionInvalidGmailPassword", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Thank you for your feedback!.
        /// </summary>
        internal static string FeedbackConfirmationLetterBody {
            get {
                return ResourceManager.GetString("FeedbackConfirmationLetterBody", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to User notification.
        /// </summary>
        internal static string FeedbackConfirmationLetterSubject {
            get {
                return ResourceManager.GetString("FeedbackConfirmationLetterSubject", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Feedback №{0}, 
        ///Date: {1}.
        ///User email: {2},
        ///Status: {3}
        ///Content: {4}.
        /// </summary>
        internal static string FeedbackEmailBodyToAdmins {
            get {
                return ResourceManager.GetString("FeedbackEmailBodyToAdmins", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Feedback №{0}.
        /// </summary>
        internal static string FeedbackEmailSubjectToAdmins {
            get {
                return ResourceManager.GetString("FeedbackEmailSubjectToAdmins", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to GoogleEmailAddress.
        /// </summary>
        internal static string GmailAddress {
            get {
                return ResourceManager.GetString("GmailAddress", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to EmailMessage.
        /// </summary>
        internal static string GmailMessage {
            get {
                return ResourceManager.GetString("GmailMessage", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to GoogleEmailPassword.
        /// </summary>
        internal static string GmailPassword {
            get {
                return ResourceManager.GetString("GmailPassword", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Your apply was confirmed!.
        /// </summary>
        internal static string TournamentRequestConfirmitionLetterBody {
            get {
                return ResourceManager.GetString("TournamentRequestConfirmitionLetterBody", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Tournament Request №{0}, 
        ///User: {1}.
        ///Tournament: {2},
        ///Team: {3}.
        /// </summary>
        internal static string TournamentRequestEmailBodyToAdmins {
            get {
                return ResourceManager.GetString("TournamentRequestEmailBodyToAdmins", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Tournament Request №{0}.
        /// </summary>
        internal static string TournamentRequestEmailSubjectToAdmins {
            get {
                return ResourceManager.GetString("TournamentRequestEmailSubjectToAdmins", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to User notification.
        /// </summary>
        internal static string TournamentRequestLetterSubject {
            get {
                return ResourceManager.GetString("TournamentRequestLetterSubject", resourceCulture);
            }
        }
    }
}
