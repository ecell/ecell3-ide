﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:2.0.50727.832
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EcellLib.PropertyWindow {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MessageResProperty {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MessageResProperty() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EcellLib.PropertyWindow.MessageResProperty", typeof(MessageResProperty).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
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
        ///   Get exception while system change the property of object. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrChanged {
            get {
                return ResourceManager.GetString("ErrChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Get exception while system create the bitmap of this window. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrCreBitmap {
            get {
                return ResourceManager.GetString("ErrCreBitmap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Format Error. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrFormat {
            get {
                return ResourceManager.GetString("ErrFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   There are invalid characters in ID. に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ErrID {
            get {
                return ResourceManager.GetString("ErrID", resourceCulture);
            }
        }
    }
}
