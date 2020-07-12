using Forge.Forms.Annotations;
using Newtonsoft.Json;
using SuperMemoAssistant.Services.UI.Configuration;
using SuperMemoAssistant.Sys.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.TextFormatting
{
  [Form(Mode = DefaultFields.None)]
  [Title("Dictionary Settings",
        IsVisible = "{Env DialogHostContext}")]
  [DialogAction("cancel",
        "Cancel",
        IsCancel = true)]
  [DialogAction("save",
        "Save",
        IsDefault = true,
        Validates = true)]
  public class TextFormattingCfg : CfgBase<TextFormattingCfg>, INotifyPropertyChangedEx
  {
    [Title("Text Formatting Plugin")]
    [Heading("By Jamesb | Experimental Learning")]

    [Heading("Features:")]
    [Text(@"- Bind common text formatting operations to hotkeys.
- Integrates with other plugins, such as Dev Context Menu.")]

    [Heading("Plugin Integration Settings")]
    [Heading("DevContextMenu")]

    // OUTDENT
    [Field(Name = "Add Outdent command to Dev Context Menu?")]
    public bool AddOutdentMenuItem { get; set; } = true;

    // INDENT
    [Field(Name = "Add Indent command to Dev Context Menu?")]
    public bool AddIndentMenuItem { get; set; } = true;

    // JUSTIFY CENTER
    [Field(Name = "Add Justify Center command to Dev Context Menu?")]
    public bool AddJustifyCenterMenuItem { get; set; } = true;

    // JUSTIFY LEFT
    [Field(Name = "Add Justify Left command to Dev Context Menu?")]
    public bool AddJustifyLeftMenuItem { get; set; } = true;

    // JUSTIFY RIGHT
    [Field(Name = "Add Justify Right command to Dev Context Menu?")]
    public bool AddJustifyRightMenuItem { get; set; } = true;

    // JUSTIFY FULL
    [Field(Name = "Add Justify Full command to Dev Context Menu?")]
    public bool AddJustifyFullMenuItem { get; set; } = true;

    // SUPERSCRIPT
    [Field(Name = "Add Superscript command to Dev Context Menu?")]
    public bool AddSuperscriptMenuItem { get; set; } = true;

    // SUBSCRIPT
    [Field(Name = "Add Subscript command to Dev Context Menu?")]
    public bool AddSubscriptMenuItem { get; set; } = true;

    // INSERT LINE
    [Field(Name = "Add Insert Line command to Dev Context Menu?")]
    public bool AddInsertLineMenuItem { get; set; } = true;

    // STRIKETHROUGH
    [Field(Name = "Add Strikethrough command to Dev Context Menu?")]
    public bool AddStrikethroughMenuItem { get; set; } = true;

    [JsonIgnore]
    public bool IsChanged { get; set; }

    public override string ToString()
    {
      return "Text Formatting Settings";
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
