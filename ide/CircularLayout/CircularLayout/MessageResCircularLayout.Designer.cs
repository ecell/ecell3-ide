﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:2.0.50727.3082
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecell.IDE.Plugins.CircularLayout {
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
    internal class MessageResCircularLayout {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MessageResCircularLayout() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ecell.IDE.Plugins.CircularLayout.MessageResCircularLayout", typeof(MessageResCircularLayout).Assembly);
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
        ///   Circular に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string MenuItemCircular {
            get {
                return ResourceManager.GetString("MenuItemCircular", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Please select more than two nodes for circular layout ! に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string MsgLessNode {
            get {
                return ResourceManager.GetString("MsgLessNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Please select nodes as they form a rectangle に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string MsgSelectRect {
            get {
                return ResourceManager.GetString("MsgSelectRect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Nodes will be layouted circularly に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ToolTip {
            get {
                return ResourceManager.GetString("ToolTip", resourceCulture);
            }
        }
    }
}
